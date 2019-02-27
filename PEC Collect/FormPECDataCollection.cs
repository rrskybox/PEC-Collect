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
/// V1.0 -- 2/15/19 (approx)
///     1. Release on Software Bisque support forum
/// V1.1 -- 2/27/19
///     1. Various minor bug fixes and GUI updates

using System;
using System.Deployment.Application;
using System.Windows.Forms;
using TheSkyXLib;

namespace PEC_Collect
{
    public partial class FormPECDataCollection : Form
    {
        public FormPECDataCollection()
        {
            InitializeComponent();
            // Acquire the version information and put it in the form header
            try { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch { this.Text = " in Debug"; } //probably in debug, no version info available
            this.Text = "PEC Collect V" + this.Text;
            StarList.InstallDBQ();
        }

        public bool abortFlag = false;

        private void StartButton_Click(object sender, EventArgs e)
        {
            //Initialize stuff, then...

            //Repeat 
            //Move to target area:  Dec =0, HA = .1;
            //Find nearest guide star with magnitude <8 and positive HA (DBQ PEC_Collect.dbq)
            //Center on target star
            //Find star
            //Set Exposure
            //Find star again
            //Run Autoguide for 20 minutes
            //Abort
            //Repeat

            do
            {
                TSXLink.Rotator.Disconnect();

                double altAtZeroDec = 90 - GetLocationLatitude();
                SlewAzAlt(182, altAtZeroDec);

                StarList.TargetStarSearch();
                SlewRADec(StarList.TargetRA, StarList.TargetDec);
                AutoFocus.Check(AtFocus3Checkbox.Checked);
                SlewRADec(StarList.TargetRA, StarList.TargetDec);

                AutoGuide.SetAutoGuideStar();
                AutoGuide.OptimizeExposure();
                AutoGuide.AutoGuideStart();
                int twentyMinutes = (60 * 60 * 20) + 20;
                for (int i = 0; i < twentyMinutes; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    System.Windows.Forms.Application.DoEvents();
                    if (abortFlag) break;
                }
                LoopsCounter.Value -= 1;
            } while ((abortFlag == false) && (LoopsCounter.Value > 0));

        }

        /// <summary>
        /// Method to point mount at RA/Dec coordinates
        /// </summary>
        /// <param name="ra2000"></param>
        /// <param name="dec2000"></param>
        public static void SlewRADec(double ra2000, double dec2000)
        {

            //Convert ra and dec (J2000) to JNow
            sky6Utils tsxu = new sky6Utils();
            tsxu.Precess2000ToNow(ra2000, dec2000);
            double raNow = tsxu.dOut0;
            double decNow = tsxu.dOut1;
            //Make sure this is synchronous wait.
            sky6RASCOMTele tsxm = new sky6RASCOMTele { Asynchronous = 0 };
            tsxm.SlewToRaDec(raNow, decNow, "Guide Target");
        }

        /// <summary>
        /// Method to point mount at Az/Alt coordinates
        /// </summary>
        /// <param name="az"></param>
        /// <param name="alt"></param>
        public static void SlewAzAlt(double az, double alt)
        {
            //Make sure this is synchronous wait.
            sky6RASCOMTele tsxm = new sky6RASCOMTele { Asynchronous = 0 };
            tsxm.SlewToAzAlt(az, alt, "");
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

 
    }
}
