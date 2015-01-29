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
    class Program
    {
        static void Main(string[] args)
        {
            string container = "Health"; //Console.ReadLine();

            FileStream pagesList = File.OpenRead(@"C:\Users\vaagraw\Desktop\Matris\HealthPagesLinks.txt");
            using (StreamReader sr = new StreamReader(pagesList))
            { 
                while(!sr.EndOfStream)
                {
                    string[] entry = sr.ReadLine().Split(new char[] {';'});

                    string blob = entry[0] + "/" + entry[1];
                    Console.WriteLine(blob);
                    if (!string.IsNullOrEmpty(blob))
                    {
                        //Azure cloud = new Azure();
                        Azure.UploadBlob(container, blob);
                    }
                }
                
            }
            //Console.Write("Input name of Blob: ");
            //string blob = Console.ReadLine();
            //string blob = @"Test\DietHealthTemplate.html"; //Console.ReadLine();

            //Azure.ListBlobs(container);            

            Console.Write("Done!");
            Console.ReadKey();
        }
    }
}
