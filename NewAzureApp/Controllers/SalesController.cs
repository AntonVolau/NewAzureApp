using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewAzureApp.Models.SitesPages;
using PagedList;

namespace NewAzureApp.Controllers
{
    public class SalesController : Controller
    {
        private SalesContext db = new SalesContext();

        // GET: Sales
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SumSortParm = String.IsNullOrEmpty(sortOrder) ? "sum_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var sales = db.Sales.Include(s => s.Customer).Include(s => s.Manager).Include(s => s.Product);
            if (!String.IsNullOrEmpty(searchString))
            {
                sales = sales.Where(s => s.Manager.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "sum_desc":
                    sales = sales.OrderByDescending(s => s.Sum);
                    break;
                default:  // Name ascending 
                    sales = sales.OrderBy(s => s.Sum);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));
        }

        // GET: Sales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name");
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sum,CustomerId,ManagerId,ProductId,Date")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", sale.CustomerId);
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", sale.ManagerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            return View(sale);
        }

        // GET: Sales/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", sale.CustomerId);
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", sale.ManagerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sum,CustomerId,ManagerId,ProductId,Date")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", sale.CustomerId);
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", sale.ManagerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sale = await db.Sales.FindAsync(id);
            db.Sales.Remove(sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
