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
    public class ReportsController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        /// <summary>
        /// Şube ve poliklinik seçerek çekilecek rapor ekranı
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOfficeDepartmentReport()
        {

            var officelist = new List<SelectListItem>();
            officelist.Add(new SelectListItem { Value = "0", Text = "--Lütfen Şube Seçiniz--" });
            foreach (var item in db.Office)
            {
                officelist.Add(new SelectListItem { Value = item.officeId.ToString(), Text = item.name });
            }
            ViewBag.officeId = officelist;
            return View();
        }

        /// <summary>
        /// Şube ve poliklinik seçerek çekilecek rapor metodu
        /// </summary>
        /// <param name="officeId">Şube Kodu</param>
        /// <param name="departmentId">Poliklinik Kodu</param>
        /// <param name="start">Raporun başlangıç Tarihi</param>
        /// <param name="finish">Raporun bitiş Tarihi</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOfficeDepartmentReport(int officeId, int departmentId, DateTime? start, DateTime? finish)
        {
            List<Appointment> model = new List<Appointment>();
            if (start != null && finish == null)
            {
                List<int> getDoctorIdList = GetDoctorList(departmentId);
                foreach (var item in getDoctorIdList)
                {
                    var appointmentList = db.Appointment.Where(p => p.appointmentStartDate >= start && p.DoctorId == item && p.userApprove == true).ToList();
                    model.AddRange(appointmentList);
                }
            }
            else if (start == null && finish != null)
            {
                List<int> getDoctorIdList = GetDoctorList(departmentId);
                foreach (var item in getDoctorIdList)
                {
                    var appointmentList = db.Appointment.Where(p => p.appointmentStartDate <= finish && p.DoctorId == item && p.userApprove == true).ToList();
                    model.AddRange(appointmentList);
                }
            }
            else if (start == null && finish == null)
            {
                List<int> getDoctorIdList = GetDoctorList(departmentId);
                foreach (var item in getDoctorIdList)
                {
                    var appointmentList = db.Appointment.Where(p => p.DoctorId == item && p.userApprove == true).ToList();
                    model.AddRange(appointmentList);
                }

            }
            else
            {
                List<int> getDoctorIdList = GetDoctorList(departmentId);
                foreach (var item in getDoctorIdList)
                {
                    var appointmentList = db.Appointment.Where(p => p.DoctorId == item && p.appointmentStartDate >= start && p.appointmentStartDate <= finish && p.userApprove == true).ToList();
                    model.AddRange(appointmentList);
                }
            }
            var officelist = new List<SelectListItem>();
            officelist.Add(new SelectListItem { Value = "0", Text = "--Lütfen Şube Seçiniz--" });
            foreach (var item in db.Office)
            {
                officelist.Add(new SelectListItem { Value = item.officeId.ToString(), Text = item.name });
            }
            ViewBag.officeId = officelist;
            return View(model);
        }

        /// <summary>
        /// Seçilen poliklinikteki doktorlar üzerinden alınan randevuları getirmek için kullanacağım metot.
        /// </summary>
        /// <param name="departmentId">Poliklinik Id'si</param>
        /// <returns>Bölümdeki doktorları listeler.</returns>
        private List<int> GetDoctorList(int departmentId)
        {
            List<int> doctorsId = new List<int>();
            var model = db.Doctor.Where(p => p.DepartmentId == departmentId).ToList();
            foreach (var item in model)
            {
                doctorsId.Add(item.doctorId);
            }
            return doctorsId;
        }

        /// <summary>
        /// Seçilen doktora göre rapor veren ekran
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReportByDoctorId()
        {
            ViewBag.doctorId = new SelectList(db.Doctor.ToList(), "doctorId", "FullInfo");
            return View();
        }

        /// <summary>
        /// Seçilen doktora göre rapor veren metot
        /// </summary>
        /// <param name="doctorId">Doktor Id'si</param>
        /// <param name="start">Raporun başlangıç tarihi</param>
        /// <param name="finish">Raporun bitiş tarihi</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportByDoctorId(int doctorId, DateTime? start, DateTime? finish)
        {
            List<Appointment> model = new List<Appointment>();
            if (start != null && finish == null)
            {
                model = (from app in db.Appointment
                         where app.DoctorId == doctorId &&
                         app.appointmentStartDate >= start
                         select app).ToList();
            }
            else if (start == null && finish != null)
            {
                model = (from app in db.Appointment
                         where app.DoctorId == doctorId &&
                         app.appointmentStartDate <= finish
                         select app).ToList();
            }
            else if (start != null && finish != null)
            {
                model = (from app in db.Appointment
                         where app.DoctorId == doctorId &&
                         app.appointmentStartDate >= start &&
                         app.appointmentStartDate <= finish
                         select app).ToList();
            }
            else
            {
                model = (from app in db.Appointment
                         where app.DoctorId == doctorId
                         select app).ToList();
            }
            ViewBag.doctorId = new SelectList(db.Doctor.ToList(), "doctorId", "FullInfo");
            return View(model);
        }

        /// <summary>
        /// Hasta bilgilerine göre rapor veren ekran
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReportByPatient()
        {

            return View();
        }

        /// <summary>
        /// Hasta bilgilerine göre rapor veren metot
        /// </summary>
        /// <param name="tcIdNumber">Hasta TC no</param>
        /// <param name="start">Başlangıç Tarihi</param>
        /// <param name="finish">Bitiş Tarihi</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportByPatient(string tcIdNumber, DateTime? start, DateTime? finish)
        {
            List<Appointment> model = new List<Appointment>();
            if (start != null && finish == null)
            {
                model = (from app in db.Appointment
                         where app.Patient.IdentityNo == tcIdNumber &&
                         app.appointmentStartDate >= start
                         select app).ToList();
            }
            else if (start == null && finish != null)
            {
                model = (from app in db.Appointment
                         where app.Patient.IdentityNo == tcIdNumber &&
                         app.appointmentStartDate <= finish
                         select app).ToList();
            }
            else if (start != null && finish != null)
            {
                model = (from app in db.Appointment
                         where app.Patient.IdentityNo == tcIdNumber &&
                         app.appointmentStartDate >= start &&
                         app.appointmentStartDate <= finish
                         select app).ToList();
            }
            else
            {
                model = (from app in db.Appointment
                         where app.Patient.IdentityNo == tcIdNumber
                         select app).ToList();
            }
            return View(model);
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