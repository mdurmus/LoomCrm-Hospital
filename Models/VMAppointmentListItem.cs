using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class VMAppointmentListItem
    {
        public int appointmentId { get; set; }
        public string officeName { get; set; }
        public string departmentName { get; set; }
        public string doctorFullName { get; set; }
        public string patientFullName { get; set; }
        public DateTime appointmentDate { get; set; }
        public TimeSpan appointmentTime { get; set; }
    }
}