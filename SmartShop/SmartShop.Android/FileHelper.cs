using SmartShop.Data;
using SmartShop.Droid;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace SmartShop.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}