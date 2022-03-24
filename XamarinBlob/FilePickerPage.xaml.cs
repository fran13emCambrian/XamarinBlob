using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using Plugin.Media.Abstractions;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace XamarinBlob
{
    public partial class FilePickerPage : ContentPage
    {
        MediaFile file;
        static string _storageConnection = "DefaultEndpointsProtocol=https;AccountName=xamarinblob1;AccountKey=LmgfHydMOaJJVOX0SxloGk0oFnTPTFLUeJN4Z7r8QQclVjgLaUBPcxTDESZpvc3Nspe24FMlmwNz+AStXgVrQA==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        static CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        static CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("textfiles");
        public FilePickerPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var pickResult = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf,

                PickerTitle = "Pick a File",

            }); 
            if (pickResult != null)
            {
                var stream = await pickResult.OpenReadAsync();
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

        private Task UploadImage(CloudBlockBlob blockBlob, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
