/// PEC Collect
/// Windows automation for generating multiple TSX tracking logs
/// ------------------------------------------------------------------------
/// Module Name: PEC Collect
/// Purpose: Iterative generation of multiple tracking logs in tSX
/// Developer: Rick McAlister
/// Creation Date:  2/15/19
/// Copyright: Rick McAlister, 2019
/// 
/// Version Information
/// V1.0 -- Basic features
///     V1.1 Release on Software Bisque support forum
///     V1.2 Added image mirroring detect and output


using System;
using System.Deployment.Application;
using System.Windows.Forms;
using TheSkyXLib;

namespace PEC_Collect
{
    public partial class FormPECCollect : Form
    {
        public FormPECCollect()
        {
            InitializeComponent();
            // Acquire the version information and put it in the form header
            try { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch { this.Text = " in Debug"; } //probably in debug, no version info available
            this.Text = "PEC Collect V" + this.Text;
            StarList.InstallDBQ();
            BinningListBox.SelectedIndex = 0;
            FocusComboBox.SelectedIndex = 2;
        }

        public bool abortFlag = false;

        public int Binning { get; set; } = 1;  // 1=1x1, 2=2x2, 3=3x3

        private void StartButton_Click(object sender, EventArgs e)
        {



            //Initialize stuff
            //Verify camera orientation near 0 degrees

            //Repeat 
            //      Move to target area:  Dec =0, HA = .1;
            //      Find nearest guide star with magnitude <8 and positive HA (DBQ PEC_Collect.dbq)
            //      Center on target star
            //      Find star
            //      Set Exposure
            //      Find star again
            //      Run Autoguide for 20 minutes
            //      Abort
            //Loop

            //Set Binning
            AutoGuide.GuiderXBinning = Binning;
            AutoGuide.GuiderYBinning = Binning;

            //Check orientation
            //slew to west side near meridian at 0 deg declination
            double altAtZeroDec = 90 - GetLocationLatitude();
            TSXLink.ReliableAzAltSlew(183, altAtZeroDec,"");

            //  Take image, image link it and get Position Angle
            AstroImage asti = new AstroImage
            {
                Camera = AstroImage.CameraType.Imaging,
                Frame = AstroImage.ImageType.Light,
                Delay = 0,
                Exposure = 10,
                ImageReduction = AstroImage.ReductionType.AutoDark,
                BinX = Binning,
                BinY = Binning
            };
            if (PACheckBox.Checked)
            {
                TSXLink.Camera gCam = new TSXLink.Camera(asti) { AutoSaveOn = 1 };
                int tstat = gCam.GetImage();
                TSXLink.PlateSolution psln = TSXLink.ImageSolution.PlateSolve(gCam.LastImageFilename());
                //Check for failed image solution.  If so, just return as an error message will have bee3n posted already.
                if (psln == null) return;
                //Check for the image PA to be 0 +/- 3 degrees
                //  if not then give option to opt out.
                if (psln.ImagePA > 3)
                {
                    if (psln.ImagePA < 357)
                    {
                        DialogResult dr = MessageBox.Show(("PA " + (int)psln.ImagePA + "is not near zero.  Continue?"),
                                                            "Camera Orientation Error",
                                                            MessageBoxButtons.YesNo);
                        if (dr == DialogResult.No) return;
                    }
                }
                //Add info to text output:
                if (psln.ImageIsMirrored) OutputTextBox.Text += "\n\r\n\r" + "  --> Plate Solved PA = " + ((int)psln.ImagePA).ToString() + " degrees and image is mirrored (check West in TCS).";
                else OutputTextBox.Text += "\n\r\n\r" + "  -->Plate Solved PA = " + ((int)psln.ImagePA).ToString() + " degrees and image is not mirrored (do not check West in TCS).";
            }
            //Passed the orientation test, west side

            CompletionTime.Text = (DateTime.Now.AddMinutes((double)LoopsCounter.Value * (double)DurationMinutes.Value)).ToString("HH:mm:ss");

            //Loops
            do
            {
                //Set loop number and time left
                TimeLeft.Text = (TimeSpan.FromMinutes((int)DurationMinutes.Value).ToString("T"));

                //Reset Camera
                AstroImage nasti = new AstroImage
                {
                    Camera = AstroImage.CameraType.Imaging,
                    Frame = AstroImage.ImageType.Light,
                    Delay = 0,
                    Exposure = 10,
                    SubFrame = 0,
                    ImageReduction = AstroImage.ReductionType.AutoDark,
                    BinX = Binning,
                    BinY = Binning
                };
                TSXLink.Camera ngCam = new TSXLink.Camera(asti) { AutoSaveOn = 1 };

                //Slew to just west of the meridian at 0 deg Declination
                sky6StarChart tsxsc = new sky6StarChart();
                TSXLink.ReliableAzAltSlew(183, altAtZeroDec,"");
                //find a target star here
                StarList.TargetStarSearch();
                //slew to and center on the found star
                int cls1 = TSXLink.ReliableClosedLoopSlew(StarList.TargetRA,StarList.TargetDec,StarList.TargetName);
                if (cls1 != 0) return;
                //check the focus using this star
                // selected value = 0 for @focus2
                // selected value = 1 for @focus3
                // selected value = 2 for none
                switch (FocusComboBox.SelectedIndex)
                {
                    case 0:
                        AutoFocus.Check(false);
                        break;
                    case 1:
                        AutoFocus.Check(true);
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
                AutoGuide.GuiderXBinning = Binning;
                AutoGuide.GuiderYBinning = Binning;
                //Center up the target star as guide star
                AutoGuide.SetAutoGuideStar();
                //Optimize it's exposure level
                AutoGuide.GuideExposure = AutoGuide.OptimizeExposure();
                //Start tracking
                bool gsStatus = AutoGuide.AutoGuideStart();

                //Wait for the duration, checking for abort (e.g. End)
                int durationSecs = (60 * (int)DurationMinutes.Value);
                for (int i = 0; i < durationSecs; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    TimeLeft.Text = (TimeSpan.FromSeconds(durationSecs - i).ToString("T"));
                    this.Show();
                    System.Windows.Forms.Application.DoEvents();
                    if (abortFlag) break;
                }
                //Wait is over, kill the autoguiding/tracking, increment the loop counter and loop
                AutoGuide.AutoGuideStop();
                LoopsCounter.Value -= 1;
                //Check for pause -- used for changing PEC Curves when experimenting
                if (PauseCheckBox.Checked)
                {
                    MessageBox.Show("Pausing for configuration changes, if any.");
                }

                /////////////************************///////////////
                //Binning sensitivity experiment -- remove aftwr completed
                //Check to see if BinSwap is enabled
                //  if so, then if binning is 1x1, change to 2x2 and vis versa
                if (BinSwapCheckbox.Checked)
                {
                    if (Binning == 1)
                    {
                        BinningListBox.SelectedIndex = 1;
                        Binning = 2;
                    }
                    else
                    {
                        BinningListBox.SelectedIndex = 0;
                        Binning = 1;
                    }
                }
                /////////////************************////////////////

            } while ((abortFlag == false) && (LoopsCounter.Value > 0));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double GetLocationLatitude()
        {
            //Get and return the latitude of the current TSX star chart (as set by location)
            sky6StarChart tsxsc = new sky6StarChart();
            tsxsc.DocumentProperty(Sk6DocumentProperty.sk6DocProp_Latitude);
            double lat = tsxsc.DocPropOut;
            return (lat);
        }

        /// <summary>
        /// Handles the "Stop" button to shut down the guider and iterations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndButton_Click(object sender, EventArgs e)
        {
            abortFlag = true;
            AutoGuide.AutoGuideStop();
        }

        private void BinningListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Sets binning
            Binning = BinningListBox.SelectedIndex + 1;
            return;
        }


    }
}
