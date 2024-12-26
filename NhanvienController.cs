using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using lasttest.Models;

namespace lasttest.Controllers
{
    public class NhanvienController : Controller
    {
        private Model1 db = new Model1();

        // GET: Nhanvien
        public ActionResult Index()
        {
            var nhanviens = db.Nhanviens.Include(n => n.Phongban);
            return View(nhanviens.ToList());
        }

        // GET: Nhanvien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nhanvien nhanvien = db.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(nhanvien);
        }

        // GET: Nhanvien/Create
        public ActionResult Create()
        {
            ViewBag.maphong = new SelectList(db.Phongbans, "maphong", "tenphong");
            return View();
        }

        // POST: Nhanvien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Nhanvien nv)
        {

            try
            {
                db.Nhanviens.Add(nv);
                db.SaveChanges();
                return Json(new { a = true, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { a = false, error = ex.Message });
            }
        }
        [ChildActionOnly]
        public PartialViewResult CategoryMenu()
        {
            var li = db.Phongbans.ToList();
            return PartialView(li);
        }
        [Route("NhanVienTheoPhong/{maphong}")]
        public ActionResult HienThiTheoPhong(int maphong)
        {
            var li = db.Nhanviens.Where(e => e.maphong == maphong).ToList();
            return View(li);
        }
        [HttpGet]
        public ActionResult Login()
        {
            var nhanviens = db.Nhanviens.Include(n => n.Phongban);
            return View(nhanviens.ToList());
        }
        [HttpPost]
        public ActionResult Login(int user, string pass)
        {
            var a = db.Nhanviens.Where(p => p.manv == user && p.matkhau == pass).FirstOrDefault();
            if (a != null)
            {
                Session["user"] = a.hotennv;
                return RedirectToAction("Index", "Nhanvien");
            }
            else
            {
                ViewBag.err = "Sai tên đăng nhập hoặc mật khẩu";
                return View("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Index", "Nhanvien");
        }
        // GET: Nhanvien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nhanvien nhanvien = db.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            ViewBag.maphong = new SelectList(db.Phongbans, "maphong", "tenphong", nhanvien.maphong);
            return View(nhanvien);
        }

        // POST: Nhanvien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manv,hotennv,tuoi,diachi,luongnv,maphong,matkhau")] Nhanvien nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maphong = new SelectList(db.Phongbans, "maphong", "tenphong", nhanvien.maphong);
            return View(nhanvien);
        }

        // GET: Nhanvien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nhanvien nhanvien = db.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(nhanvien);
        }

        // POST: Nhanvien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nhanvien nhanvien = db.Nhanviens.Find(id);
            db.Nhanviens.Remove(nhanvien);
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
