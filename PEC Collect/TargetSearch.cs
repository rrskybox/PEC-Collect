using System.Linq;
using TheSkyXLib;
using System.Xml.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PEC_Collect
{
    public partial class StarList
    {

        public static string StarSearchDBQPath { get; set; }
        public static string TargetName { get; set; }
        public static double TargetRA { get; set; }
        public static double TargetDec { get; set; }

        public static string TargetStarSearch()
        {
            //Upon instantiation...
            //Open empty working XML file for Star list
            //Create connection to TSX DataWizard
            //Get the path to the query files, then set the path to SuperScanQuery.sdb
            //Run the DataWizard
            //Create an XML datastructure for the Observing List and load it with Observing LIst entries
            //Replace the current working Star List file with the new XML data list
            //Close the file
            string gName;
            double gRA;
            double gDec;

            //Locate current position on star chart and set FOV to 3 degrees
            sky6StarChart tsxsc = new sky6StarChart();
            double centerRA = tsxsc.RightAscension;
            double centerDec = tsxsc.Declination;
            tsxsc.FieldOfView = 3.0;


            //Look for closest member of list
            //Note the Hour Angle should always be greater than zero
            // and the search area witin 3 degrees of the center of the chart
            // Also only uses HIP stars because "Find" doesn't work on all names, like Tycho

            double bestSeparation = 100.0;
            double tSeparation;
            TargetRA = centerRA;
            TargetDec = centerDec;
            sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
            do
            {
                //tsx_oi = GetStars(); // will return null if null tsx_oi (including exception thrown) or count = 0;
                //Runs the database query and checks results. objecty information false
                //   else returns null
                sky6DataWizard tsx_dw = new sky6DataWizard();
                ///Set query path 
                tsx_dw.Path = StarSearchDBQPath;
                tsx_dw.Open();
                string tst = tsx_dw.Path;
                try { tsx_oi = tsx_dw.RunQuery; }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                if (tsx_oi.Count == 0) return null;
                //
                //
                if (tsx_oi == null)
                {
                    MessageBox.Show("Waiting half a minute for a better target star");
                    System.Threading.Thread.Sleep(30000); //sleep for thirty seconds then try again
                }
            } while (tsx_oi == null);
            int oiCount = tsx_oi.Count;
            for (int i = 0; i < oiCount; i++)
            {
                tsx_oi.Index = i;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
                gName = tsx_oi.ObjInfoPropOut;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
                gRA = tsx_oi.ObjInfoPropOut;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
                gDec = tsx_oi.ObjInfoPropOut;
                if (gName.Contains("HIP"))
                {
                    tSeparation = ComputeDistance(centerRA, centerDec, gRA, gDec);
                    if (tSeparation < bestSeparation)
                    {
                        bestSeparation = tSeparation;
                        TargetName = gName;
                        TargetRA = gRA;
                        TargetDec = gDec;
                    }
                }
            }
            tsxsc.Find(TargetName);
            return TargetName;
        }

        private static double ComputeDistance(double ra1, double dec1, double ra2, double dec2)
        {
            //Computes the angular distance between two polar coordinates using TSX utility function
            //
            sky6Utils tsx_ut = new sky6Utils();
            tsx_ut.ComputeAngularSeparation(ra1, dec1, ra2, dec2);
            double dist = tsx_ut.dOut0;
            return dist;
        }

        /// <summary>
        /// Creates the target guide star database query, if needed
        /// </summary>
        public static void InstallDBQ()
        {
            //Installs the dbq file in the proper destination folder if it is not installed already.
            //
            //  Generate the install path from the defaults.       
            StarSearchDBQPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Software Bisque\\TheSkyX Professional Edition\\Database Queries\\PECCollect.dbq";
            if (!File.Exists(StarSearchDBQPath))
            {
                Assembly dassembly = Assembly.GetExecutingAssembly();
                //Collect the file contents to be written
                Stream dstream = dassembly.GetManifestResourceStream("PEC_Collect.PECCollect.dbq");

                int dlen = Convert.ToInt32(dstream.Length);
                int doff = 0;
                byte[] dbytes = new byte[dstream.Length];
                int dreadout = dstream.Read(dbytes, doff, dlen);
                FileStream dbqfile = File.Create(StarSearchDBQPath);
                dbqfile.Close();
                //write to destination file
                File.WriteAllBytes(StarSearchDBQPath, dbytes);
                dstream.Close();
            }
            return;
        }

        public static sky6ObjectInformation GetStars()
        {
            //Runs the database query and checks results. objecty information false
            //   else returns null
            sky6DataWizard tsx_dw = new sky6DataWizard();
            sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
            ///Set query path 
            tsx_dw.Path = StarSearchDBQPath;
            tsx_dw.Open();
            string tst = tsx_dw.Path;
            try { tsx_oi = tsx_dw.RunQuery; }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            if (tsx_oi.Count == 0) return null;
            else return tsx_oi;
        }
    }
}
