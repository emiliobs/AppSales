namespace Sales.Backend.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Sales.Backend.Data;
    using Sales.Backend.Helper;
    using Sales.Backend.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;


    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public ProductsAPIController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            this._environment = environment;
        }

        // GET: api/ProductsAPI
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product.OrderBy(p=>p.Description);
        }

        // GET: api/ProductsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }


            //el ok convierte en json el objeto
            return Ok(product);
        }

        // PUT: api/ProductsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            //aqui hago tros el proceso de la imagen si es nueva la grabo o sino dejo la que ya esta en memoria:
            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot/images";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //if (fullPath.Contains('\\'))
                //{
                //    fullPath= fullPath.Split('\\').Last();
                //}

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // POST: api/ProductsAPI
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            //saqui le envio los campo ya con valores, IsAvailable and PubishOn:
            //aqui loa configuro con la hora de londres: hace el callculo de la hora londres con la local:
            product.PublishOn = DateTime.Now.ToUniversalTime();
            product.IsAvailable = true;

          

            var picture = string.Empty;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            { 
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot/images";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //if (fullPath.Contains('\\'))
                //{
                //    fullPath= fullPath.Split('\\').Last();
                //}

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }   

         _context.Product.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }


        //private Product ToProduct(ProductsView view, string picture)
        //{
        //    return new Product()
        //    {
        //        Description = view.Description,
        //        ImagePath = picture,
        //        IsAvailable = view.IsAvailable,
        //        Price = view.Price,
        //        ProductId = view.ProductId,
        //        PublishOn = view.PublishOn,
        //        Remarks = view.Remarks,


        //    };
        //}

        // DELETE: api/ProductsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}