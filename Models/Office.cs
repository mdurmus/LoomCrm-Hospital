using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class Office
    {
        public int officeId { get; set; }

        [Display( Name ="Şube Adı"),
        Required(ErrorMessage ="Lütfen Şube Adını Belirtiniz")]
        public string name { get; set; }

        [DataType(DataType.Url),
        Display(Name ="Google Maps Adresi")]
        public string mapsUrl { get; set; }

        [Display(Name ="Şube Görseli")]
        public string officePicturePath { get; set; }

        public bool isActive { get; set; }

        public DateTime creationDate { get; set; }

        [NotMapped]
        public HttpPostedFileBase officePicture { get; set; }

        public ICollection<Department> departments { get; set; }

    }
}