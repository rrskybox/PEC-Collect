using System;
using System.Collections.Generic;
using TheSkyXLib;
using System.Windows.Forms;

namespace PEC_Collect
{
    public static partial class AutoGuide
    {
        public static double GuideStarX { get; set; }
        public static double GuideStarY { get; set; }
        public static double GuideExposure { get; set; } = 0.5;             //Default initial exposure is 0.5 seconds
        public static int GuiderXBinning { get; set; } = 2;                 //Default binning is 2x2
        public static int GuiderYBinning { get; set; } = 2;  
        public static double GuideStarADU { get; set; } = 16000;            //Default target ADU is 16000
        public static double MaximumGuiderExposure { get; set; } = 4.0;     //Default maximum exposure time is 4 seconds
        public static double MinimumGuiderExposure { get; set; } = 0.1;     //Default minimum exposure time is 0.1seconds


        //MaxPixel-based method for getting the ADU in a trackbox-sized subframed image (NOte: could be hot pixel)
        public static double GuideStarMaxPixel(double exposure)
        {
            //Take a subframe image on the guider that is centered on a previously centered star
            //  and get the maximum pixel ADU 
            //The procedure assumes no hot pixels

            AstroImage asti = new AstroImage
            {
                Camera = AstroImage.CameraType.Imaging,
                Frame = AstroImage.ImageType.Light,
                BinX = GuiderXBinning,
                BinY = GuiderYBinning,
                SubFrame = 1,
                Delay = 0,
                Exposure = exposure,
                ImageReduction = AstroImage.ReductionType.AutoDark
            };
            //Set image reduction
            TSXLink.Camera gCam = new TSXLink.Camera(asti) { AutoSaveOn = 0 };

            //Compute the subframe from the trackbox size
            int sizeX = gCam.TrackBoxX;
            int sizeY = gCam.TrackBoxY;
            gCam.SubframeTop = (int)GuideStarY - (sizeY / 2);
            gCam.SubframeBottom = (int)GuideStarY + (sizeY / 2);
            gCam.SubframeLeft = (int)GuideStarX - (sizeX / 2);
            gCam.SubframeRight = (int)GuideStarX + (sizeX / 2);

            int tstat = gCam.GetImage();
            if (tstat != 0)
            {
                return 0;
            }

            double maxPixel = gCam.MaximumPixel;
            return maxPixel;
        }

