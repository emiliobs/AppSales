namespace Sales.Backend.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductsView : Product
    {
        public IFormFile ImageFile { get; set; }

       
    }
}
