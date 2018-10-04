namespace Sales.Backend.Models
{
    using Microsoft.AspNetCore.Http;
    public class ProductsView : Product
    {
        public IFormFile ImageFile { get; set; }
    }
}
