using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace evaporate
{

    class Program
    {
        static int Main(string[] args)
        {
            //TODO : Specify the accessKey and SecurityKey here
            string accessKey = string.Empty;
            string securityKey = string.Empty;
            UploadInstallerUtility utility = new UploadInstallerUtility(ReportProgress, accessKey, securityKey);

            //TODO : Specify the source directory here.
            string sourceDir = string.Empty; 
            utility.Upload(sourceDir, "myBucket"); //Blocking call
            return 0;
        }

        static void ReportProgress(string status)
        {
            Console.WriteLine(status);
        }
    }
}