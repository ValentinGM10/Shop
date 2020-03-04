using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        // private readonly IRepository repository;
        private readonly IUserHelper userHelper;

        // public ProductsController(IRepository repository, IUserHelper userHelper)
        // public ProductsController(IRepository repository)
        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            // this.repository = repository;
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        // GET: Products
        //public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            // return View(this.repository.GetProducts());
            // return View(await _context.Products.ToListAsync());
            
            //return View(this.productRepository.GetAll());
            return View(this.productRepository.GetAll().OrderBy(p => p.Name));
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }






        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        //         public async Task<IActionResult> Create( Product product)
        public async Task<IActionResult> Create(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {

                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    //path = Path.Combine(
                    //    Directory.GetCurrentDirectory(), 
                    //    "wwwroot\\images\\Products", 
                    //    view.ImageFile.FileName);


                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);


                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    //path = $"~/images/Products/{view.ImageFile.FileName}";
                    path = $"~/images/Products/{file}";
                }

                var product = this.ToProduct(view, path);


                //_context.Add(product);
                //await _context.SaveChangesAsync();
                //TODO:  Cambiar el Usuario quemado Agregar
                product.User = await this.userHelper.GetUserByEmailAsync("vgalvez10@gmail.com");

                //this.repository.AddProduct(product);
                // await this.repository.SaveAllAsync();

                
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            //return View(product);
            return View(view);  
        }


        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User
            };
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var view = this.ToProducViewModel(product);
            return View(view);
        }



        private ProductViewModel ToProducViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);


                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Products", view.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        // path = $"~/images/Products/{view.ImageFile.FileName}";
                        path = $"~/images/Products/{file}";
                    }

                    // TODO: Pending to change to: this.User.Identity.Name
                    view.User = await this.userHelper.GetUserByEmailAsync("vgalvez10@gmail.com");
                    var product = this.ToProduct(view, path);
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(view.Id))
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

        //// POST: Products/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(ProductViewModel view)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var path = view.ImageFile;

        //            if (view.ImageFile != null && view.ImageFile.Length > 0)
        //            {
        //                path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images\\Products", view.ImageFile.FileName);

        //                using (var stream = new FileStream(path, FileMode.Create))
        //                {
        //                    await view.ImageFile.CopyToAsync(stream);
        //                }

        //                path = $"~/images/Products/{view.ImageFile.FileName}";
        //            }




        //            // TODO: Pending to change to: this.User.Identity.Name
        //            product.User = await this.userHelper.GetUserByEmailAsync("jzuluaga55@gmail.com");
        //            await this.productRepository.UpdateAsync(product);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await this.productRepository.ExistAsync(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(product);
        //}





        // GET: Products/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // var product = this.repository.GetProduct(id.Value);
            //  var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = await this.productRepository.GetByIdAsync(id.Value);

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
            //var product = await _context.Products.FindAsync(id);
            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();

            //var product = this.repository.GetProduct(id);
            //this.repository.RemoveProduct(product);
            //await this.repository.SaveAllAsync();

            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
