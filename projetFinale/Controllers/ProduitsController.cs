using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projetFinale.Data;
using projetFinale.Models;

namespace projetFinale.Controllers
{
    public class ProduitsController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        
        public ProduitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produits
        public async Task<IActionResult> Index(string searchString)
        {
            var produits = await _context.Goudes.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(n => n.Marque.Contains(searchString)).ToList();
            }
            return View(produits);
        }
        // GET: Produits
        public async Task<IActionResult> BoutiqueA(string searchString)
        {
            var produits = await _context.Goudes.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(n => n.Marque.Contains(searchString)).ToList(); 
            }
            ViewBag.CartCount = await GetCartCount();
            return View(produits);
        }
        /*public async Task<IActionResult> SearchAdmin(string searchString)
        {
            var produits = await _context.Goudes.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(n => n.Marque.Contains(searchString)).ToList();
            }
            return View(produits);
        }*/
        [Authorize]
        public async Task<IActionResult> Panier() 
        { 
            var cartItems = await _context.Chariots
                .Include(c => c.Produit)
                .ToListAsync(); 
            return View(cartItems); 
        }
        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Goudes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produits == null)
            {
                return NotFound();
            }

            return View(produits);
        }
        public async Task<IActionResult> DetailItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Goudes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produits == null)
            {
                return NotFound();
            }

            return View(produits);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marque,Capacite,Utilisation,Fonctionnalite,Prix,Image,CategorieId")] Produits produits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produits);
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Goudes.FindAsync(id);
            if (produits == null)
            {
                return NotFound();
            }
            return View(produits);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marque,Capacite,Utilisation,Fonctionnalite,Prix,Image,CategorieId")] Produits produits)
        {
            if (id != produits.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitsExists(produits.Id))
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
            return View(produits);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Goudes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produits == null)
            {
                return NotFound();
            }

            return View(produits);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produits = await _context.Goudes.FindAsync(id);
            if (produits != null)
            {
                _context.Goudes.Remove(produits);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitsExists(int id)
        {
            return _context.Goudes.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int quantite)
        {
            var produit = await _context.Goudes.FindAsync(id);
            if (produit == null) 
            { 
                return NotFound(); 
            }
            var cartItem = await _context.Chariots
                .FirstOrDefaultAsync(c => c.ProduitId == id);
            if (cartItem!=null)
            {
                cartItem.Quantite += quantite;
                _context.Chariots.Update(cartItem);
            }
            else
            {
                cartItem = new Chariot
                {
                    ProduitId = produit.Id,
                    Marque = produit.Marque,
                    Capacite = produit.Capacite,
                    Prix = produit.Prix,
                    Quantite = quantite,
                    Produit = produit
                };
                _context.Chariots.Add(cartItem);
            };
            await _context.SaveChangesAsync();
            return RedirectToAction("BoutiqueA", "Produits");
        }
        public async Task<IActionResult> ViderPanier()
        { 
            var carItems=_context.Chariots.ToList();
            foreach (var carItem in carItems) 
            {
                _context.Chariots.Remove(carItem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Panier");
        }
        private async Task<int> GetCartCount()
        { 
            var cartCount = await _context.Chariots.SumAsync(c => c.Quantite); 
            return cartCount; 
        }
    }
}
