using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    
    public class Appointment
    {
        [Key]
        public int appointmentId { get; set; }
                
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage ="Lütfen muayene tarihini belirtiniz.")]
        [DataType(DataType.Date)]
        [Display(Name ="Muayene Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime appointmentStartDate { get; set; }

        public DateTime appointmentFinishDate { get; set; }

        public bool userApprove { get; set; }

        public bool isActive { get; set; }

        public bool personalOff { get; set; }

        public string ValidateCode { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        





    }
}