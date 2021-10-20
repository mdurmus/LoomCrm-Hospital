using LoomCrm_Hospital.Filters;
using LoomCrm_Hospital.Models;
using System.Linq;
using System.Web.Mvc;

namespace LoomCrm_Hospital.Controllers
{
    [GirisKontrol]
    public class DepartmentController : Controller
    {
        LoomHospitalContext db = new LoomHospitalContext();

        /// <summary>
        /// Hastane bünyesindeki tüm poliklinikleri listeler
        /// </summary>
        /// <returns>Poliklinik listesi</returns>
        public ActionResult GetDepartmentList()
        {
            var model = (from dep in db.Department
                         where dep.name!="System"
                         select dep).ToList();
            return View(model);
        }

        /// <summary>
        /// Yeni poliklinik ekler.
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDepartment()
        {
            ViewBag.officeId = new SelectList(db.Office.Where(v => v.isActive == true),"officeId","name").OrderBy(p=>p.Text).ToList();
            return View();
        }

        /// <summary>
        /// Yeni eklenecek polikliğin post metodu
        /// </summary>
        /// <param name="dep">Poliklinik Adı</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDepartment(Department dep)
        {
          bool result =  CheckDepartment(dep.name,dep.officeId);
            if (result != false)
            {
                if (ModelState.IsValid)
                {
                    dep.isActive = true;
                    db.Department.Add(dep);
                    db.SaveChanges();
                    return RedirectToAction("GetDepartmentList", "Department");
                }
            }
            ViewBag.Message = "Eklemek istediğiniz poliklinik bu şubede zaten ekli";
            ViewBag.officeId = new SelectList(db.Office.Where(v => v.isActive == true), "officeId", "name").OrderBy(p => p.Text).ToList();
            return View(dep);
        }

        /// <summary>
        /// Poliklinik bilgilerinin düzenleneceği ekran
        /// </summary>
        /// <param name="departmentId">Poliklinik Id'si</param>
        /// <returns></returns>
        public ActionResult EditDepartment(int departmentId)
        {
            var model = (from dep in db.Department
                         where dep.departmentId == departmentId
                         select dep).FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// Polikliniğin düzenlendiği metot
        /// </summary>
        /// <param name="department">Düzenlenmiş olarak poliklinik verir.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment(Department department)
        {
            var model = db.Department.Find(department.departmentId);
                if (ModelState.IsValid)
                {
                    model.medicalExamDuration = department.medicalExamDuration;
                    model.name = department.name;
                    model.officeId = department.officeId;
                    db.SaveChanges();
                    return RedirectToAction("GetDepartmentList","Department");
                }
            return View(department);
        }

        /// <summary>
        /// Departmanın sitemde eklenme durumunu kontrol eder.
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <param name="officeId">Şube Kodu</param>
        /// <returns></returns>
        private bool CheckDepartment(string department,int officeId)
        {
            bool sonuc;
            var model = (from dep in db.Department
                         where dep.officeId == officeId &&
                         dep.name == department
                         select dep).FirstOrDefault();
            if (model != null)
            {
                sonuc = false;
            }
            else
            {
                sonuc = true;
            }
            return sonuc;
        }
        
        /// <summary>
        /// Polikliniği aktif/pasif eder.
        /// </summary>
        /// <param name="departmentId">İşlem yapılacak departmant</param>
        /// <returns></returns>
        public ActionResult DepartmentStatus(int departmentId)
        {
            var model = (from dep in db.Department
                         where dep.departmentId == departmentId
                         select dep).FirstOrDefault();
            if (model.isActive == true)
            {
                model.isActive = false;
            }
            else
            {
                model.isActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("GetDepartmentList","Department");
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