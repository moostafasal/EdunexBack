using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using EduNexBL.DTOs;

namespace EduNexBL.Services
{
    public interface IStorageService
    {
        Task<S3respons> UploadFileAsync(S3object obj, AwsCredentials awsCredentialsValues);
    }
}
