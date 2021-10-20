using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class Department
    {
        [Key]
        public int departmentId { get; set; }

        [ForeignKey("Office")]
        public int officeId { get; set; }

        [Required(ErrorMessage ="Lütfen poliklinik adını belirtiniz."),
        Display(Name ="Poliklinik Adı")]
        public string name { get; set; }

        [Display(Name ="Muayene Süresi"),
        Required(ErrorMessage ="Lütfen muayene süresini belirtiniz.")]
        public int medicalExamDuration { get; set; }

        public bool isActive { get; set; }

        public virtual Office Office { get; set; }

        public virtual ICollection<Doctor> doctors { get; set; }

        public string FullInfo { get { return this.Office.name + " " + this.name; } }

    }
}