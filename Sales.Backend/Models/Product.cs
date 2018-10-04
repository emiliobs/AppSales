
namespace Sales.Backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]          
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false )]
        public decimal Price { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Publish On")]
        public DateTime PublishOn { get; set; }


        //public string ImageFullPath
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(ImagePath))
        //        {
        //            return $"noproduct"; 
        //        }
                
        //       // return ImagePath;
        //        //return $"https://salesapiservices.azurewebsites.net/{ImagePath.Substring(1)}";
        //        return $"http://192.168.0.11:555/images/{ImagePath}";
        //    }

        //}
    }
}
