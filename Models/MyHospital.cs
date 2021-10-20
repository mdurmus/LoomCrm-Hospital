using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class MyHospital
    {
        public int MyHospitalId { get; set; }

        [Display(Name="Hastane Adı")]
        [Required(ErrorMessage ="Lütfen bir isim belirtiniz.")]
        public string Name { get; set; }

        [Display(Name = "Öğle Arası Saati")]
        [Required(ErrorMessage = "Lütfen bir saat belirtiniz.")]
        [DataType(DataType.Time)]
        public DateTime lunchTime { get; set; }

        [Display(Name = "Öğle Tatili Süresi")]
        [Required(ErrorMessage = "Lütfen bir dakika olarak belirtiniz.")]
        
        public int lunchDurationMinute { get; set; }

        [Display(Name = "Sms Kullanıcı Adı")]
        [Required(ErrorMessage = "Sms firmanızdan aldığınız adı giriniz.")]
        public string smsUserName { get; set; }

        [Display(Name = "Sms Şifre")]
        [Required(ErrorMessage = "Sms firmanızdan aldığınız şifreyi giriniz.")]
        public string smsPassword { get; set; }

        [Display(Name = "Sms Başlık")]
        [Required(ErrorMessage = "Sms firmanızdan aldığınız başlık bilgisini giriniz.")]
        public string smsHeader { get; set; }

        [Display(Name = "Sms Başlık")]
        [Required(ErrorMessage = "Sms firmanızdan aldığınız API adresini giriniz.")]
        [DataType(DataType.Url,ErrorMessage ="Lütfen geçerli bir url giriniz.")]
        public string smsApiPage { get; set; }

        public bool usersCanLogin { get; set; }

        public bool officeAvaliable { get; set; }

        public bool departmentAvaliable { get; set; }

    }
}