using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace XamarinBlob
{

    public partial class MainPage : ContentPage
    {
        MediaFile file;
        static string _storageConnection = "DefaultEndpointsProtocol=https;AccountName=xamarinblob1;AccountKey=LmgfHydMOaJJVOX0SxloGk0oFnTPTFLUeJN4Z7r8QQclVjgLaUBPcxTDESZpvc3Nspe24FMlmwNz+AStXgVrQA==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        static CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        static CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("images");

        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void BtnStore_Clicked(object sender, EventArgs e)
        {

            string filePath = file.Path;
            string fileName = Path.GetFileName(filePath);
          await cloudBlobContainer.CreateIfNotExistsAsync();

            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await UploadImage(blockBlob, filePath);
        }

     

        private static async Task UploadImage(CloudBlockBlob blob, string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
        }

        private void BtnGet_Clicked(object sender, EventArgs e)
        {
            if (file == null)
                return;
            imgChoosed2.Source = ImageSource.FromStream(() =>
            {
                var imageStram = file.GetStream();
                return imageStram;
            });
        }

        private static async Task DownloadImage(CloudBlockBlob blob, string filePath)
        {
            if (blob.ExistsAsync().Result)
            {
                await blob.DownloadToFileAsync(filePath, FileMode.CreateNew);
            }
        }

    }
}
