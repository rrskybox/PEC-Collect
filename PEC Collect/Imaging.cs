using System;
using TheSkyXLib;


namespace PEC_Collect
{

    #region AstroImage class
    //AstroImage class is a structure containing all the info to generate an image, and methods for
    //  taking the four types:  Light, Dark, Bias, Flat

    public class AstroImage
    {
        public enum CameraType
        {
            Imaging = 0,
            Guider = 1
        }
        public enum ReductionType
        {
            None = ccdsoftImageReduction.cdNone,
            AutoDark = ccdsoftImageReduction.cdAutoDark,
            BiasDarkFlat = ccdsoftImageReduction.cdBiasDarkFlat
        }

        public enum ImageType
        {
            Bias = ccdsoftImageFrame.cdBias,
            Dark = ccdsoftImageFrame.cdDark,
            Flat = ccdsoftImageFrame.cdFlat,
            Light = ccdsoftImageFrame.cdLight
        }

        private int filter;
        //private int subframeRight;

        public AstroImage()
        {
            TargetName = "";
            Camera = 0;
            Exposure = 0;
            BinX = 1;
            BinY = 1;
            filter = 0;
            Delay = 0;
            Frame = ImageType.Light;
            ImageReduction = ReductionType.None;
            SubFrame = 0;
            SubframeLeft = 0;
            SubframeTop = 0;
            SubframeBottom = 0;
            SubframeRight = 0;
            AutoSave = 1;
        }

        public string TargetName { get; set; }
        public double Exposure { get; set; }
        public CameraType Camera { get; set; }
        public int BinX { get; set; }
        public int BinY { get; set; }
        public int Filter { get => filter; set => filter = value; }
        public double Delay { get; set; }
        public ImageType Frame { get; set; }
        public ReductionType ImageReduction { get; set; }
        public int SubFrame { get; set; }
        public int SubframeLeft { get; set; }
        public int SubframeTop { get; set; }
        public int SubframeBottom { get; set; }
        public int SubframeRight { get; set; }
        public int AutoSave { get; set; }
    }
    #endregion

    #region Imaging Class

    public class Imaging
    {
        public int FilterChangeDelay = 2;

        public string TakeLightFrame(AstroImage asti)
        {
            //Image and save light frame
            //   Turn on autosave
            //   Set exposure length
            //   Set for light frame type
            //   Set for autodark
            //   Check for filter change, if (so) { set for a 5 second delay, otherwise, no delay
            //   Set for asynchronous execution
            //   Start exposure and wait until completed or aborted
            //   Clean up mess and return;

            //Save the current time so we can calculate an overhead later
            DateTime imageStart = DateTime.Now;
            asti.Delay = FilterChangeDelay;
            TSXLink.FilterWheel.Filter = asti.Filter;

            TSXLink.Camera tcam = new TSXLink.Camera(asti)
            {
                //Autosave this image
                AutoSaveOn = 1
            };
            int camResult = tcam.GetImage();
            if (camResult != 0)
            {
                return null;
            }
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(1000);

            //Calculate and save the overhead duration
            DateTime imageEnd = DateTime.Now;
            TimeSpan imageDuration = imageEnd - imageStart;

            return tcam.LastImageFilename();
        }



    }

    #endregion
}
