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
    public class AppointmentController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult GetAppointmentListToDay()
        {
            var model = (from app in db.Appointment
                         where app.appointmentStartDate.Day == DateTime.Now.Day &&
                         app.appointmentStartDate.Month == DateTime.Now.Month &&
                         app.appointmentStartDate.Year == DateTime.Now.Year &&
                         app.userApprove == true
                         select app).OrderBy(p => p.appointmentStartDate).ToList();

            return View(model);
        }

        public ActionResult GetOffice()
        {
            var company = db.MyHospital.FirstOrDefault();
            if (company.officeAvaliable == false && company.departmentAvaliable == false)
            {
                return RedirectToAction("GetDoctor","Appointment",new { officeId=1, departmentId=1 });
            }
            else if (company.officeAvaliable == false && company.departmentAvaliable == true)
            {
                return RedirectToAction("GetDepartment", "Appointment", new { officeId = 1});
            }
            var model = db.Office.Where(p => p.isActive == true).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectOffice(int officeId)
        {

            return RedirectToAction("GetDepartment", "Appointment", new { officeId = officeId });
        }

        public ActionResult GetDepartment(int officeId)
        {
            var model = db.Department.Where(p => p.officeId == officeId && p.isActive == true).ToList();
            ViewBag.officeId = officeId;
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectDepartment(int departmentId, int officeId)
        {

            return RedirectToAction("GetDoctor", "Appointment", new { departmentId = departmentId, officeId = officeId });
        }

        public ActionResult GetDoctor(int officeId, int departmentId)
        {
            var model = db.Doctor.Where(x => x.DepartmentId == departmentId && x.isActive == true).ToList();
            ViewBag.officeId = officeId;
            ViewBag.departmentId = departmentId;
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectDoctor(int departmentId, int officeId, int doctorId)
        {
            return RedirectToAction("CreateAppointment", "Appointment", new { departmentId = departmentId, officeId = officeId, doctorId = doctorId });
        }

        public ActionResult CreateAppointment(bool? lunch, DateTime? startDate, int departmentId, int officeId, int doctorId)
        {
            if (startDate != null)
            {
                ViewBag.Message = "İstemiş olduğunuz zaman aralığında başka bir randevu vardır.";
            }
            if (lunch == false)
            {
                ViewBag.Message = "Öğlen Tatiline randevu alamazsınız!";
            }
            var department = (from dep in db.Department
                              where dep.departmentId == departmentId
                              select dep).FirstOrDefault();

            var office = (from off in db.Office
                          where off.officeId == officeId
                          select off).FirstOrDefault();

            var doctor = (from doc in db.Doctor
                          where doc.doctorId == doctorId
                          select doc).FirstOrDefault();
            VMAppointment vmapp = new VMAppointment();
            vmapp.departmentId = departmentId;
            vmapp.departmentName = department.name;

            vmapp.doctorId = doctorId;
            vmapp.doctorName = doctor.name + " " + doctor.SurName;

            vmapp.officeId = officeId;
            vmapp.officeName = office.name;
            ViewBag.doctorId = new SelectList(db.Doctor.Where(x => x.DepartmentId == departmentId && x.isActive == true), "doctorId", "FullInfo", doctorId);
            return View(vmapp);
        }

        [HttpPost]
        public ActionResult ListAppointment(int doctorId)
        {
            var model = (from app in db.Appointment
                         where app.DoctorId == doctorId &&
                         app.userApprove == true &&
                         app.isActive == true &&
                         app.appointmentStartDate >= DateTime.Now
                         select app).ToList();
            var secenek = new object();
            secenek = model.Select(k => new
            {
                id = k.appointmentId,
                personalId = k.DoctorId,
                title = k.personalOff ? "Personel İzinli" : "Diğer Randevu",
                start = k.appointmentStartDate.AddHours(3),
                end = k.appointmentFinishDate.AddHours(3)
            });
            return Json(secenek, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAppointment(DateTime appointmentStartDate, int officeId, int patientId, int doctorId, int departmentId, string diffGsmNumber)
        {

            var examMinute = db.Department.Find(departmentId).medicalExamDuration;
            bool appointmentChechkResult = CheckAppointmentExist(appointmentStartDate, examMinute, doctorId);
            if (appointmentChechkResult == false)
            {
                return RedirectToAction("CreateAppointment", "Appointment", new { startDate = appointmentStartDate, officeId = officeId, patientId = patientId, doctorId = doctorId, departmentId = departmentId });
            }

            bool appointmentLunchCheck = CheckLunchTime(appointmentStartDate, appointmentStartDate.AddMinutes(examMinute));
            if (appointmentLunchCheck == false)
            {
                return RedirectToAction("CreateAppointment", "Appointment", new { startDate = appointmentStartDate, officeId = officeId, patientId = patientId, doctorId = doctorId, departmentId = departmentId, lunch = false });
            }
            Random generatedCode = new Random();
            int code = generatedCode.Next(1000, 9999);
            Appointment newAppointment = new Appointment();
            newAppointment.appointmentStartDate = appointmentStartDate;
            newAppointment.appointmentFinishDate = appointmentStartDate.AddMinutes(examMinute);
            newAppointment.DoctorId = doctorId;
            newAppointment.isActive = true;
            newAppointment.PatientId = patientId;
            newAppointment.ValidateCode = code.ToString();
            db.Appointment.Add(newAppointment);
            db.SaveChanges();
            return RedirectToAction("EnterValidateCode", "Appointment", new { appointmentId = newAppointment.appointmentId, gsmNumber = diffGsmNumber });
        }

        public ActionResult EnterValidateCode(int appointmentId, string gsmNumber)
        {
            var model = db.Appointment.Find(appointmentId);
            var company = db.MyHospital.FirstOrDefault();
            ViewBag.DoctorName = model.Doctor.name + " " + model.Doctor.SurName;
            ViewBag.Department = model.Doctor.Department.name;
            ViewBag.Office = model.Doctor.Department.Office.name;
            ViewBag.Date = model.appointmentStartDate;
            ViewBag.AppointmentId = appointmentId;
            if (gsmNumber != null)
            {
               // SmsWorks.SMSGonder(gsmNumber.ToString(), "Sms Kodunuz: " + model.ValidateCode, company.smsUserName, company.smsPassword, company.smsHeader, company.smsApiPage);
                ViewBag.SmsCode = model.ValidateCode;
            }
            else
            {
               // SmsWorks.SMSGonder(model.Patient.gsmNumber, "Sms Kodunuz: " + model.ValidateCode, company.smsUserName, company.smsPassword, company.smsHeader, company.smsApiPage);
                ViewBag.SmsCode = model.ValidateCode;
            }
           
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterValidateCode(int appointmentId,string code,bool? a)
        {
            var model = db.Appointment.Find(appointmentId);
            if (model.ValidateCode == code)
            {
                model.userApprove = true;
                db.SaveChanges();
                return RedirectToAction("AppointmentSuccess","Appointment",new { appointmentId= appointmentId });
            }
            return RedirectToAction("AppointemtnFailed", "Appointment",new { appointmentId = appointmentId });

        }

        public ActionResult AppointmentSuccess(int appointmentId)
        {
            var model = db.Appointment.Find(appointmentId);
            return View(model);

        }

        public ActionResult AppointemtnFailed(int appointmentId) {
            var model = db.Appointment.Find(appointmentId);
            ViewBag.Doctor = model.Doctor.name + " " + model.Doctor.SurName;
            ViewBag.Date = model.appointmentStartDate;
            db.Appointment.Remove(model);
            db.SaveChanges();
            return View(); }


        public ActionResult EditAppointmentList(int? oldDoctorId, DateTime? startDate, DateTime? finishDate)
        {

            List<Appointment> model = new List<Appointment>();
            if (oldDoctorId != null && startDate != null && finishDate != null)
            {
                model = db.Appointment.Where(p => p.DoctorId == oldDoctorId && p.appointmentStartDate >= startDate && p.appointmentFinishDate <= finishDate).ToList();
            }
            ViewBag.oldDoctorId = oldDoctorId;
            ViewBag.startDate = startDate;
            ViewBag.finishDate = finishDate;
            return View(model);
        }

        public ActionResult EditAppointmentListOnce(int appId, int? oldDoctorId, DateTime? startDate, DateTime? finishDate)
        {
            var model = db.Appointment.Find(appId);
            // Yeni Doktor seçimini yaptıracağız. bunun içinde dropdownlar ile doldurmamız gerek.
            ViewBag.doctorId = new SelectList(db.Doctor.Where(p => p.isActive == true), "doctorId", "FullInfo");
            ViewBag.oldDoctorId = oldDoctorId;
            ViewBag.startDate = startDate;
            ViewBag.finishDate = finishDate;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointmentListOnce(int appId, int oldDoctorId, int doctorId, int patientId, DateTime appointmentStartDate, DateTime startDate, DateTime finishDate)
        {
            var examduration = db.Doctor.Find(doctorId).Department.medicalExamDuration;
            var model = db.Appointment.Find(appId);
            model.DoctorId = doctorId;
            model.PatientId = patientId;
            model.appointmentStartDate = appointmentStartDate;
            model.appointmentFinishDate = appointmentStartDate.AddMinutes(examduration);
            db.SaveChanges();
            return RedirectToAction("EditAppointmentList", "Appointment", new { oldDoctorId = oldDoctorId, startDate = startDate, finishDate = finishDate });
        }

        /// <summary>
        /// Seçilen zaman aralığında randevu varlığını kontrol eder.
        /// </summary>
        /// <param name="startTime">Başlangıç Zamanı</param>
        /// <param name="examMinute">Poliklinikteki muayene süresi</param>
        /// <returns>False Zaten randevu var. True randevu eklenebilir.</returns>
        private bool CheckAppointmentExist(DateTime startTime, int examMinute, int doctorId)
         {
            DateTime finishTime = startTime.AddMinutes(examMinute);
            bool result = false;
            var model = (from app in db.Appointment
                         where
                         app.DoctorId == doctorId &&
                         app.appointmentStartDate >= startTime &&
                         app.appointmentFinishDate <= finishTime
                         select app
                         ).ToList();
            if (model.Count == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool CheckLunchTime(DateTime appointmentStart, DateTime appointmentFinish)
        {
            bool result = false;
            var model = db.MyHospital.FirstOrDefault();
            DateTime lunchStartDateTime = model.lunchTime;
            int lunchDuration = model.lunchDurationMinute;
            DateTime lunchFinishDateTime = lunchStartDateTime.AddMinutes(lunchDuration);
            TimeSpan lunchStartTime = lunchStartDateTime.TimeOfDay;
            TimeSpan lunchFinishTime = lunchFinishDateTime.TimeOfDay;
            TimeSpan appointmentStartTime = appointmentStart.TimeOfDay;
            TimeSpan appointmentFinishTime = appointmentFinish.TimeOfDay;
            if (appointmentStartTime <= lunchStartTime && appointmentFinishTime >= lunchStartTime)
            {
                result = false;
            }
            else if (appointmentStartTime >= lunchStartTime && appointmentFinishTime <= lunchFinishTime)
            {
                result = false;
            }
            else if (appointmentStartTime <= lunchFinishTime && appointmentFinishTime >= lunchFinishTime)
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