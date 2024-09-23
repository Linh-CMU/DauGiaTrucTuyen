using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class MailUtils
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="_from">From.</param>
        /// <param name="_to">To.</param>
        /// <param name="_subject">The subject.</param>
        /// <param name="_body">The body.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public static async Task<bool> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client)
        {
            // Tạo nội dung Email
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);


            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends the mail local SMTP.
        /// </summary>
        /// <param name="_from">From.</param>
        /// <param name="_to">To.</param>
        /// <param name="_subject">The subject.</param>
        /// <param name="_body">The body.</param>
        /// <returns></returns>
        public static async Task<bool> SendMailLocalSmtp(string _from, string _to, string _subject, string _body)
        {
            using (SmtpClient client = new SmtpClient("localhost"))
            {
                return await SendMail(_from, _to, _subject, _body, client);
            }
        }

        /// <summary>
        /// Sends the mail google SMTP.
        /// </summary>
        /// <param name="fromEmail">From email.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="gmailSend">The gmail send.</param>
        /// <param name="gmailPassword">The gmail password.</param>
        /// <returns></returns>
        public static async Task<bool> SendMailGoogleSmtp(
                    string fromEmail,
                    string toEmail,
                    string subject,
                    string body)
        {
            MailMessage message = new MailMessage(
                from: fromEmail,
                to: toEmail,
                subject: subject,
                body: body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(fromEmail));
            message.Sender = new MailAddress(fromEmail);

            // Tạo SmtpClient kết nối đến smtp.gmail.com
            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential("nguyenanh0978638@gmail.com", "zwlcvsnblwndpbpe");
                client.EnableSsl = true;
                return await SendMail(fromEmail, toEmail, subject, body, client);
            }
        }

    }
}
