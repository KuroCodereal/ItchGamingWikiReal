using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    public class IGWMail
    {
        //get date from datepicker
        /*
         https://stackoverflow.com/questions/31612903/how-to-get-the-value-from-datepicker-in-textfield
         */
        private string MailHost;
        private int MailPort;
        private string MailUserName;
        private string MailPassword;
        public IGWMail(string _MailHost,int _MailPort,string _MailUser,string _MailPass)
        {
            if(string.IsNullOrEmpty(_MailHost) || _MailPort <= 0 || string.IsNullOrEmpty(_MailUser) || string.IsNullOrEmpty(_MailPass))
            {
                throw new Exception("Các giá trị đầu vào không được để trống!");
            }

            this.MailHost = _MailHost;
            this.MailPort = _MailPort;
            this.MailUserName = _MailUser;
            this.MailPassword = _MailPass;

        }
        /*
         Các param cần thiết cho 1 email:
        sendTo: Email cần gửi
        sub: Subject Email
        content: Nội dung Email (có thể là html)
        attachment: Các file hoặc các tệp đính kèm theo
         */

        public void SendMail(string sendTo,string sub, string content, List<Attachment> attachments)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Host = this.MailHost ?? "smtp.gmail.com";
                smtp.Port = this.MailPort <= 0 ? 587 : this.MailPort;
                smtp.EnableSsl = true;


                //--------------
                /*
                 smtp.Timeout = 200000; //ms ?
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                 */
                //--------------

                smtp.Credentials = new NetworkCredential(this.MailUserName,this.MailPassword);
                MailMessage mail = new MailMessage(this.MailUserName, sendTo, sub, content);
                if(attachments != null)
                {
                    foreach(Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                smtp.Send(mail);
            }
            catch
            {
                return;
            }
        }
    }
}
