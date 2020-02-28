using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;

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
            return View(this.productRepository.GetAll());
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
        public async Task<IActionResult> Create( Product product)
        {
            if (ModelState.IsValid)
            {

                //_context.Add(product);
                //await _context.SaveChangesAsync();
                //TODO:  Cambiar el Usuario quemado Agregar
                product.User = await this.userHelper.GetUserByEmailAsync("vgalvez10@gmail.com");

                //this.repository.AddProduct(product);
                // await this.repository.SaveAllAsync();

                
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Pending to change to: this.User.Identity.Name
                    product.User = await this.userHelper.GetUserByEmailAsync("jzuluaga55@gmail.com");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(product.Id))
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

            return View(product);
        }





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
