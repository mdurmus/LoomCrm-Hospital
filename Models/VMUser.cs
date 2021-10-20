using System;

namespace LoomCrm_Hospital.Models
{
    public enum userType
    {
        Patient,
        Personal,
        Doctor
    }


    public class VMUser
    {
        public int userId { get; set; }
        public userType userType { get; set; }
        public string Name { get; set; }
        public string surName { get; set; }
        public string email { get; set; }
        public string photoFile { get; set; }
        public string gsmNumber { get; set; }
        public DateTime registeredDate { get; set; }
    }
}