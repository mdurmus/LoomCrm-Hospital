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
    public class HomeController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult PersonalPage()
        {
            ViewBag.Mesaj = "Personel Sayfası";
            ViewBag.OfficeSupport = db.MyHospital.FirstOrDefault().officeAvaliable;
            ViewBag.DepartmentSupport = db.MyHospital.FirstOrDefault().departmentAvaliable;
            return View();
        }

        public ActionResult PatientPage()
        {
            int patientId = Convert.ToInt32(Session["userId"]);
            var model = db.Appointment.Where(x => x.PatientId == patientId && x.userApprove == true).ToList();
            var lastAppointment = model.OrderByDescending(p => p.appointmentStartDate).Take(10);
            var lastDoctor = model.LastOrDefault();
            if (lastDoctor != null)
            {
                ViewBag.lastDoctor = lastDoctor.Doctor.name + " " + lastDoctor.Doctor.SurName;
            }
            ViewBag.appointmentTotalCount = model.Count;
            return View(lastAppointment);
        }


        public ActionResult DoctorPage()
        {
            ViewBag.doctorId = Convert.ToInt32(Session["userId"]);
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