using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Diagnostics;

namespace OneFourThree.App_Code
{
    public class Email
    {
        #region Sending Email
        public void SendEmail(string Subject, string Body, string To)
        {
            if (String.IsNullOrEmpty(To))
                return;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                // mail.From = new MailAddress("yourmail@gmail.com");
                mail.From = new MailAddress("waterdelivery.myanmaritstar@gmail.com");
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                //smtp.Credentials = new System.Net.NetworkCredential("yourmail@gmail.com", "password"); // ***use valid credentials***
                smtp.Credentials = new System.Net.NetworkCredential("waterdelivery.myanmaritstar@gmail.com", "00Waterdelivery@");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch
            {
                Debug.WriteLine("Error in Sending Mail");
            }
        }
        #endregion
    }
}