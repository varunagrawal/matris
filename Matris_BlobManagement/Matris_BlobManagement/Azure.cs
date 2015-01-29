using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace Matris_BlobManagement
{
    class Azure
    {
        [JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }

        [JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty(PropertyName = "sasQueryString")]
        public string SasQueryString { get; set; }

        [JsonProperty(PropertyName = "pageUri")]
        public string ImageUri { get; set; }

        public static string Connection = "http://matris.blob.core.windows.net/";
        public static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
        public static CloudBlobClient client = storageAccount.CreateCloudBlobClient();

        public static void CreateContainer(string name)
        {
            CloudBlobContainer container = client.GetContainerReference(name.ToLower());

            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            
        }

        public static void UploadBlob(string container, string blob)
        {
            container = container.ToLower();
            blob = blob.ToLower();

            CreateContainer(container);
            CloudBlobContainer cont = client.GetContainerReference(container);
            CloudBlockBlob blockBlob = cont.GetBlockBlobReference(blob);

            using (var fileStream = GetFile(container, blob))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        public static void ListBlobs(string container)
        {
            CloudBlobContainer cont = client.GetContainerReference(container.ToLower());
            foreach (IListBlobItem item in cont.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    Console.WriteLine("Blob: {0}\n", blob.Uri);
                }
                else if (item.GetType() == typeof(CloudPageBlob)) 
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;
                    Console.WriteLine("Page Blob: {0}\n", pageBlob.Uri);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}\n", directory.Uri);
                }
            }
        }

        public static FileStream GetFile(string container, string name)
        {
            return System.IO.File.OpenRead(Path.Combine(@"C:\Users\vaagraw\Desktop\Matris\", name));
        }
    }
}
