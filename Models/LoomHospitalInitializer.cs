using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LoomCrm_Hospital.Models
{
    public class LoomHospitalInitializer : CreateDatabaseIfNotExists<LoomHospitalContext>
    {
        protected override void Seed(LoomHospitalContext context)
        {
            insertPersonal(context);
            insertPatient(context);
            insertMyHospital(context);
            insertOffice(context);
            insertDepartment(context);
            insertProfessionals(context);
        }

        private void insertMyHospital(LoomHospitalContext ctx)
        {
            MyHospital mh = new MyHospital();
            mh.lunchDurationMinute = 60;
            mh.lunchTime = new DateTime(2017,09,18,12,30,00);
            mh.Name = "Loom Crm Hospital";
            mh.smsApiPage = "http://www.apiadresiniz.com";
            mh.smsHeader = "SMS Başlık Bilgisi";
            mh.smsPassword = "SMS Password";
            mh.smsUserName = "SMS Kullanıcı Adı";
            ctx.MyHospital.Add(mh);
            ctx.SaveChanges();
        }

        private void insertPatient(LoomHospitalContext ctx)
        {
            Patient pat = new Patient();
            pat.birthDate = DateTime.Now;
            pat.EmailAddress = "hasta@loomcrm.com";
            pat.gsmNumber = "05327084420";
            pat.IdentityNo = "33127445639";
            pat.isActive = true;
            pat.isFemale = false;
            pat.Name = "Mehmet";
            pat.password = "123";
            pat.surName = "Durmuş";
            pat.registeredDate = DateTime.Now;
            
            ctx.Patient.Add(pat);
            ctx.SaveChanges();
        }

        private void insertPersonal(LoomHospitalContext ctx)
        {
            Personal admin = new Personal();
            admin.EmailAddress = "admin@loomcrm.com";
            admin.gsmNumber = "05327084420";
            admin.Name = "Mehmet";
            admin.surName = "Durmuş";
            admin.password = "1234";
            admin.isActive = true;
            admin.registeredDate = DateTime.Now;
            ctx.Personal.Add(admin);
            ctx.SaveChanges();
        }

        private void insertOffice(LoomHospitalContext ctx)
        {
            Office newOffice = new Office();
            newOffice.creationDate = DateTime.Now;
            newOffice.isActive = false;
            newOffice.mapsUrl = "Maps Adresi";
            newOffice.name = "System";
            ctx.Office.Add(newOffice);
            ctx.SaveChanges();
        }

        private void insertDepartment(LoomHospitalContext ctx)
        {
            Department newDepartment = new Department();
            newDepartment.isActive = false;
            newDepartment.medicalExamDuration = 0;
            newDepartment.name = "System";
            newDepartment.officeId = 1;
            ctx.Department.Add(newDepartment);
            ctx.SaveChanges();
        }

        private void insertProfessionals(LoomHospitalContext ctx)
        {
            List<Professional> pList = new List<Professional>();
            Professional prof = new Professional();
            prof.isActive = true;
            prof.Name = "Profesör";
            pList.Add(prof);
            Professional docent = new Professional();
            docent.isActive = true;
            docent.Name = "Doçent";
            pList.Add(docent);
            Professional ydocent = new Professional();
            ydocent.isActive = true;
            ydocent.Name = "Yardımcı Doçent";
            pList.Add(ydocent);
            Professional opdoktor = new Professional();
            opdoktor.isActive = true;
            opdoktor.Name = "Operatör Doktor";
            pList.Add(opdoktor);
            Professional uzdok = new Professional();
            uzdok.Name = "Uzman Doktor";
            uzdok.isActive = true;
            pList.Add(uzdok);
            Professional pradoktor = new Professional();
            pradoktor.isActive = true;
            pradoktor.Name = "Pratisyen Doktor";
            pList.Add(pradoktor);
            Professional disDoktor = new Professional();
            disDoktor.isActive = true;
            disDoktor.Name = "Diş Hekimi";
            pList.Add(disDoktor);
            ctx.Professional.AddRange(pList);
            ctx.SaveChanges();
        }
    }
}