﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace GenEmailingService
{
    public class EmailUtil
    {
        NetworkCredential credentials;
        SmtpClient client;

        public string LogFile {
            get { return string.Format(ConfigurationManager.AppSettings["logfile"], DateTime.Now.Day, 
                DateTime.Now.Month, DateTime.Now.Year); }
        }

        public EmailUtil()
        {
            credentials = new NetworkCredential(ConfigurationManager.AppSettings["AdminEmail"], ConfigurationManager.AppSettings["AdminPass"]);
            client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
        }

        public bool SendEmail(string toEmail, string toName, string fromEmail, string fromName, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);
                message.IsBodyHtml = true;
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.Send(message);
                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " - Mail sent to " + toEmail);
                }
                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " - " + ex.Message);                    
                }
                return false;
            }
        }
    }
}
