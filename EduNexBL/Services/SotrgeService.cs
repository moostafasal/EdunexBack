using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon.S3.Model;

using EduNexBL.DTOs;
using Microsoft.Extensions.Configuration;

namespace EduNexBL.Services
{
    public class StorageService : IStorageService
    {
        //private readonly IConfigurationManager _config;

        public StorageService()
        {
            //_config = config;
        }

        public async Task<S3respons> UploadFileAsync(S3object obj, AwsCredentials awsCredentialsValues)
        {
            // Create AWS credentials object
            var credentials = new BasicAWSCredentials(awsCredentialsValues.AwsKey, awsCredentialsValues.AwsSecretKey);

            // Configure the Amazon S3 client
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1 // Update with your bucket's region
            };

            // Initialize response object
            var response = new S3respons();

            try
            {
                // Create Amazon S3 client
                using (var client = new AmazonS3Client(credentials, config))
                {
                    // Create upload request
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = obj.InputStream,
                        Key = obj.Name,
                        BucketName = obj.BucketName,
                        CannedACL = S3CannedACL.NoACL // Set ACL for the uploaded object
                    };

                    // Use TransferUtility to upload the file asynchronously
                    using (var transferUtility = new TransferUtility(client))
                    {
                        await transferUtility.UploadAsync(uploadRequest);
                    }

                    // Manually construct pre-signed URL for the uploaded object
                    var fileUrl = $"https://{obj.BucketName}.s3.amazonaws.com/{Uri.EscapeDataString(obj.Name)}";

                    // Set response properties
                    response.StatusCode = 201;
                    response.Message = $"{obj.Name} has been uploaded successfully";
                    response.FileUrl = fileUrl;
                }
            }
            catch (AmazonS3Exception s3Ex)
            {
                // Handle Amazon S3 exception
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}


