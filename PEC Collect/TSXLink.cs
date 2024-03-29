﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using TheSky64Lib;



//Library of methods and data for interfacing to TSX through .NET (COM) library
namespace PEC_Collect
{
    public partial class TSXLink
    {

        #region SexTractor

        public class SexTractor
        {
            // Added enumeration of inventory index because TSX doesn't
            public enum SourceExtractionType
            {
                sexX,
                sexY,
                sexMagnitude,
                sexClass,
                sexFWHM,
                sexMajorAxis,
                sexMinorAxis,
                sexTheta,
                sexEllipticity
            }

            public ccdsoftImage timg = null;

            public SexTractor()
            {
                //ccdsoftCamera tsxa = new ccdsoftCamera();
                timg = new ccdsoftImage();
                return;
            }

            public void Close()
            {
                return;
            }

            public bool SourceExtractGuider()
            {
                int iStat, aStat;
                try
                {
                    aStat = timg.AttachToActiveImager();
                    iStat = timg.ShowInventory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
                return true;
            }

            //*** Converts an array of generic "objects" to an array of doubles
            private double[] ConvertDoubleArray(object[] oIn)
            {
                double[] dOut = new double[oIn.Length];
                for (int i = 0; i < oIn.Length; i++)
                {
                    dOut[i] = Convert.ToDouble(oIn[i]);
                }
                return dOut;
            }

            //*** Converts an array of generic "objects" to a list of doubles
            private List<double> ConvertDoubleList(object[] oIn)
            {
                List<double> dOut = new List<double>();
                for (int i = 0; i < oIn.Length; i++)
                {
                    dOut.Add(Convert.ToDouble(oIn[i]));
                }
                return dOut;
            }

            //*** returns the index of the largest value found in a list
            public int GetListLargest(List<double> iArray)
            {
                int idx = 0;
                for (int i = 0; i < iArray.Count; i++)
                {
                    if (iArray[i] > iArray[idx]) { idx = i; }
                }
                return idx;
            }

            //*** Get the ADU value at pixel X,Y
            public double GetPixelADU(int xPix, int yPix)
            {
                //get the array height and width
                int aHeight = timg.HeightInPixels;
                int aWidth = timg.WidthInPixels;
                //need to some out of bounds checking here someday
                //var aRow = timg.scanLine(yPix);
                //double aVal = aRow[xPix];
                double aVal = (double)timg.scanLine(yPix)[xPix];
                return aVal;
            }

            public double[] GetSourceExtractionArray(SourceExtractionType dataIndex)
            {
                {
                    //Array.ConvertAll((Link.TSXSend(TSXCLASS + "InventoryArray(" + InventoryIndex.ToString() + ")")).Split(','), Double.Parse);
                    //Array.ConvertAll<object, double>(objects, o => (double)o);
                    //double[] sexArray = Array.ConvertAll(timg.InventoryArray((int)dataIndex), double.Parse);
                    //var doub = obj.Cast<double>();
                    //double[] sexArray = Array.ConvertAll<object, double>(timg.InventoryArray((int)dataIndex), o => (double)o);
                    //double[] sexArray = timg.InventoryArray((int)dataIndex).Cast<double>();
                    //double[] sexArray = timg.InventoryArray((int)dataIndex);
                    object[] iA = timg.InventoryArray((int)dataIndex);
                    double[] sexArray = ConvertDoubleArray(iA);
                    return sexArray;
                }

            }

            public List<double> GetSourceExtractionList(SourceExtractionType dataIndex)
            {
                {
                    object[] iA = timg.InventoryArray((int)dataIndex);
                    //List<double> sexArray = timg.InventoryArray((int)dataIndex).Cast<double>();
                    //double[] iA = timg.InventoryArray((int)dataIndex);
                    //List<double> sexArray = iA.ToList<double>();
                    List<double> sexArray = ConvertDoubleList(iA);
                    return sexArray;
                }

            }

            public int WidthInPixels => timg.WidthInPixels;

            public int HeightInPixels => timg.HeightInPixels;

        }

        #endregion

        #region filterwheel
        public partial class FilterWheel
        {
            public static List<string> FilterWheelList()
            {

                ccdsoftCamera tsxc = new ccdsoftCamera();
                //Connect the camera, if fails, then just return after clean up
                try
                { tsxc.Connect(); }
                catch
                {
                    return null;
                }
                List<string> tfwList = new List<string>();
                for (int filterIndex = 0; filterIndex < tsxc.lNumberFilters; filterIndex++)
                {
                    tfwList.Add(tsxc.szFilterName(filterIndex));
                }
                return tfwList;
            }

