using LoomCrm_Hospital.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace LoomCrm_Hospital.Controllers
{
    public static class SmsWorks
    {

        public static void SMSGonder(string numaralar, string mesaj, string kullaniciBilgisi, string sifreBilgisi, string baslikBilgisi, string apiAdresi)
        {
            string kullaniciAdi = kullaniciBilgisi, sifre = sifreBilgisi, baslik = baslikBilgisi;
            string xmlRequest = "";
            xmlRequest += "<request>";
            xmlRequest += "  <authentication>";
            xmlRequest += "    <username>" + kullaniciAdi + "</username>";
            xmlRequest += "    <password>" + sifre + "</password>";
            xmlRequest += "  </authentication>";
            xmlRequest += "  <order>";
            xmlRequest += "    <sender>" + baslik + "</sender>";
            xmlRequest += "    <message>";
            xmlRequest += "      <text><![CDATA[" + mesaj + "]]></text>";
            xmlRequest += "      <receipents>";
            xmlRequest += "        <number>" + numaralar.Remove(0, 1) + "</number>";
            xmlRequest += "      </receipents>";
            xmlRequest += "    </message>";
            xmlRequest += "  </order>";
            xmlRequest += "</request>";
            SendApi("Doğrulama Kodu", numaralar, xmlRequest, apiAdresi);

        }

        private static void SendApi(string mesajTip, string numara, string desen, string apiAdres)
        {
            // APIYE XML DESENİ VE API ADRESİ GÖNDERİLİYOR.
            WebRequest request = WebRequest.Create(apiAdres);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(desen);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            SaveSMSLog(responseFromServer, numara, mesajTip);
        }

        private static void SaveSMSLog(string responseFromServer, string numara, string mesajTipi)
        {
            string StatusCode = string.Empty;
            string StatusMessage = string.Empty;
            string orderId = string.Empty;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(responseFromServer);
            XmlNodeList nodeList = xml.DocumentElement.SelectNodes("/response/status");
            foreach (XmlNode node in nodeList)
            {
                StatusCode = node.SelectSingleNode("code").InnerText;
                StatusMessage = node.SelectSingleNode("message").InnerText;
            }
            nodeList = xml.DocumentElement.SelectNodes("/response/order");
            foreach (XmlNode node in nodeList)
            {
                orderId = node.SelectSingleNode("id").InnerText;
            }
            using (LoomHospitalContext db = new LoomHospitalContext())
            {
                SmsLog sl = new SmsLog();
                sl.resultCode = StatusCode;
                sl.MessageType = mesajTipi;
                sl.mobileNumberToSms = numara;
                sl.resultMessage = StatusMessage;
                sl.proccessDate = DateTime.Now;
                sl.orderId = orderId;
                db.SmsLog.Add(sl);
                db.SaveChanges();
            }
        }
    }
}