        //SexTractor-based method for getting the ADU of the star at the center of a trackbox subframe
        public static double GuideStarSextractor(double exposure)
        {
            //Determines the ADU for the X/Y centroid of the maximum FWHM star in a subframe
            //
            //Take a subframe image on the guider using TSX guide star coordinates and trackbox size 

            AstroImage asti = new AstroImage
            {
                Camera = AstroImage.CameraType.Imaging,
                SubFrame = 1,
                Frame = AstroImage.ImageType.Light,
                BinX = GuiderXBinning,
                BinY = GuiderYBinning,
                Delay = 0,
                Exposure = exposure,
                ImageReduction = AstroImage.ReductionType.AutoDark
            };
            TSXLink.Camera gCam = new TSXLink.Camera(asti)
            {
                AutoSaveOn = 1//Turn on autosave so we can extract a star inventory via ShowInventory()
            };

            //Compute the subframe from the trackbox size
            int sizeX = gCam.TrackBoxX;
            int sizeY = gCam.TrackBoxY;
            gCam.SubframeTop = (int)GuideStarY - (sizeY / 2);
            gCam.SubframeBottom = (int)GuideStarY + (sizeY / 2);
            gCam.SubframeLeft = (int)GuideStarX - (sizeX / 2);
            gCam.SubframeRight = (int)GuideStarX + (sizeX / 2);

            //Take an image f
            int tstat = gCam.GetImage();
            if (tstat != 0)
            {
                return 0;
            }

            //Next step is to generate the collection of stars in the subframe
            TSXLink.SexTractor sEx = new TSXLink.SexTractor();
            bool xStat = sEx.SourceExtractGuider();

            List<double> FWHMlist = sEx.GetSourceExtractionList(TSXLink.SexTractor.SourceExtractionType.sexFWHM);
            List<double> CenterX = sEx.GetSourceExtractionList(TSXLink.SexTractor.SourceExtractionType.sexX);
            List<double> CenterY = sEx.GetSourceExtractionList(TSXLink.SexTractor.SourceExtractionType.sexY);

            //Find the star with the greatest FWHM
            int iMax = sEx.GetListLargest(FWHMlist);

            //Determine the ADU value at the center of this listed star.
            //  if it is zero, then look for the maximum pixel ADU

            double maxStarADU = 0;
            try
            {
                maxStarADU = sEx.GetPixelADU((int)CenterX[iMax], (int)CenterY[iMax]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                maxStarADU = 0; ;
            }

            if (maxStarADU == 0)
            { maxStarADU = gCam.MaximumPixel; }
            return maxStarADU;
        }

        //Grease slick to check if autoguiding is already running
        public static bool IsAutoGuideOn()
        {
            AstroImage asti = new AstroImage { Camera = AstroImage.CameraType.Imaging };
            TSXLink.Camera gCam = new TSXLink.Camera(asti);
            return gCam.IsAutoGuideOn();
        }

        //Fires up the autoguider including picking a star and optimizing the exposure time
        public static bool AutoGuideStart()
        {
            //Turns on autoguiding, assuming that everything has been initialized correctly
            //Disconnect the rotator, if any

            TSXLink.Rotator.Disconnect();

            AstroImage asti = new AstroImage
            {
                Camera = AstroImage.CameraType.Imaging,
                BinX = GuiderXBinning,
                BinY = GuiderYBinning,
                Frame = AstroImage.ImageType.Light,
                Exposure = GuideExposure,
                ImageReduction = AstroImage.ReductionType.AutoDark,
                Delay = 0
            };
            //Create new guider object for running the guider
            //then center the AO, if enabled
            TSXLink.Camera gCam = new TSXLink.Camera(asti) { AutoSaveOn = 0 };
            //guide star and exposure should have already been run
            //turn on guiding
            bool agStat = gCam.AutoGuiderOn();
            return agStat;
        }

        //Aborts autoguiding, if running
        public static bool AutoGuideStop()
        {
            //Halt Autoguiding
            //Open default image so we can turn the guider off then open guider and turn it off
            AstroImage asti = new AstroImage() { Camera = AstroImage.CameraType.Imaging };
            TSXLink.Camera gCam = new TSXLink.Camera(asti);
            gCam.AutoGuiderOff();
            return true;
        }

        //*** SetAutoGuideStar picks a guide star and places a subframe around it
        public static bool SetAutoGuideStar()
        {
            // Subroutine takes a picture, picks a guide star, computes a subframe to put around it,
            //  loads the location and subframe into the autoguider
            //

            // Subroutine picks a guide star, computes a subframe to put around it based on current TSX settings
            // and loads the location and subframe into the autoguider
            // 
            int MagWeight = 1;
            int FWHMWeight = 1;
            int ElpWeight = -1;
            int ClsWeight = 1;

            // Algorithm:
            // 
            //   Compute optimality and normalizaton values (see below) 
            //   Eliminate all points near edge and with neighbors
            //   Compute optimality differential and normalize, and add
            //   Select best (least sum) point
            // 
            // Normalized deviation from optimal where optimal is the best value for each of the four catagories:
            //   Magnitude optimal is lowest magnitude
            //   FWHM optimal is average FWHM
            //   Ellipticity optimal is lowest ellipticity
            //   Class optimal is lowest class
            // 
            // Normalized means adjusted against the range of highest to lowest becomes 1 to 0, unless there is only one datapoint
            // 
            // 

            AstroImage asti = new AstroImage
            {
                Camera = AstroImage.CameraType.Imaging,
                SubFrame = 0,
                Frame = AstroImage.ImageType.Light,
                BinX = GuiderXBinning,
                BinY = GuiderYBinning,
                Delay = 0,
                Exposure = GuideExposure,
                ImageReduction = AstroImage.ReductionType.AutoDark
            };
            TSXLink.Camera guider = new TSXLink.Camera(asti) { AutoSaveOn = 1 };

            //Take an image f
            int camResult = guider.GetImage();

            //acquire the current trackbox size (need it later)
            int TrackBoxSize = guider.TrackBoxX;

            TSXLink.SexTractor tsex = new TSXLink.SexTractor();

            try
            {
               bool sStat = tsex.SourceExtractGuider();
            }
            catch (Exception ex)
            {
                // Just close up, TSX will spawn error window
                MessageBox.Show("Star Extracdtion Error: " + ex.Message);
                return false;
            }

            int Xsize = tsex.WidthInPixels;
            int Ysize = tsex.HeightInPixels;

            // Collect astrometric light source data from the image linking into single index arrays: 
            //  magnitude, fmhm, ellipsicity, x and y positionc
            //

            double[] MagArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexMagnitude);
            if (MagArr.Length == 0)
            {
                tsex.Close();
                return false;
            }
            double[] FWHMArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexFWHM);
            double[] XPosArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexX);
            double[] YPosArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexY);
            double[] ElpArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexEllipticity);
            double[] ClsArr = tsex.GetSourceExtractionArray(TSXLink.SexTractor.SourceExtractionType.sexClass);

            // Get some useful statistics
            // Max and min magnitude
            // Max and min FWHM
            // Max and min ellipticity
            // max and min class
            // Average FWHM

            double maxMag = MagArr[0];
            double minMag = MagArr[0];
            double maxFWHM = FWHMArr[0];
            double minFWHM = FWHMArr[0];
            double maxElp = ElpArr[0];
            double minElp = ElpArr[0];
            double maxCls = ClsArr[0];
            double minCls = ClsArr[0];

            double AvgFWHM = 0;

            for (int i = 0; i < MagArr.Length; i++)
            {
                if (MagArr[i] < minMag) { minMag = MagArr[i]; }
                if (MagArr[i] > maxMag) { maxMag = MagArr[i]; }
                AvgFWHM += FWHMArr[i];
                if (FWHMArr[i] < minFWHM) { minFWHM = FWHMArr[i]; }
                if (FWHMArr[i] > maxFWHM) { maxFWHM = FWHMArr[i]; }
                if (ElpArr[i] < minElp) { minElp = ElpArr[i]; }
                if (ElpArr[i] > maxElp) { maxElp = ElpArr[i]; }
                if (ClsArr[i] < minCls) { minCls = ClsArr[i]; }
                if (ClsArr[i] > maxCls) { maxCls = ClsArr[i]; }
            }

            AvgFWHM = AvgFWHM / FWHMArr.Length;

            // Create a set of "best" values
            double optMag = minMag;       // Magnitudes increase with negative values
            double optFWHM = AvgFWHM;     // Looking for the closest to average FWHM
            double optElp = minElp;     // Want the minimum amount of elongation
            double optCls = maxCls;      // 1 = star,0 = galaxy
                                         // Create a set of ranges
            double rangeMag = maxMag - minMag;
            double rangeFWHM = maxFWHM - minFWHM;
            double rangeElp = maxElp - minElp;
            double rangeCls = maxCls - minCls;
            // Create interrum variables for weights
            double normMag;
            double normFWHM;
            double normElp;
            double normCls;
            // Count keepers for statistics
            int SourceCount = 0;
            int EdgeCount = 0;
            int NeighborCount = 0;

            // Create a selection array to store normilized and summed difference values
            int[] SelectArr = new int[MagArr.Length];

            // Convert all points to normalized differences, checking for zero ranges (e.g.single or identical data points)
            for (int i = 0; i < MagArr.Length; i++)
            {
                if (rangeMag != 0) { normMag = 1 - Math.Abs(optMag - MagArr[i]) / rangeMag; }
                else { normMag = 0; }
                if (rangeFWHM != 0) { normFWHM = 1 - Math.Abs(optFWHM - FWHMArr[i]) / rangeFWHM; }
                else { normFWHM = 0; }
                if (rangeElp != 0) { normElp = 1 - Math.Abs(optElp - ElpArr[i]) / rangeElp; }
                else { normElp = 0; }
                if (rangeCls != 0) { normCls = 1 - Math.Abs(optCls - ClsArr[i]) / rangeCls; }
                else { normCls = 0; }

                // Sum the normalized points, weight and store value
                SelectArr[i] = Convert.ToInt32((normMag * MagWeight) +
                                                (normFWHM * FWHMWeight) +
                                                (normElp * ElpWeight) +
                                                (normCls * ClsWeight));
                SourceCount += 1;

                // Remove neighbors and edge liers
                int edgekeepout = TrackBoxSize;

                if (IsOnEdge((int)XPosArr[i], (int)YPosArr[i], Xsize, Ysize, edgekeepout))
                {
                    SelectArr[i] = -1;
                }
                else
                {
                    for (int j = i + 1; j < SelectArr.Length - 1; j++)
                        if (IsNeighbor((int)XPosArr[i], (int)YPosArr[i], (int)XPosArr[j], (int)YPosArr[j], TrackBoxSize))
                        {
                            SelectArr[i] = -2;
                        }
                }
            }

            // Now find the best remaining entry

            int bestOne = 0;
            for (int i = 0; i < SelectArr.Length; i++)
            {
                if (SelectArr[i] > SelectArr[bestOne])
                {
                    bestOne = i;
                }
            }

            guider.GuideStarX = XPosArr[bestOne] * asti.BinX;
            guider.GuideStarY = YPosArr[bestOne] * asti.BinY;
            asti.SubframeLeft = (int)(XPosArr[bestOne] - (TrackBoxSize / 2)) * asti.BinX;
            asti.SubframeRight = (int)(XPosArr[bestOne] + (TrackBoxSize / 2)) * asti.BinX;
            asti.SubframeTop = (int)(YPosArr[bestOne] - (TrackBoxSize / 2)) * asti.BinY;
            asti.SubframeBottom = (int)(YPosArr[bestOne] + (TrackBoxSize / 2)) * asti.BinY;

            if (SelectArr[bestOne] != -1)
            {
                GuideStarX = guider.GuideStarX;
                GuideStarY = guider.GuideStarY;
                tsex.Close();
                return true;
            }
            else
            {
                // run statistics -- only if (total failure
                for (int i = 0; i < SourceCount; i++)
                {
                    if (SelectArr[i] == -1)
                    {
                        EdgeCount += 1;
                    }
                    if (SelectArr[i] == -2)
                    {
                        NeighborCount += 1;
                    }
                }
                tsex.Close();
                return false;
            }
        }

        //*** Optimize Exposure determines the best exposure time for the target star
        public static double OptimizeExposure()
        {
            //Get the best exposure time based on the target ADU
            //
            //Subrountine loops up to 4 times, taking an image, and calulating a new exposure that comes closest to meeting ADU goal
            //
            //Take an image with current exposure set from last guider exposure
            //If the max ADu is 100 or less, then there was no star at all.  Double the exposure upto MaxGuiderExposure
            //If the returned exposure is 64000 or more, then the star was probably saturated.  Halve the exposure down to no less than the minguider exposure.
            //If not within 20% of targetADU, then recalculate and rerun
            //If within 20% then recalculate and done, then update the exposure settings and return the exposure
            //
            double exposure = GuideExposure;
            double tgtADU = GuideStarADU;
            double maxExposure = MaximumGuiderExposure;
            double minExposure = MinimumGuiderExposure;
            //set the maximum number of iterations based on the maximum number of halves or doubles that could be performed.

            //Only take at max 4 shots at getting a good exposure, otherwise return the max or min
            for (int i = 0; i < 4; i++)
            {
                //Start at the initial exposure.  This is a minimum.
                //Take a subframe image
                //Get maximum pixels ADU
                //
                double maxPixel = GuideStarSextractor(exposure);  //Uses SexTractor engine
                ;
                //Check through too low, too high and just right
                if (maxPixel < 500)  //way too low
                {
                    exposure = LimitMaxMin((exposure * 2), maxExposure, minExposure);
                }
                else if (maxPixel > 60000.0)  //too close to saturation
                {
                    exposure = LimitMaxMin((exposure / 2), maxExposure, minExposure);
                }
                else if (!(CloseEnough(maxPixel, tgtADU, 20.0)))  //if not quite close enought recalculate exposure try again
                {
                    if (maxPixel > tgtADU)
                    {
                        exposure = LimitMaxMin(((tgtADU / maxPixel) * exposure), maxExposure, minExposure);
                    }
                    else
                    {
                        exposure = LimitMaxMin(((tgtADU / maxPixel) * exposure), maxExposure, minExposure);
                    }
                }
                else
                {
                    exposure = LimitMaxMin(((tgtADU / maxPixel) * exposure), maxExposure, minExposure);
                    break;
                }
            }
            return exposure;
        }

        //*** Determines if the given x,y position is off the border by at least the xsize and y size
        private static bool IsOnEdge(int Xpos, int Ypos, int Xsize, int Ysize, int border)
        {
            if ((Xpos - border > 0) &&
                (Xpos + border < Xsize) &&
                (Ypos - border > 0) &&
                (Ypos + border) < Ysize)
            { return false; }
            else
            { return true; }
        }

        //*** Determines if two x,y positions are within a given distance of each other
        private static bool IsNeighbor(int Xpos1, int Ypos1, int Xpos2, int Ypos2, int subsize)
        {
            int limit = subsize / 2;
            if ((Math.Abs(Xpos1 - Xpos2) >= limit) || (Math.Abs(Ypos1 - Ypos2) >= limit))
            { return false; }
            else
            { return true; };
        }

        public static double LimitMaxMin(double inVal, double maxLimit, double minLimit)
        {
            //Returns InVal, limited by max and min
            if (inVal > maxLimit)
            { return maxLimit; }
            if (inVal < minLimit)
            { return minLimit; }
            return inVal;
        }

        public static bool CloseEnough(double testval, double targetval, double percentnear)
        {
            //Cute little method for determining if a value is withing a certain percentatge of
            // another value.
            //testval is the value under consideration
            //targetval is the value to be tested against
            //npercentnear is how close (in percent of target val, i.e. x100) the two need to be within to test true
            // otherwise returns false

            if ((Math.Abs(targetval - testval)) <= (Math.Abs((targetval * percentnear / 100))))
            { return true; }
            else
            { return false; }
        }

    }
}
