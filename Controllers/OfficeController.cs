using LoomCrm_Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LoomCrm_Hospital.Filters;

namespace LoomCrm_Hospital.Controllers
{
    [GirisKontrol]
    public class OfficeController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        /// <summary>
        /// Hastanenin tüm şubelerini verir
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOfficeList()
        {
            var offices = (from off in db.Office where off.name!="System" select off).OrderBy(x => x.name).ToList();
            return View(offices);
        }

        public ActionResult GetOfficeBox()
        {
            var model = (from off in db.Office
                         where off.name!="System"
                         select off).ToList();
            return View(model);
        }

        /// <summary>
        /// Yeni Şube Ekleme Metodu
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateOffice()
        {
            return View();
        }

        /// <summary>
        /// Şubeyi ekleyen metot
        /// </summary>
        /// <param name="off">Sadece isim yazman yeterli</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOffice(Office off)
        {
            if (ModelState.IsValid)
            {
                off.creationDate = DateTime.Now;
                off.isActive = true;
                db.Office.Add(off);
                db.SaveChanges();
                if (off.officePicture != null)
                {
                    PrepareOfficePicture(off.officePicture, off.officeId);
                }
                return RedirectToAction("GetOfficeList", "Office");
            }
            return View(off);
        }

        /// <summary>
        /// Şubenin durumunu değiştirir.
        /// </summary>
        /// <param name="officeId">Ofisin Id'sini belirtiniz.</param>
        /// <returns></returns>
        public ActionResult OfficeStatus(int officeId)
        {
            var model = db.Office.Find(officeId);
            if (model.isActive==true)
            {
                model.isActive = false;
            }
            else
            {
                model.isActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("GetOfficeList");
        }

        public ActionResult EditOffice(int officeId,bool? edited)
        {
            if (edited == false)
            {
                ViewBag.Message = "Lütfen ilgili alanları kontrol ediniz.";
            }
            var model = db.Office.Find(officeId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditOffice(Office off)
        {
            var model = db.Office.Find(off.officeId);
            if (ModelState.IsValid)
            {
                model.mapsUrl = off.mapsUrl;
                model.name = off.name;
                db.SaveChanges();
                if (off.officePicture != null)
                {
                    PrepareOfficePicture(off.officePicture, off.officeId);
                }
                return RedirectToAction("GetOfficeList","Office");
            }

            return RedirectToAction("EditOffice","Office",new {edited=false });
        }

        private void PrepareOfficePicture(HttpPostedFileBase picture, int officeId)
        {
            string fileName = "Office" + officeId + picture.FileName;
            string fullPath = "~/Images/Office/" + fileName;
            picture.SaveAs(Server.MapPath(fullPath));
            var model = db.Office.Find(officeId);
            model.officePicturePath = fullPath;
            db.SaveChanges();
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