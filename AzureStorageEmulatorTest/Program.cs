using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RimDev.Automation.StorageEmulator;
using System;
using System.IO;

namespace AzureStorageEmulatorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";

            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();

            // Create container. Name must be lower case.
            Console.WriteLine("Creating container...");
            var container = serviceClient.GetContainerReference("mycontainer");
            container.CreateIfNotExistsAsync().Wait();

            string localpath = @"C:\Games";
            string[] fileEntries = Directory.GetFiles(localpath);
            foreach (string filePath in fileEntries)
            {
                string key = Path.GetFileName(filePath);
                CloudBlockBlob blob = container.GetBlockBlobReference(key);
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    blob.UploadFromStreamAsync(fs);
                }
            }
            Console.WriteLine("Done!");
            
            //blob.UploadTextAsync("asd").Wait();
        }
    }
}
