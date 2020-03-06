using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.Controllers
{
    public class BillController : Controller
    {
        private QuanLyQuanCafeEntities2 db = new QuanLyQuanCafeEntities2();

        // GET: Bill
        public ActionResult Index(int? id)
        {
            var bills = db.Bills.Include(b => b.TableFood);
            List<Bill> spamBills = new List<Bill>();
            foreach(var bill in bills)
            {
                if(bill.BillInfoes.Count == 0)
                {
                    spamBills.Add(bill);
                }
            }
            db.Bills.RemoveRange(spamBills);
            db.SaveChanges();
            BillData billdatas = new BillData();
            billdatas.bills = db.Bills.Include(p => p.TableFood);
            ViewBag.Detail = false;

            if (id != null)
            {

                billdatas.billInfors = db.Bills.Find(id).BillInfoes;
                ViewBag.Detail = true;
                var Totel = 0.0;
                foreach(var billinfo in billdatas.billInfors)
                {
                    Totel += billinfo.Food.price * billinfo.count;
                    
                }
                ViewBag.Totel = Totel;
            }
           

            return View(billdatas);
        }

        // GET: Bill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bill/Create
        public ActionResult Create()
        {
            ViewBag.idTable = new SelectList(db.TableFoods, "id", "name");
            return View();
        }

        // POST: Bill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,DateCheckIn,DateCheckOut,idTable,status")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idTable = new SelectList(db.TableFoods, "id", "name", bill.idTable);
            return View(bill);
        }

        // GET: Bill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTable = new SelectList(db.TableFoods, "id", "name", bill.idTable);
            return View(bill);
        }

        // POST: Bill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DateCheckIn,DateCheckOut,idTable,status")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idTable = new SelectList(db.TableFoods, "id", "name", bill.idTable);
            return View(bill);
        }

        // GET: Bill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
            db.SaveChanges();
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
