using Castle.DynamicProxy;
using Core.Entities;
using Core.Utilities.Interceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.Utilities.IoC;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopWatch;

        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopWatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopWatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopWatch.Elapsed.TotalSeconds > _interval)
            {
                // Mail Gönderme
                string body = $"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name} --> {_stopWatch.Elapsed.TotalSeconds}";
                Console.Write(body);
            }
            _stopWatch.Reset();
        }

        void SendMail(string body)
        {
            string subject = "Performans Maili";


            SendMailDto sendMailDto = new SendMailDto
            {
                email = "brkntgl52@outlook.com",
                subject = subject,
                body = body,
                Email = "brkntgl52@outlook.com",
                Password = "Berkant_123456",
                Port = 587,
                SMTP = "smtp.office365.com",
                SSL = true
            };

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(sendMailDto.Email);
                mail.To.Add(sendMailDto.email);
                mail.Subject = sendMailDto.subject;
                mail.Body = sendMailDto.body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(sendMailDto.SMTP))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(sendMailDto.Email, sendMailDto.Password);
                    smtp.EnableSsl = sendMailDto.SSL;
                    smtp.Port = sendMailDto.Port;
                    smtp.Send(mail);
                }
            }
        }
    }
}