            public static int Filter
            {
                get
                {
                    ccdsoftCamera tsxc = new ccdsoftCamera();
                    int fi = tsxc.FilterIndexZeroBased;
                    return fi;
                }
                set
                {
                    ccdsoftCamera tsxc = new ccdsoftCamera
                    {
                        FilterIndexZeroBased = value
                    };
                    return;
                }
            }
        }


        #endregion

        #region Camera Class

        public class Camera
        {
            private ccdsoftCamera tsxc;

            public Camera(AstroImage asti)
            {
                tsxc = new ccdsoftCamera
                {
                    Autoguider = (int)asti.Camera,
                    BinX = asti.BinX,
                    BinY = asti.BinY,
                    Delay = asti.Delay,
                    Frame = (ccdsoftImageFrame)asti.Frame,
                    ImageReduction = (ccdsoftImageReduction)asti.ImageReduction,
                    Subframe = asti.SubFrame,
                    SubframeBottom = asti.SubframeBottom,
                    SubframeTop = asti.SubframeTop,
                    SubframeRight = asti.SubframeRight,
                    SubframeLeft = asti.SubframeLeft,
                    AutoSaveOn = asti.AutoSave,
                    ExposureTime = asti.Exposure,
                    AutoguiderExposureTime = asti.Exposure
                };
            }

            public int GetImage()
            {
                //Takes an image asynchronously using TSX
                Asynchronous = 1;

                tsxc.AutoguiderExposureTime = tsxc.ExposureTime;

                try
                { tsxc.TakeImage(); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return ex.HResult;
                }

                //Wait while the image is being taken, using 1 second naps.  Check each time to see
                //  if (the user has hit abort.  if (so, close everything up.
                int expstatus = 0;
                while (expstatus == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    try
                    { expstatus = tsxc.IsExposureComplete; }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return ex.HResult;
                    }
                }
                return 0;
            }

            public void CameraAbort()
            {
                tsxc.Abort();
                return;
            }

            public string LastImageFilename()
            {
                string path = tsxc.LastImageFileName;
                return path;
            }

            public double CCDTemperature
            {
                //reads the current ccd temperature and writes the set point
                get => tsxc.Temperature;
                set
                {
                    tsxc.TemperatureSetPoint = value;
                    tsxc.RegulateTemperature = 1;
                }
            }

            public int WidthInPixels => tsxc.WidthInPixels;

            public int HeightInPixels => tsxc.HeightInPixels;

            public int ImageADU
            {
                get
                {
                    ccdsoftImage tsxi = new ccdsoftImage();
                    tsxi.AttachToActiveImager();
                    int adu = (int)tsxi.averagePixelValue();
                    return adu;
                }
            }

            public double MaximumPixel => tsxc.MaximumPixel;

            public int Asynchronous
            { set => tsxc.Asynchronous = value; }

            public int AutoSaveOn
            { set => tsxc.AutoSaveOn = value; }

            public int IsExposureComplete => tsxc.IsExposureComplete;

            public int Autoguide()
            {
                tsxc.Asynchronous = 1;
                return tsxc.Autoguide();
            }

            public int Calibrate(int AO)
            { return tsxc.Calibrate(AO); }

            public ccdsoftCameraState State => tsxc.State;

            public int CenterAO()
            {
                int retVal;
                try { retVal = tsxc.centerAO(); }
                catch { retVal = -1; }
                return retVal;
            }

            public double GuideStarX
            {
                get => tsxc.GuideStarX;
                set => tsxc.GuideStarX = value;
            }

            public double GuideStarY
            {
                get => tsxc.GuideStarY;
                set => tsxc.GuideStarY = value;
            }

            public int BinX
            {
                get => tsxc.BinX;
                set => tsxc.BinX = value;
            }

            public int BinY
            {
                get => tsxc.BinY;
                set => tsxc.BinY = value;
            }

            public double CalibrationVectorXPositiveXComponent
            {
                get => tsxc.CalibrationVectorXPositiveXComponent;
                set => tsxc.CalibrationVectorXPositiveXComponent = value;
            }

            public double CalibrationVectorXPositiveYComponent
            {
                get => tsxc.CalibrationVectorXPositiveYComponent;
                set => tsxc.CalibrationVectorXPositiveYComponent = value;
            }

