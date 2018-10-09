namespace Sales.Backend.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Sales.Backend.Data;
    using Sales.Backend.Models;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;


        public ProductsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }


        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.OrderBy(p => p.Description).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {

            //aqui envio la vista create null
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsView view)
        {
           
            if (ModelState.IsValid)
            {
                var picture = string.Empty;

                if (view.ImageFile != null)
                {
                    var path = Path.Combine(this._environment.WebRootPath, "images");
                    Directory.CreateDirectory(path);

                    picture = view.ImageFile.FileName;

                    if (picture.Contains('\\'))
                    {
                        picture = picture.Split('\\').Last();
                    }
                    using (FileStream fs = new FileStream(Path.Combine(path, "", picture), FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(fs);
                    }
                }

                    var product = ToProduct(view, picture);

                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                

               
            }
            return View(view);
        }

            private Product ToProduct(ProductsView view, string picture)
            {
                return new Product()
                {
                    Description = view.Description,
                    ImagePath = picture,
                    IsAvailable = view.IsAvailable,
                    Price = view.Price,
                    ProductId = view.ProductId,
                    PublishOn = view.PublishOn,
                    Remarks = view.Remarks,


                };
            }

            // GET: Products/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound("Sorry, the Products doesn't Exist.!");
                }

                var product = await _context.Product.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Sorry,the Product wasn't Found.!");
                }

                //de producto al productView
                var view = ToView(product);

                return View(view);
            }

            private ProductsView ToView(Product product)
            {
                return new ProductsView()
                {
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    IsAvailable = product.IsAvailable,
                    PublishOn = product.PublishOn,
                    Remarks = product.Remarks,

                };
            }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductsView view)
        {
            //if (id != product.ProductId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {

                var picture = view.ImagePath;

                if (view.ImageFile != null)
                {
                    var path = Path.Combine(this._environment.WebRootPath, "images");
                    Directory.CreateDirectory(path);

                    picture = view.ImageFile.FileName;

                    if (picture.Contains('\\'))
                    {
                        picture = picture.Split('\\').Last();
                    }
                    using (FileStream fs = new FileStream(Path.Combine(path, "", picture), FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(fs);
                    }
                }

                var product = ToProduct(view, picture);

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
