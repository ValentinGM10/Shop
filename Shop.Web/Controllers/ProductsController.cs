﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data;
using Shop.Web.Data.Entities;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository repository;

        public ProductsController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: Products
        //public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            // return View(await _context.Products.ToListAsync());
            return View(this.repository.GetProducts());
        }

        // GET: Products/Details/5
        // public async Task<IActionResult> Details(int? id)
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = this.repository.GetProduct(id.Value);

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
                this.repository.AddProduct(product);
                await this.repository.SaveAllAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id) 
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products.FindAsync(id);
            var product = this.repository.GetProduct(id.Value);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(product);
                    //await _context.SaveChangesAsync();
                    this.repository.UpdateProduct(product);
                    await this.repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!ProductExists(product.Id))
                    if (!this.repository.ProductExists(product.Id))
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //  var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = this.repository.GetProduct(id.Value);
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

            var product = this.repository.GetProduct(id);
            this.repository.RemoveProduct(product);
            await this.repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
