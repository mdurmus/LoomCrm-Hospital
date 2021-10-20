using LoomCrm_Hospital.Filters;
using LoomCrm_Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoomCrm_Hospital.Controllers
{
    [GirisKontrol]
    public class DoctorController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        /// <summary>
        /// Kayıtlı tüm doktorların listesini verir.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDoctorList()
        {
            var model = (from doc in db.Doctor
                         select doc).ToList();
            return View(model);
        }

        /// <summary>
        /// Sisteme yeni doktor ekleme ekranı
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDoctor()
        {
            bool officeSupport = db.MyHospital.FirstOrDefault().officeAvaliable;
            ViewBag.professionalId = new SelectList(db.Professional.Where(p=>p.isActive == true).ToList(), "professionalId", "name");
            if (officeSupport == true)
            {
                ViewBag.officeId = new SelectList(db.Office.Where(p => p.isActive == true).ToList(), "officeId", "name");
            }
            else
            {
                ViewBag.officeId = new SelectList(db.Office.ToList(), "officeId", "name");
            }
            return View();
        }

        /// <summary>
        /// Sisteme yeni doktor ekleyen metot
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDoctor(Doctor doctor)
        {
            bool isItAddedDoctor = CheckAlreadyAddedDoctor(doctor.TCIdentityNumber);
            if (isItAddedDoctor == true)
            {
                if (ModelState.IsValid)
                {
                    doctor.isActive = true;
                    doctor.registeredDate = DateTime.Now;
                    db.Doctor.Add(doctor);
                    db.SaveChanges();
                    if (doctor.doctorPicture != null)
                    {
                        SaveDoctorPictureFile(doctor.doctorPicture, doctor.doctorId);
                    }
                    return RedirectToAction("GetDoctorList", "Doctor");
                }
            }
            ViewBag.Message = "Bu kimlik numaralı doktorun sistemde kaydı vardır.";
            ViewBag.professionalId = new SelectList(db.Professional.Where(p => p.isActive == true).ToList(), "professionalId", "name");
            ViewBag.officeId = new SelectList(db.Office.Where(p => p.isActive == true).ToList(), "officeId", "name");

            ViewBag.departmentId = new SelectList(db.Department.Where(p => p.isActive == true).ToList(), "departmentId", "FullInfo");
            return View(doctor);
        }

        [HttpPost]
        public ActionResult GetDepartment(int officeId)
        {
            bool departmentSupport = db.MyHospital.FirstOrDefault().departmentAvaliable;
            List<Department> department = new List<Department>();
            if (departmentSupport)
            {
                department = (from dep in db.Department
                              where dep.officeId == officeId &&
                              dep.isActive == true
                              select dep).ToList();
            }
            else
            {
                department = (from dep in db.Department
                              where dep.officeId == officeId
                              select dep).ToList();
            }
            
            List<SelectListItem> departmentList = new List<SelectListItem>();
            if (department != null)
            {
                foreach (var x in department)
                {
                    departmentList.Add(new SelectListItem { Text = x.name, Value = x.departmentId.ToString() });
                }
            }
            return Json(new SelectList(departmentList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        [HttpPost]
        public ActionResult GetDoctor(int departmentId)
        {
            var doctors = (from doc in db.Doctor
                              where doc.DepartmentId == departmentId &&
                              doc.isActive == true
                              select doc).ToList();
            List<SelectListItem> departmentList = new List<SelectListItem>();
            if (doctors != null)
            {
                foreach (var x in doctors)
                {
                    departmentList.Add(new SelectListItem { Text = x.name+" "+x.SurName+" "+x.TCIdentityNumber, Value = x.doctorId.ToString() });
                }
            }
            return Json(new SelectList(departmentList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }



        /// <summary>
        /// Doktorun bilgilerini getiren ekran
        /// </summary>
        /// <param name="doctorId">Doktor Id'si</param>
        /// <returns></returns>
        public ActionResult EditDoctor(int doctorId)
        {
            var model = (from doc in db.Doctor
                         where doc.doctorId == doctorId
                         select doc).FirstOrDefault();

            ViewBag.professionalId = new SelectList(db.Professional.Where(x=>x.isActive==true).ToList(), "professionalId", "name", model.ProfessionalId);
            ViewBag.officeId = new SelectList(db.Office.Where(p => p.isActive == true).ToList(), "officeId", "name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDoctor(Doctor doc)
        {

            if (ModelState.IsValid)
            {
                var model = (from doct in db.Doctor
                             where doct.doctorId == doc.doctorId
                             select doct).FirstOrDefault();
                model.DepartmentId = doc.DepartmentId;
                model.DiplomaNumber = doc.DiplomaNumber;
                model.doctorMailAddress = doc.doctorMailAddress;
                model.gsmNumber = doc.gsmNumber;
                model.name = doc.name;
                model.ProfessionalId = doc.ProfessionalId;
                model.SurName = doc.SurName;
                model.TCIdentityNumber = doc.TCIdentityNumber;
                db.SaveChanges();
                if (doc.doctorPicture != null)
                {
                    SaveDoctorPictureFile(doc.doctorPicture, doc.doctorId);
                }
                return RedirectToAction("GetDoctorList", "Doctor");
            }
            ViewBag.professionalId = new SelectList(db.Professional.Where(p => p.isActive == true).ToList(), "professionalId", "name", doc.ProfessionalId);
            ViewBag.departmentId = new SelectList(db.Department.Where(p => p.isActive == true).ToList(), "departmentId", "FullInfo",
                doc.DepartmentId);
            return View(doc);
        }
        
        /// <summary>
        /// Doktorun kimlik numarasına göre varlığını kontrol eden metot
        /// </summary>
        /// <param name="identityNumber">Kimlik Numarası</param>
        /// <returns></returns>
        private bool CheckAlreadyAddedDoctor(string identityNumber)
        {
            bool result = false;
            var model = (from doc in db.Doctor
                         where doc.TCIdentityNumber == identityNumber
                         select doc).FirstOrDefault();
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

        /// <summary>
        /// Doktorun fotoğrafını kaydeden metot
        /// </summary>
        /// <param name="picture">Resim Dosyası</param>
        /// <param name="doctorId">Doktor Id'si</param>
        private void SaveDoctorPictureFile(HttpPostedFileBase picture, int doctorId)
        {
            string fileName = "Doctor" + doctorId + picture.FileName;
            string fullPath = "~/Images/Doctor/" + fileName;
            picture.SaveAs(Server.MapPath(fullPath));
            var model = db.Doctor.Find(doctorId);
            model.doctorPictureFile = fullPath;
            db.SaveChanges();
        }

        /// <summary>
        /// Doktorun Aktif/Pasif durumunu düzenler.
        /// </summary>
        /// <param name="doctorId">Doktor Id'si ni verin</param>
        /// <returns></returns>
        public ActionResult DoctorStatus(int doctorId)
        {
            var model = (from doc in db.Doctor
                         where doc.doctorId == doctorId
                         select doc).FirstOrDefault();
            if (model.isActive == true)
            {
                model.isActive = false;
            }
            else
            {
                model.isActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("GetDoctorList", "Doctor");
        }

        public ActionResult DoctorTakeOff()
        {
            ViewBag.doctorId = new SelectList(db.Doctor.Where(p => p.isActive == true).OrderBy(p => p.name), "doctorId", "FullInfo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorTakeOff(int doctorId, DateTime start, DateTime finish)
        {
            var model = (from app in db.Appointment
                         where app.DoctorId == doctorId &&
                         app.appointmentStartDate >= start &&
                         app.appointmentFinishDate <= finish
                         select app).ToList();
            if (model.Count >0)
            {
              return RedirectToAction("EditAppointmentList", "Appointment",new {oldDoctorId=doctorId, startDate=start,finishDate=finish });
            }
            ViewBag.doctorId = new SelectList(db.Doctor.Where(p => p.isActive == true).OrderBy(p => p.name), "doctorId", "FullInfo");
            ViewBag.Message = "Girmiş olduğunuz tarih aralığında randevu kaydı yoktur!";
            return View();
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