            public double CalibrationVectorYPositiveXComponent
            {
                get => tsxc.CalibrationVectorYPositiveXComponent;
                set => tsxc.CalibrationVectorYPositiveXComponent = value;
            }

            public double CalibrationVectorYPositiveYComponent
            {
                get => tsxc.CalibrationVectorYPositiveYComponent;
                set => tsxc.CalibrationVectorYPositiveYComponent = value;
            }

            public double CalibrationVectorXNegativeXComponent
            {
                get => tsxc.CalibrationVectorXNegativeXComponent;
                set => tsxc.CalibrationVectorXNegativeXComponent = value;
            }

            public double CalibrationVectorXNegativeYComponent
            {
                get => tsxc.CalibrationVectorXNegativeYComponent;
                set => tsxc.CalibrationVectorXNegativeYComponent = value;
            }

            public double CalibrationVectorYNegativeXComponent
            {
                get => tsxc.CalibrationVectorYNegativeXComponent;
                set => tsxc.CalibrationVectorYNegativeXComponent = value;
            }

            public double CalibrationVectorYNegativeYComponent
            {
                get => tsxc.CalibrationVectorYNegativeYComponent;
                set => tsxc.CalibrationVectorYNegativeYComponent = value;
            }

            public double DeclinationAtCalibration => tsxc.DeclinationAtCalibration;

            public int TrackBoxX
            {
                get => tsxc.TrackBoxX;
                set => tsxc.TrackBoxX = value;
            }

            public int TrackBoxY
            {
                get => tsxc.TrackBoxY;
                set => tsxc.TrackBoxY = value;
            }

            public double GuideErrorX => tsxc.GuideErrorX;

            public double GuideErrorY => tsxc.GuideErrorY;

            public int SubframeBottom
            {
                get => tsxc.SubframeBottom;
                set => tsxc.SubframeBottom = value;
            }

            public int SubframeTop
            {
                get => tsxc.SubframeTop;
                set => tsxc.SubframeTop = value;
            }

            public int SubframeLeft
            {
                get => tsxc.SubframeLeft;
                set => tsxc.SubframeLeft = value;
            }

            public int SubframeRight
            {
                get => tsxc.SubframeRight;
                set => tsxc.SubframeRight = value;
            }

