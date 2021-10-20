using LoomCrm_Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoomCrm_Hospital.Controllers
{
    public class LoginController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult Login(bool? login)
        {
            if (login == false)
            {
                ViewBag.Message = "Lütfen giriş bilgilerinizi kontrol ediniz.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {

            GetVersionInformation();
            
            var hospital = db.MyHospital.FirstOrDefault();

            if (hospital.usersCanLogin == true)
            {
                var patient = (from pat in db.Patient
                               where pat.EmailAddress == email && pat.password == password && pat.isActive == true
                               select pat).FirstOrDefault();
                if (patient != null)
                {
                    Session.Add("userType", "patient");
                    Session.Add("userId", patient.patientId);
                    return RedirectToAction("PatientPage", "Home");
                }
                var personal = (from per in db.Personal
                                where per.EmailAddress == email && per.password == password && per.isActive == true
                                select per).FirstOrDefault();

                if (personal != null)
                {
                    Session.Add("userType", "personal");
                    Session.Add("userId", personal.personalId);
                    return RedirectToAction("PersonalPage", "Home");
                }

                var doctor = (from doc in db.Doctor
                              where doc.doctorMailAddress == email && doc.password == password && doc.isActive == true
                              select doc).FirstOrDefault();

                if (doctor != null)
                {
                    Session.Add("userType", "doctor");
                    Session.Add("userId", doctor.doctorId);
                    return RedirectToAction("DoctorPage", "Home");
                }
            }
            else
            {
                var personal = (from per in db.Personal
                                where per.EmailAddress == email && per.password == password && per.isActive == true
                                select per).FirstOrDefault();

                if (personal != null)
                {
                    Session.Add("userType", "personal");
                    Session.Add("userId", personal.personalId);
                    return RedirectToAction("PersonalPage", "Home");
                }

                var doctor = (from doc in db.Doctor
                              where doc.doctorMailAddress == email && doc.password == password && doc.isActive == true
                              select doc).FirstOrDefault();

                if (doctor != null)
                {
                    Session.Add("userType", "doctor");
                    Session.Add("userId", doctor.doctorId);
                    return RedirectToAction("DoctorPage", "Home");
                }
            }
            return RedirectToAction("Login", "Login", new { login = false });
        }

        private void GetVersionInformation()
        {
            
            bool office = db.MyHospital.FirstOrDefault().officeAvaliable;
            if (office)
            {
                Session.Add("officeSupport", true);
            }
            else
            {
                Session.Add("officeSupport", false);
            }

            bool department = db.MyHospital.FirstOrDefault().departmentAvaliable;
            if (department)
            {
                Session.Add("departmentSupport", true);
            }
            else
            {
                Session.Add("departmentSupport", false);
            }
        }

        public ActionResult UserDetail(int userId, string userType)
        {
            VMUser userDetail = new VMUser();
            if (userType == "patient")
            {
                var patient = (from pat in db.Patient
                               where pat.patientId == userId
                               select pat).FirstOrDefault();

                userDetail.email = patient.EmailAddress;
                userDetail.photoFile = patient.photoFile;
                userDetail.surName= patient.surName;
                userDetail.Name = patient.Name;
                userDetail.gsmNumber = patient.gsmNumber;
                userDetail.userType = Models.userType.Patient;
                userDetail.userId = userId;
                userDetail.registeredDate = patient.registeredDate;
            }
            else if (userType == "personal")
            {
                var personal = (from per in db.Personal
                                where per.personalId == userId
                                select per).FirstOrDefault();

                userDetail.email = personal.EmailAddress;
                userDetail.gsmNumber = personal.gsmNumber;
                userDetail.Name = personal.Name;
                userDetail.photoFile = personal.photoFile;
                userDetail.registeredDate = personal.registeredDate;
                userDetail.surName = personal.surName;
                userDetail.userType = Models.userType.Personal;
                userDetail.userId = userId;
            }
            else
            {
                var doctor = (from doc in db.Doctor
                                where doc.doctorId == userId
                                select doc).FirstOrDefault();

                userDetail.email = doctor.doctorMailAddress;
                userDetail.gsmNumber = doctor.gsmNumber;
                userDetail.Name = doctor.name;
                userDetail.photoFile = doctor.doctorPictureFile;
                userDetail.registeredDate = doctor.registeredDate;
                userDetail.surName = doctor.SurName;
                userDetail.userType = Models.userType.Doctor;
                userDetail.userId = userId;
            }
            return PartialView("_UserPartial", userDetail);
        }


        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login","Login");
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