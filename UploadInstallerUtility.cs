using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace evaporate
{

    /// <summary>
    /// Uploads installer artifact to the AWS S3 "atmel-studio" 
    /// </summary>
    public sealed class UploadInstallerUtility
    {

        string accessKey = "";
        string securityKey = "";

        private TransferUtilityUploadDirectoryRequest request;
        UploadProgressDelegate reportUploadProgress;
        const string awsHelpUrl = "http://docs.aws.amazon.com/AmazonSimpleDB/latest/DeveloperGuide/AboutAWSAccounts.html";

        public UploadInstallerUtility(UploadProgressDelegate uploadProgress, string accessKey, string securityKey)
        {


            if (string.IsNullOrEmpty(accessKey))
                throw new AmazonS3Exception("Access Key is empty, please initialize it. Refer " + awsHelpUrl + " for more details.");
            this.accessKey = accessKey;

            if (string.IsNullOrEmpty(accessKey))
                throw new AmazonS3Exception("Security Key is empty, please initialize it. Refer " + awsHelpUrl + " for more details.");
            this.securityKey = securityKey;

            this.reportUploadProgress = uploadProgress;
            Initialize();

        }

        void Initialize()
        {
            request = new TransferUtilityUploadDirectoryRequest();
            request.CannedACL = S3CannedACL.PublicRead;
            request.Timeout = new TimeSpan(1, 0, 0); //Set timeout as 1 hour
            request.UploadDirectoryProgressEvent += request_UploadDirectoryProgressEvent;
        }


        /// <summary>
        /// UPloads the files in a given source directory to the specified bucketName. Note that subdirectories are not included. Only files within the folder are included.
        /// </summary>
        /// <param name="directory">Source Directory in the local file system to upload</param>
        /// <param name="bucketName">Name of the bucket in Amazon S3</param>
        /// <returns></returns>
        public int Upload(string directory, string bucketName)
        {
            request.Directory = directory;
            request.BucketName = bucketName;


            TransferUtility transferManager = new TransferUtility(accessKey,securityKey, RegionEndpoint.USWest2);
            transferManager.UploadDirectory(request);
            return 0;
        }


        void request_UploadDirectoryProgressEvent(object sender, UploadDirectoryProgressArgs e)
        {
            if (this.reportUploadProgress != null)
            {
                reportUploadProgress(string.Format("Processing {0}, Total Size : {1} K, Uploaded {2} K", e.CurrentFile, e.TotalNumberOfBytesForCurrentFile / 1024, e.TransferredBytesForCurrentFile / 1024));
            }
        }
    }
}