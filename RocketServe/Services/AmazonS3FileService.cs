using Amazon.S3;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Services
{
    public class AmazonS3FileService
    {

        string AWS_accessKey = "AKIA5EPSQMQCROD7MMCK";
        string AWS_secretKey = "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";
        string AWS_bucketName = "rocket-serve-menus";
        string AWS_defaultFolder = "Test_Folder";

        /// <summary>
        /// Uploads File to S3 service
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="subFolder">subfolder for the given file</param>
        /// <returns>URL of File or Error string</returns>
        public async Task<string> UploadFileToAWSAsync(IBrowserFile file, string name = "", string subFolder = "")
        {
            var result = "";
            try
            {
                var s3Client = new AmazonS3Client(AWS_accessKey, AWS_secretKey, Amazon.RegionEndpoint.EUWest3);
                var bucketName = AWS_bucketName;
                var keyName = AWS_defaultFolder;
                if (!string.IsNullOrEmpty(subFolder))
                    keyName = keyName + "/" + subFolder.Trim();

                string fileName;
                if (!String.IsNullOrEmpty(name))
                    fileName = name;
                else
                    fileName = file.Name;

                keyName = keyName + "/" + fileName;

                var fs = file.OpenReadStream();
                var request = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = fs,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead,

                };
                await s3Client.PutObjectAsync(request);

                result = string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, keyName);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