            public void Calibrate(bool AO)
            {
                //Make sure there is a delay for the mount to settle
                tsxc.Delay = 2;
                if (!AO)
                    try
                    { int calstat = tsxc.Calibrate(0); } //1 for AO, anything else for not AO(autoguider)            
                    catch { return; }
                //wait for completion;
                while (tsxc.State == ccdsoftCameraState.cdStateCalibrate)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                //Also run AO calibration, if configured
                if (AO)
                {
                    //lg.LogIt("Calibrating AO ");
                    try
                    { int calstat = tsxc.Calibrate(1); } //1 for AO, anything else for not AO(autoguider)            
                    catch
                    { return; }
                    //wait for completion;
                    while (tsxc.State == ccdsoftCameraState.cdStateCalibrate)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            public bool AutoGuiderOn()
            {
                //Turn on Asynchronous, then turn on autoguide and return status
                tsxc.Asynchronous = 0;
                AutoGuiderOff();
                tsxc.Asynchronous = 1;
                int tsxStat = tsxc.Autoguide();
                if (tsxStat != 0) return false;
                else return true;
            }

            public void AutoGuiderOff()
            {
                //Turn off Asynchronous, then turn off autoguide
                tsxc.Asynchronous = 0;
                tsxc.Abort();
                return;
            }

            public bool IsAutoGuideOn()
            {
                //Returns true is the autoguider is running
                ccdsoftCameraState agState = tsxc.State;
                if (agState == ccdsoftCameraState.cdStateAutoGuide) return true;
                else return false;
            }
        }
        #endregion

        #region Focus Class

        public partial class Focus
        {
            public static bool RunAtFocusAny(AstroImage asti, int aftype)
            {
                //Run @Focus2 or 3 with filternumber.  return True if (successful, false if (not

                //   Before running this method, save the current target name and camera configuration so it can be found again 
                //   Restore current target, using Name with Find method, and reload camera configuration afterwards
                //   as @Focus3 using automatic star search off, overwrites the observating list and object)
                //   Also, may want to run Closed}Slew back to target and Turn on temperature compensation (A)
                //

                //Make sure focuser is connected
                //Create camera object
                ccdsoftCamera tsxc = new ccdsoftCamera
                {
                    Asynchronous = 0,
                    AutoSaveFocusImages = 0,
                    ImageReduction = (ccdsoftImageReduction)asti.ImageReduction,
                    Frame = (ccdsoftImageFrame)asti.Frame,
                    FilterIndexZeroBased = asti.Filter,
                    FocusExposureTime = asti.Exposure,
                    Delay = 0
                };
                //Run @Focus2 or 3
                //   Create a camera object
                //   Launch the autofocus watching out for an exception -- which will be posted in TSX
                switch (aftype)
                {
                    case 2:
                        try
                        { int focstat = tsxc.AtFocus2(); }
                        catch
                        {
                            //Just close up, TSX will spawn error window unless this is an abort
                            //lg.LogIt("@Focus2 fails for " + ex.Message);
                            return (false);
                        }
                        //@Focus2 will generate an observing list.  Clear it.
                        break;

                    case 3:
                        try
                        { int focstat = tsxc.AtFocus3(3, true); }
                        catch
                        {
                            //Just close up, TSX will spawn error window unless this is an abort
                            //lg.LogIt("@Focus3 fails for " + ex.Message);
                            return (false);
                        }
                        //lg.LogIt("@Focus3 successful");
                        break;
                    default:
                        // lg.LogIt("Unknown AtFocus selection -- focus failed");
                        break;
                }
                return true;
            }

            public static void RunTempComp()
            {
                //Enable Temperature compensation with current data to mode A (for Optec)
                // lg = FormHumason.LogReport;

                ccdsoftCamera tsxc = new ccdsoftCamera
                {
                    focTemperatureCompensationMode = ccdsoftfocTempCompMode.cdfocTempCompMode_A
                };
                //lg.LogIt("Focuser Temperature Compensation turned on");
                return;
            }

            public static double GetTemperature()
            {
                ccdsoftCamera tsxc = new ccdsoftCamera();
                double celsius = tsxc.focTemperature;
                return celsius;
            }

            public static int MoveTo(double position)
            {
                int movestat;
                ccdsoftCamera tsxc = new ccdsoftCamera();
                if (position < tsxc.focPosition)
                {
                    movestat = tsxc.focMoveIn((int)(tsxc.focPosition - position));
                }
                else
                {
                    movestat = tsxc.focMoveOut((int)(position - tsxc.focPosition));
                }
                return movestat;
            }
        }

        #endregion

        #region Rotator
        public class Rotator
        {
            public static void Disconnect()
            {
                ccdsoftCamera tsxr = new ccdsoftCamera();
                try { tsxr.Asynchronous = 0; }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                try { tsxr.rotatorDisconnect(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        #endregion

        #region Dome
        public static bool IsDomeTrackingUnderway()
        {
            //Test to see if a dome tracking operation is underway.
            // If so, doing a IsGotoComplete will throw an Error 212.
            // return true
            // otherwise return false
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsConnected != 0)
            {
                int testDomeTrack;
                try { testDomeTrack = tsxd.IsGotoComplete; }
                catch { return true; }
                if (testDomeTrack == 0) return true;
                else return false;
            }
            else return false;
        }

        public static void ToggleDomeCoupling()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsConnected != 0)
            {
                tsxd.IsCoupled = 0;
                System.Threading.Thread.Sleep(1000);
                tsxd.IsCoupled = 1;
                //Wait for all dome activity to stop
                while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            }
            return;
        }

        public static void DomeCouplingOn()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsConnected != 0)
            {
                tsxd.IsCoupled = 1;
                System.Threading.Thread.Sleep(500);
                while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            }
            return;
        }

        public static void DomeCouplingOff()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsConnected != 0)
            {
                tsxd.IsCoupled = 0;
                System.Threading.Thread.Sleep(500);
                while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            }
            return;
        }
        #endregion

        #region Image Linking
        public static class ImageSolution
        {
            public static PlateSolution PlateSolve(string path)
            {
                sky6MyFOVs tsxf = new sky6MyFOVs();
                tsxf.Property(FOVName(), 0, sk6MyFOVProperty.sk6MyFOVProp_Scale);
                double scale = tsxf.OutVar;

                ImageLink tsxl = new ImageLink
                {
                    pathToFITS = path
                };
                //set Scale
                tsxl.scale = scale;
                try
                { tsxl.execute(); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                ImageLinkResults tsxr = new ImageLinkResults();
                ccdsoftCamera tcam = new ccdsoftCamera();
                PlateSolution ipa = new PlateSolution
                {
                    ImageRA = tsxr.imageCenterRAJ2000,
                    ImageDec = tsxr.imageCenterDecJ2000,
                    ImagePA = tsxr.imagePositionAngle,
                    ImageIsMirrored = Convert.ToBoolean(tsxr.imageIsMirrored)
                };
                return ipa;
            }

        }

