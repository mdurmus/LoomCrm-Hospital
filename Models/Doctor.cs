using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class Doctor
    {
        public int doctorId { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("Professional")]
        public int ProfessionalId { get; set; }

        [Required(ErrorMessage = "Lütfen TC No belirtiniz."),
         Display(Name = "TC Kimlik No"),
         StringLength(maximumLength: 11, ErrorMessage = "Kimlik No 11 karakter ve sadece sayı olmalıdır!", MinimumLength = 11)]
        public string TCIdentityNumber { get; set; }

        [Required(ErrorMessage = "Lütfen doktor ismini giriniz."),
        Display(Name = "Doktor İsmi")]
        public string name { get; set; }

        [Required(ErrorMessage = "Lütfen doktor soyismini giriniz."),
        Display(Name = "Doktor Soyismi")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Lütfen diploma numarasını giriniz."),
        Display(Name = "Diploma No")]
        public string DiplomaNumber { get; set; }

        [Display(Name = "Cep Telefonu")]
        public string gsmNumber { get; set; }

        [Required(ErrorMessage = "Lütfen mail adresi belirtiniz"),
        Display(Name = "Mail Adresi"),
        DataType(DataType.EmailAddress)]
        public string doctorMailAddress { get; set; }

        [Display(Name ="Şifre")]
        [Required(ErrorMessage ="Lütfen şifre belirtiniz.")]
        public string password { get; set; }

        public string doctorPictureFile { get; set; }

        public DateTime registeredDate { get; set; }

        [NotMapped]
        public HttpPostedFileBase doctorPicture { get; set; }

        [NotMapped]
        public string FullInfo { get { return this.name + " " + this.SurName+" "+TCIdentityNumber; } }

        public bool isActive { get; set; }

        public virtual Department Department { get; set; }

        public virtual Professional Professional { get; set; }

    }
}