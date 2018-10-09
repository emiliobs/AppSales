namespace Sales.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Products
    {
       
        public int ProductId { get; set; }

        
        public string Description { get; set; }

        public string Remarks { get; set; }
        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublishOn { get; set; }

        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noproduct";
                }

                return $"http://192.168.0.11:555/{ImagePath.Substring(8)}";
                //return $"https://salesapiservices.azurewebsites.net/{ImagePath.Substring(1)}";
                //return $"http://192.168.0.11:555/{ImagePath}";
                //return $"http://192.168.0.11:555/wwwroot/images/wwwroot/images/{ImagePath}";

            }

        }
    }
}
