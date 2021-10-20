using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class VMAppointment
    {
        public int officeId { get; set; }
        public string officeName { get; set; }

        public int departmentId { get; set; }
        public string departmentName { get; set; }

        public int doctorId { get; set; }
        public string doctorName { get; set; }

    }
}