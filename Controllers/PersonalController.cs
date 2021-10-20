using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoomCrm_Hospital.Models;
using LoomCrm_Hospital.Filters;

namespace LoomCrm_Hospital.Controllers
{
    [GirisKontrol]
    public class PersonalController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult GetPersonalList()
        {
            var personalList = (from per in db.Personal select per).ToList();
            return View(personalList);
        }

        public ActionResult CreatePersonal()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersonal(Personal newPersonal, HttpPostedFileBase photoFile)
        {
            newPersonal.registeredDate = DateTime.Now;
            newPersonal.isActive = true;
            if (ModelState.IsValid)
            {
                db.Personal.Add(newPersonal);
                db.SaveChanges();
                if (photoFile != null)
                {
                    SavePersonalPhoto(newPersonal.personalId, photoFile);
                }
                return RedirectToAction("GetPersonalList", "Personal");
            }

            return View(newPersonal);
        }


        public ActionResult EditPersonal(int personalId)
        {
            var model = db.Personal.Find(personalId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonal(Personal editedPersonal,HttpPostedFileBase photoFile)
        {
            if (ModelState.IsValid)
            {
                var model = db.Personal.Find(editedPersonal.personalId);
                model.EmailAddress = editedPersonal.EmailAddress;
                model.gsmNumber = editedPersonal.gsmNumber;
                model.Name = editedPersonal.Name;
                model.password = editedPersonal.password;
                model.surName = editedPersonal.surName;
                db.SaveChanges();
                if (photoFile != null)
                {
                    SavePersonalPhoto(editedPersonal.personalId, photoFile);
                }
                return RedirectToAction("GetPersonalList","Personal");
            }
            return View(editedPersonal);
        }


        /// <summary>
        /// Personeli aktif/pasif eder.
        /// </summary>
        /// <param name="personalId">İşlem yapılacak personel</param>
        /// <returns></returns>
        public ActionResult PersonalStatus(int personalId)
        {
            var model = (from per in db.Personal
                         where per.personalId == personalId
                         select per).FirstOrDefault();
            if (model.isActive == true)
            {
                model.isActive = false;
            }
            else
            {
                model.isActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("GetPersonalList", "Personal");
        }

        private void SavePersonalPhoto(int personalId, HttpPostedFileBase picture)
        {
            string fileName = "Personal" + personalId + picture.FileName;
            string fullPath = "~/Images/Personal/" + fileName;
            picture.SaveAs(Server.MapPath(fullPath));
            var model = db.Personal.Find(personalId);
            model.photoFile = fullPath;
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