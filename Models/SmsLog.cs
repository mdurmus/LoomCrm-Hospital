using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoomCrm_Hospital.Models
{
    public class SmsLog
    {
        public int smslogId { get; set; }
        public string resultCode { get; set; }
        public string MessageType { get; set; }
        public string resultMessage { get; set; }
        public string orderId { get; set; }
        public DateTime proccessDate { get; set; }
        public string mobileNumberToSms { get; set; }


    }
}