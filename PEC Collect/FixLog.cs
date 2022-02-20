using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PEC_Collect
{
    public static class FixLog
    {
        //Class contains routines to fix last log entries, if possible
        //
        public static void FixImagePA(string sbPath, double currentpa)
        {
            //changes the "unknown" in the image pa to a value
            //
            //Look up current directory
            string cDir = FindCurrentDir(sbPath);
            string logFilename = FindLastLog(cDir);
            //construct file path names
            string logFilePath = logFilename;
            string tempFilePath = logFilename + ".tmp";
            //open up current log file
            TextReader tr = new StreamReader(logFilePath);
            //open up temporary file in current directory
            TextWriter tw = new StreamWriter(tempFilePath);
            //set up postion angle text string
            string writePAout = "Position angle now = " + currentpa.ToString("0.00");
            //read in each of log file, replace the position angle line
            while (tr.Peek() != -1)
            {
                string readIn = tr.ReadLine();
                if (readIn.Contains("Position angle now"))
                { tw.WriteLine(writePAout); }
                else
                { tw.WriteLine(readIn); }
            }
            //delete the current log and rename the temporary log
            tw.Close();
            tr.Close();
            File.Replace(tempFilePath, logFilePath, logFilePath + ".old");
            File.Delete(tempFilePath);
            return;
        }

        private static string FindLastLog(string currentDir)
        {
            //Returns short file name of last log file
            //Looking in user... /SoftwareBisque../CameraAutosave/Imager/"current date"/"latest log"

            string[] logFiles = Directory.GetFiles(currentDir, "*.log");
            //find highest numbered log
            int highest = 0;
            string hFile = null;
            foreach (string f in logFiles)
            {
                int num = Convert.ToInt32((f.Split('.'))[1]);
                if (num > highest)
                {
                    highest = num;
                    hFile = f;
                }
            }
            return hFile;
        }

        private static string FindCurrentDir(string sbPath)
        {
            //Returns the current log directory for imager
            //Looking in directory path:  user... /SoftwareBisque../CameraAutosave/Imager/<current date>/<most recent>.log"

            string LogDirectoryFullPath = sbPath
                + DateTime.Now.ToString("MMMM dd yyyy");
            if (Directory.Exists(LogDirectoryFullPath)) 
                return LogDirectoryFullPath;
            else
            {
                LogDirectoryFullPath = sbPath
                + (DateTime.Now - TimeSpan.FromDays(1)).ToString("MMMM dd yyyy");
            }
            return LogDirectoryFullPath;
        }
    }
}
