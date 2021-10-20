using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoomCrm_Hospital.Models
{
    public class Professional
    {
        public int professionalId { get; set; }

        [Display(Name = "Uzmanlık Tipi"),
        Required(ErrorMessage = "Lütfen bir uzmanlık belirtiniz.")]
        public string Name { get; set; }

        public bool isActive { get; set; }


        public virtual ICollection<Doctor> doctors { get; set; }
    }
}