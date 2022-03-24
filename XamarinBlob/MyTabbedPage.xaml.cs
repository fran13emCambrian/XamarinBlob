using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace XamarinBlob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTabbedPage : TabbedPage
    {
        public MyTabbedPage()
        {
            InitializeComponent();
        }
    }
}
