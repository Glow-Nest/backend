﻿// using Application.Interfaces;
// using CloudinaryDotNet;
// using CloudinaryDotNet.Actions;
// using Microsoft.Extensions.Configuration;
//
// namespace Services.Cloudinary;
//
// public class CloudinaryService : ICloudinaryService
// {
//     private readonly CloudinaryDotNet.Cloudinary _cloudinary;
//     
//     public CloudinaryService(IConfiguration configuration)
//     {
//         var cloudName = configuration["Cloudinary:CloudName"];
//         var apiKey = configuration["Cloudinary:ApiKey"];
//         var apiSecret = configuration["Cloudinary:ApiSecret"];
//
//         var account = new Account(cloudName, apiKey, apiSecret);
//         _cloudinary = new CloudinaryDotNet.Cloudinary(account);
//     }
//     
//     public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
//     {
//         var uploadParams = new ImageUploadParams()
//         {
//             File = new FileDescription(fileName, fileStream),
//             Folder = "beauty-salon"
//         };
//
//         var uploadResult = await _cloudinary.UploadAsync(uploadParams);
//         return uploadResult.SecureUrl.ToString(); 
//     }
// }