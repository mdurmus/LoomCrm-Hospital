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
    public class MyHospitalController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        public ActionResult GetHospitalInfo()
        {
            var model = db.MyHospital.FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHospital(MyHospital myHospital)
        {
            var model = db.MyHospital.Find(myHospital.MyHospitalId);
            model.lunchDurationMinute = myHospital.lunchDurationMinute;
            model.lunchTime = myHospital.lunchTime;
            model.Name = myHospital.Name;
            model.smsApiPage = myHospital.smsApiPage;
            model.smsHeader = myHospital.smsHeader;
            model.smsPassword = myHospital.smsPassword;
            model.smsUserName = myHospital.smsUserName;
            db.SaveChanges();
            return RedirectToAction("GetHospitalInfo","MyHospital");
        }
    }
}