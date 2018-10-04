namespace Sales.Backend.Helper
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    public  class FileHelper
    {
        //private  IHostingEnvironment _environment;
        //public  FileHelper(IHostingEnvironment environment)
        //{
        //    _environment = environment;
        //}
        //public  string UploadPhoto(IFormFile file, string folder)
        //{
            
        //    string path = string.Empty;
        //    string pic = string.Empty;

        //    if (file != null)
        //    {
        //        pic = Path.GetFileName(file.FileName);
        //        path = Path.Combine(_environment.ContentRootPath, folder, pic);

        //        pic = file.FileName;

        //        //if (picture.Contains('\\'))
        //        //{
        //        //    picture = picture.Split('\\').Last();
        //        //}
        //        using (FileStream fs = new FileStream(Path.Combine(path), FileMode.Create))
        //        {
        //            file.CopyToAsync(fs);
        //        }
        //    }

        //    return pic;

        
    }
}
