using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AZ.SP2013.Workflow.Common.Mail
{
    public class SmtpMailUtil
    {
        public static bool SendMail(MailConfig mailConfig)
        {
            bool useThirdPartMailServer = bool.Parse(ConfigurationManager.AppSettings.Get("UseThirdPartMailServer"));

            if (useThirdPartMailServer)
            {
                return SendMailWithSLL(mailConfig);
            }
            else
            {
                return SendMailWithDefault(mailConfig);
            }
        }

        public static bool SendMailWithDefault(MailConfig mailConfig)
        {
            //if the switch: SendNoticeEmail was set to false，it will not send mail.
            bool isSendEmail = bool.Parse(ConfigurationManager.AppSettings.Get("IfSendEmail"));
            if (!isSendEmail)
            {
                return true;
            }

            try
            {
                string mailFromAddress = ConfigurationManager.AppSettings.Get("FromEmailAddress");
                string mailServer = ConfigurationManager.AppSettings.Get("EmailServer").ToString();
                int emailServerPortal = int.Parse(ConfigurationManager.AppSettings.Get("EmailServerPortal"));

                string mailUserName = ConfigurationManager.AppSettings.Get("SMTPUserAccount");
                string mailPassword = ConfigurationManager.AppSettings.Get("SMTPUserPassword");

                mailConfig.Host = mailServer;
                mailConfig.Port = emailServerPortal;
                mailConfig.FromAddress = mailFromAddress;
                mailConfig.FromDisplay = mailFromAddress;

                mailConfig.UserName = mailUserName;
                mailConfig.UserPassword = mailPassword;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(mailConfig.FromAddress, mailConfig.FromDisplay);

                for (int i = 0; i < mailConfig.ToAddresses.Count; i++)
                {
                    string address = mailConfig.ToAddresses[i].Trim();
                    if (address != "")
                    {
                        mailMessage.To.Add(new MailAddress(address));
                    }
                }

                for (int i = 0; i < mailConfig.ToAddresses.Count; i++)
                {
                    string address = mailConfig.ToAddresses[i].Trim();
                    if (address != "")
                    {
                        mailMessage.To.Add(new MailAddress(address));
                    }
                }

                mailMessage.Subject = mailConfig.Subject;
                mailMessage.Body = mailConfig.Body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                SmtpClient sc = new SmtpClient(mailConfig.Host);
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                //sc.Credentials = CredentialCache.DefaultNetworkCredentials;// 
                sc.Credentials = new NetworkCredential(mailConfig.UserName, mailConfig.UserPassword);
                sc.Send(mailMessage);
                //sc.SendMailAsync(mailMessage);
                mailMessage.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendMailWithSLL(MailConfig mailConfig)
        {
            //if the switch: SendNoticeEmail was set to false，it will not send mail.
            bool isSendEmail = bool.Parse(ConfigurationManager.AppSettings.Get("IfSendEmail"));
            if (!isSendEmail)
            {
                return true;
            }

            try
            {
                string mailServer = ConfigurationManager.AppSettings.Get("SMTPServerAddress").ToString();
                int emailServerPortal = int.Parse(ConfigurationManager.AppSettings.Get("SMTPServerPort"));

                string mailUserName = ConfigurationManager.AppSettings.Get("SMTPUserAccount");
                string mailPassword = ConfigurationManager.AppSettings.Get("SMTPUserPassword");

                mailConfig.Host = mailServer;
                mailConfig.Port = emailServerPortal;
                mailConfig.UserName = mailUserName;
                mailConfig.UserPassword = mailPassword;
                mailConfig.FromAddress = mailUserName;
                mailConfig.FromDisplay = mailUserName;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(mailConfig.FromAddress, mailConfig.FromDisplay);
                if (mailConfig.ToAddresses != null)
                {
                    for (int i = 0; i < mailConfig.ToAddresses.Count; i++)
                    {
                        string address = mailConfig.ToAddresses[i].Trim();
                        if (address != "")
                        {
                            mailMessage.To.Add(new MailAddress(address));
                        }
                    }
                }

                mailMessage.Subject = mailConfig.Subject;
                mailMessage.Body = mailConfig.Body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                SmtpClient sc = new SmtpClient(mailConfig.Host, mailConfig.Port);

                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential(mailConfig.UserName, mailConfig.UserPassword);

                //sc.Send(mailMessage);
                //mailMessage.Dispose();
                
                sc.SendMailAsync(mailMessage);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
