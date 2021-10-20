using LoomCrm_Hospital.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoomCrm_Hospital.Models;
using System.Web.Mvc;

namespace LoomCrm_Hospital.Controllers
{
    [GirisKontrol]
    public class PatientController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult GetPatientList()
        {
            var model = db.Patient.ToList();
            return View(model);
        }

        public ActionResult CreatePatient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatient(Patient createdPatient, HttpPostedFileBase photoFile)
        {
            createdPatient.registeredDate = DateTime.Now;
            createdPatient.isActive = true;
            bool notAddedPatient = CheckAlreadyAddedPatient(createdPatient.IdentityNo);
            if (notAddedPatient == true)
            {
                if (ModelState.IsValid)
                {
                    db.Patient.Add(createdPatient);
                    db.SaveChanges();
                    SavePatientPictureFile(photoFile, createdPatient.patientId);
                    return RedirectToAction("GetPatientList","Patient");
                }
            }
            ViewBag.Message = "Bu kimlik numaralı hasta kaydı bulunmaktadır.";
            return View(createdPatient);
        }

        public ActionResult GetRecentAppointment()
        {
            int patientId = Convert.ToInt32(Session["userId"]);
            var model = (from app in db.Appointment
                         where app.PatientId == patientId
                         select app).OrderByDescending(p => p.appointmentStartDate).ToList();
            return View(model);
        }

        public ActionResult EditPatient(int patientId)
        {
            var model = (from pt in db.Patient
                         where pt.patientId == patientId
                         select pt).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatient(Patient editedPatient,HttpPostedFileBase photoFile)
        {
          
            if (ModelState.IsValid)
            {
                         var patient = db.Patient.Find(editedPatient.patientId);
                patient.EmailAddress = editedPatient.EmailAddress;
                patient.gsmNumber = editedPatient.gsmNumber;
                patient.IdentityNo = editedPatient.IdentityNo;
                patient.Name = editedPatient.Name;
                patient.password = editedPatient.password;
                patient.surName = editedPatient.surName;
                patient.birthDate = editedPatient.birthDate;
                patient.isFemale = editedPatient.isFemale;
                db.SaveChanges();
                if (photoFile != null)
                {
                    SavePatientPictureFile(photoFile, editedPatient.patientId);
                }
                return RedirectToAction("GetPatientList", "Patient");
            }
            return View(editedPatient);
        }

        /// <summary>
        /// Hasta aktif/pasif eder.
        /// </summary>
        /// <param name="departmentId">İşlem yapılacak departmant</param>
        /// <returns></returns>
        public ActionResult PatientStatus(int patientId)
        {
            var model = (from pat in db.Patient
                         where pat.patientId == patientId
                         select pat).FirstOrDefault();
            if (model.isActive == true)
            {
                model.isActive = false;
            }
            else
            {
                model.isActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("GetPatientList", "Patient");
        }

        /// <summary>
        /// Doktorun fotoğrafını kaydeden metot
        /// </summary>
        /// <param name="picture">Resim Dosyası</param>
        /// <param name="patientId">Doktor Id'si</param>
        private void SavePatientPictureFile(HttpPostedFileBase picture, int patientId)
        {
            string fileName = "Patient" + patientId + picture.FileName;
            string fullPath = "~/Images/Patient/" + fileName;
            picture.SaveAs(Server.MapPath(fullPath));
            var model = db.Patient.Find(patientId);
            model.photoFile = fullPath;
            db.SaveChanges();
        }

        /// <summary>
        /// Doktorun kimlik numarasına göre varlığını kontrol eden metot
        /// </summary>
        /// <param name="identityNumber">Kimlik Numarası</param>
        /// <returns></returns>
        private bool CheckAlreadyAddedPatient(string identityNumber)
        {
            bool result = false;
            var model = (from pat in db.Patient
                         where pat.IdentityNo == identityNumber
                         select pat).FirstOrDefault();
            if (model != null)
            {
                result = false;
            }
            else
            {
                result = true;

            }
            return result;
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