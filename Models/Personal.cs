using System;
using System.ComponentModel.DataAnnotations;

namespace LoomCrm_Hospital.Models
{
    public class Personal
    {
        public int personalId { get; set; }

        [Required(ErrorMessage ="Lütfen personel ismini belirtiniz."),
        Display(Name="Personel Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen personel soyismini belirtiniz."), 
        Display(Name = "Personel Soyismi")]
        public string surName { get; set; }

        [Display(Name = "Personel Soyİsmi")]
        public string gsmNumber { get; set; }

        [Display(Name="Şifre"),Required(ErrorMessage ="Lütfen bir şifre belirtiniz.")]
        public string password { get; set; }

        [Required(ErrorMessage ="Lütfen personelin mail adresini belirtiniz."),
        Display(Name ="Email Adresi"),
        DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public bool isActive { get; set; }

        public string photoFile { get; set; }

        public DateTime registeredDate { get; set; }



    }
}