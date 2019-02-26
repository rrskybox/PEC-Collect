using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheSkyXLib;

namespace PEC_Collect
{
    public partial class FormPECDataCollection : Form
    {
        public FormPECDataCollection()
        {
            InitializeComponent();
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
                ccdsoftCamera tsxc = new ccdsoftCamera();
                tsxc.FilterIndexZeroBased = tsxc.FilterIndexZeroBased + 1;
                tsxc.Delay = tsxc.Delay + 1;
                tsxc.AutoguiderExposureTime = tsxc.AutoguiderExposureTime + 1;
                tsxc.TakeImage();


                double altAtZeroDec = 90 - GetDeclinationZeroAltitude();
                SlewAzAlt(182, altAtZeroDec);

                StarList.TargetStarSearch();
                SlewRADec(StarList.TargetRA, StarList.TargetDec);
                //AutoFocus.Check();
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
            } while (abortFlag == false);

        }

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

        public static void SlewAzAlt(double az, double alt)
        {
            //Make sure this is synchronous wait.
            sky6RASCOMTele tsxm = new sky6RASCOMTele { Asynchronous = 0 };
            tsxm.SlewToAzAlt(az, alt, "");
        }

        public static double GetDeclinationZeroAltitude()
        {
            return (20);
        }



        private void EndButton_Click(object sender, EventArgs e)
        {
            abortFlag = true;
            AutoGuide.AutoGuideStop();
        }
    }
}
