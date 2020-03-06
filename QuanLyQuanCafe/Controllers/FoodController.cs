using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyQuanCafe.Models;
using System.IO;

namespace QuanLyQuanCafe.Controllers
{
    public class FoodController : Controller
    {
        private QuanLyQuanCafeEntities2 db = new QuanLyQuanCafeEntities2();

        // GET: Food
        public ActionResult Index()
        {
            var foods = db.Foods.Include(f => f.FoodCategory);
            return View(foods.ToList());
        }

        // GET: Food/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Food/Create
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.FoodCategories, "id", "name");
            
            return View();
        }

        // POST: Food/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,image,idCategory,price")] FoodImage foodImage)
        {
            if (ModelState.IsValid)
            {
                byte[] image0 = ConvertToBytes(foodImage.image);
                Food newFood = new Food { id = foodImage.id, name = foodImage.name, image = image0, idCategory = foodImage.idCategory, price = foodImage.price };

               
                db.Foods.Add(newFood);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.FoodCategories, "id", "name");
            return View(foodImage);
        }

        // GET: Food/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }

            FoodImage foodImage = new FoodImage { id = food.id, name = food.name, idCategory = food.idCategory, price = food.price, image = null };
          
            ViewBag.idCategory = new SelectList(db.FoodCategories, "id", "name", food.idCategory);
            ViewBag.image = food.image;
            return View(foodImage);
        }

        // POST: Food/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,image,idCategory,price")] FoodImage foodImage)
        {
            if (ModelState.IsValid)
            {
                string sqlQuery = string.Format("select image from food where id = {0}", foodImage.id);
                byte[] img_ = foodImage.image == null ? db.Foods.AsNoTracking().FirstOrDefault(s => s.id == foodImage.id).image : ConvertToBytes(foodImage.image);
                Food food = new Food { id = foodImage.id, name = foodImage.name, image = img_, idCategory = foodImage.idCategory, price = foodImage.price };
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.FoodCategories, "id", "name", foodImage.idCategory);
            return View(foodImage);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = db.Foods.Find(id).image;
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        // GET: Food/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);
            db.Foods.Remove(food);
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
