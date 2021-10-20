using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class Patient
    {
        public int patientId { get; set; }

        [Required(ErrorMessage = "Lütfen hasta ismini giriniz."),
        Display(Name = "Hasta İsmi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen hasta ismini giriniz."),
        Display(Name = "Hasta İsmi")]
        public string surName { get; set; }


        [Required(ErrorMessage = "Lütfen kimlik numarasını giriniz."),
        Display(Name = "Kimlik Numarası"),
        StringLength(11, ErrorMessage = "Kimlik Numarası 11 hane olmalıdır.", MinimumLength = 11)]
        public string IdentityNo { get; set; }

        [Display(Name = "Cep Telefonu"),
        Required(ErrorMessage = "Lütfen cep numarasını giriniz.")]
        public string gsmNumber { get; set; }


        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Lütfen doğum tarihini belirtiniz")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birthDate { get; set; }


        [Required(ErrorMessage = "Lütfen hastanın mail adresini belirtiniz."),
        Display(Name = "Email Adresi"),
        DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Şifre"), Required(ErrorMessage = "Lütfen bir şifre belirtiniz.")]
        public string password { get; set; }

        public bool isFemale { get; set; }

        public bool isActive { get; set; }

        public string photoFile { get; set; }

        public DateTime registeredDate { get; set; }

    }
}