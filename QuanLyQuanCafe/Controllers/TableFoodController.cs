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
    public class TableFoodController : Controller
    {
        private QuanLyQuanCafeEntities2 db = new QuanLyQuanCafeEntities2();

        // GET: TableFood
        public ActionResult Index(int? id)
        {
            var tabaleData = new TableData();
            tabaleData.tableFoods = db.TableFoods.Include(p => p.Bills)
                                                 .OrderBy(p => p.id);
            if (id != null)
            {
                var bill = db.TableFoods.Find(id).Bills.LastOrDefault();

                if (bill != null)
                {
                    if (bill.status != 1)
                    {
                        tabaleData.billInfors = bill.BillInfoes;
                        if (tabaleData.billInfors.Count() != 0)
                        {
                            ViewBag.id = tabaleData.billInfors.FirstOrDefault().idBill;
                            var Totel = 0.0 ;
                            foreach(var billInfor in tabaleData.billInfors)
                            {
                                Totel += billInfor.Food.price*billInfor.count;
                            }
                            ViewBag.Totel = Totel;
                        }
                        else
                        {
                            ViewBag.id = 0;
                        }
                    }
                }
            }
                return View(tabaleData);
            
        }

        public ActionResult Pay(int id)
        {
            var bill = db.Bills.Find(id);

            string sql = "update bill set status = {0}, DateCheckOut = {1}   where id={2}";
            db.Database.ExecuteSqlCommand(sql, 1,DateTime.UtcNow, id);

            string sql1 = "update tablefood set status = {0} where id={1}";
            db.Database.ExecuteSqlCommand(sql1, "Trống", bill.idTable);
            


            //Bill bill = new Bill { idTable = id, DateCheckIn = DateTime.UtcNow, DateCheckOut = null, status = 0 };
            //db.Bills.Add(bill);
            //db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult CreatBill(int id)
        {
            //var foods = db.Foods.Include(f => f.FoodCategory);
            if (db.TableFoods.Find(id).status == "Trống")
            {
                Bill bill = new Bill { idTable = id, DateCheckIn = DateTime.UtcNow, DateCheckOut = null, status = 0 };
                db.Bills.Add(bill);
                db.SaveChanges();
            }
                       
            return RedirectToAction("SelectFood");
        }

        public ActionResult SelectFood(int? id)
        {
            //var foods = db.Foods.Include(f => f.FoodCategory);
            var food = db.Foods.Find(id);
            var idBill_ = db.Bills.OrderBy(p => p.id).ToList().Last().id;
            if (id == null)

            {
                return View(GetFoodData(idBill_));
            }
          
            var billInfo_ = db.BillInfoes.AsNoTracking().Where(b => b.idFood == id && b.idBill == idBill_).SingleOrDefault();
            int idTable = db.Bills.Find(idBill_).idTable;

            if (billInfo_ != null)
            {
                BillInfo billInfo = new BillInfo {id = billInfo_.id, idBill = idBill_, idFood = food.id, count = billInfo_.count+1 };
                db.Entry(billInfo).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            else
            {
                BillInfo billInfo = new BillInfo { idBill = db.Bills.ToList().Last().id, idFood = food.id, count = 1  };
                db.BillInfoes.Add(billInfo);
                string Sql = "update TableFood set status = {0} where id = {1}";
                db.Database.ExecuteSqlCommand(Sql, "Có Người", idTable);            
                db.SaveChanges();

                
            }
            //ViewBag.Count = db.BillInfoes.Where(p => p.idBill = idBill_) ;


            ViewBag.idTable = idTable;
            return View(GetFoodData( idBill_));
        }

        public List<FoodData> GetFoodData(int idBill_)
        {
            
            var fooddata = new List<FoodData>();
            var foods = db.Foods;
            foreach (var food_ in foods)
            {
                var count = 0;
                var billInfor = db.BillInfoes.Where(b => b.idBill == idBill_ && b.idFood == food_.id).SingleOrDefault();
                if(billInfor != null)
                {
                    count = billInfor.count;
                }
                fooddata.Add(new FoodData
                {
                    id = food_.id,
                    name = food_.name,
                    Category = food_.FoodCategory.name,
                    price = food_.price,
                    image = food_.image,
                    count = count 
                   
                });
            }
            return fooddata;
        }

        // GET: TableFood/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableFood tableFood = db.TableFoods.Find(id);
            if (tableFood == null)
            {
                return HttpNotFound();
            }
            return View(tableFood);
        }

        // GET: TableFood/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Trống", Value = "Trống" });
            items.Add(new SelectListItem { Text = "Có Người", Value = "Có Người" });
            ViewBag.items = items;
                //new SelectList(items, "Text", "Value", db.);
            return View();
        }

        // POST: TableFood/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,status")] TableFood tableFood)
        {
            if (ModelState.IsValid)
            {
                db.TableFoods.Add(tableFood);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableFood);
        }

        // GET: TableFood/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableFood tableFood = db.TableFoods.Find(id);
            if (tableFood == null)
            {
                return HttpNotFound();
            }
            return View(tableFood);
        }

        // POST: TableFood/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,status")] TableFood tableFood)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableFood).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableFood);
        }

        // GET: TableFood/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableFood tableFood = db.TableFoods.Find(id);
            if (tableFood == null)
            {
                return HttpNotFound();
            }
            return View(tableFood);
        }

        // POST: TableFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableFood tableFood = db.TableFoods
                .Include(p => p.Bills)
                .Where(p => p.id == id).SingleOrDefault();
            var bills = tableFood.Bills;
           List<BillInfo> billInfoes = new List<BillInfo>();
            foreach(var bill in bills)
            {
                billInfoes.AddRange(bill.BillInfoes);
                
            }
            db.BillInfoes.RemoveRange(billInfoes);
            db.Bills.RemoveRange(bills);
            db.TableFoods.Remove(tableFood);
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