        private static string FOVName()
        {
            string fovName = null;
            sky6MyFOVs tsxf = new sky6MyFOVs();
            for (int i = 0; i < tsxf.Count; i++)
            {
                tsxf.Name(i);
                fovName = tsxf.OutString;
                tsxf.Property(fovName, 0, sk6MyFOVProperty.sk6MyFOVProp_Visible);
                double vis = tsxf.OutVar;
                if (vis == 1)
                {
                    return fovName;
                }
            }
            return null;
        }

        #endregion

        #region Slew


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
            //sky6RASCOMTele tsxm = new sky6RASCOMTele { Asynchronous = 0 };
            //tsxm.SlewToRaDec(raNow, decNow, "Guide Target");
            ReliableRADecSlew(raNow, decNow, "Guide Target");
            TSXLink.DomeCouplingOff();
            ClosedLoopSlew tsxcls = new ClosedLoopSlew();
            try { tsxcls.exec(); }
            catch
            {
                TSXLink.DomeCouplingOn();
                return;
            }
            TSXLink.DomeCouplingOn();
        }

        /// <summary>
        /// Method to point mount at Az/Alt coordinates
        /// </summary>
        /// <param name="az"></param>
        /// <param name="alt"></param>
        public static void SlewAzAlt(double az, double alt)
        {
            //Make sure this is synchronous wait.
            //sky6RASCOMTele tsxm = new sky6RASCOMTele { Asynchronous = 0 };
            //tsxm.SlewToAzAlt(az, alt, "");
            ReliableAzAltSlew(az, alt, "");
            return;
        }

        public static void ReliableRADecSlew(double RA, double Dec, string name)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn't solve race condition, but may avoid 
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            while (TSXLink.IsDomeTrackingUnderway()) System.Threading.Thread.Sleep(500);
            int result = -1;
            while (result != 0)
            {
                result = 0;
                try { tsxt.SlewToRaDec(RA, Dec, name); }
                catch (Exception ex) { result = ex.HResult - 1000; }
            }
            return;
        }

        public static void ReliableAzAltSlew(double RA, double Dec, string name)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn't solve race condition, but may avoid 
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            while (TSXLink.IsDomeTrackingUnderway()) System.Threading.Thread.Sleep(500);
            int result = -1;
            while (result != 0)
            {
                result = 0;
                try { tsxt.SlewToAzAlt(RA, Dec, name); }
                catch (Exception ex) { result = ex.HResult - 1000; }
            }
            return;
        }

        public static int ReliableClosedLoopSlew(double RA, double Dec, string name)
        {
            //Tries to perform CLS without running into dome tracking race condition
            //
            //Slew close to target, with the dome chasing.
            //Turn off tracking and do a CLS.  This prevents ceasing up 
            // on the dome tracking error race condition
            //Wait until the CLS completes, then turn the dome tracking back on
            //
            ReliableRADecSlew(RA, Dec, name);
            //Turn off tracking
            TSXLink.DomeCouplingOff();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            int clsStatus = 123;
            while (clsStatus == 123)
            {
                try { clsStatus = tsx_cl.exec(); }
                catch (Exception ex)
                {
                    clsStatus = ex.HResult - 1000;
                };
                if (clsStatus == 123) System.Threading.Thread.Sleep(500);
            }
            TSXLink.DomeCouplingOn();
            return clsStatus;
        }




        #endregion

        #region Plate Solution Structure

        public class PlateSolution
        {
            public PlateSolution()
            {
                ImageRA = 0;
                ImageDec = 0;
                ImagePA = 0;
                RotatorPositionAngle = 0;
                ImageIsMirrored = false;
            }

            public double ImageRA { get; set; } = 0;
            public double ImageDec { get; set; } = 0;
            public double ImagePA { get; set; } = 0;
            public double RotatorPositionAngle { get; set; } = 0;
            public bool ImageIsMirrored { get; set; } = false;
        }
        #endregion

    }
}



