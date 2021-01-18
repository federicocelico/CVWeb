using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculumWeb.Models
{
    public class SmtpSettings
    {
        public string SMTP_Client { get; set; }
        public int SMTP_Port { get; set; }
        public string Mail_From { get; set; }
        public string Mail_Password { get; set; }
        public string Mail_To { get; set; }
    }
}
