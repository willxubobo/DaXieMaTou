using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZ.SP2013.Workflow.Common.Mail
{
    public class MailConfig
    {
        /// <summary>
        /// Mail Server
        /// </summary>
        public string Host
        {            
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Username
        /// </summary>        
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>        
        public string UserPassword
        {            
            get;
            set;
        }

        /// <summary>
        /// Sender
        /// </summary>        
        public string FromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Sender Display
        /// </summary>        
        public string FromDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Receiver
        /// </summary>        
        public List<string> ToAddresses
        {            
            get;
            set;
        }

        /// <summary>
        /// CC
        /// </summary>        
        public List<string> CCAddresses
        {
            get;
            set;
        }

        /// <summary>
        /// Mail Subject
        /// </summary>        
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Mail Content
        /// </summary>        
        public string Body
        {
            get;
            set;
        }
    }
}
