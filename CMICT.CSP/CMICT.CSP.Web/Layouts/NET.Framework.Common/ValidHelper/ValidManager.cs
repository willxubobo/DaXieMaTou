using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET.Framework.Common.Extensions;

namespace NET.Framework.Common.ValidHelper
{
    public class ValidManager
    {
        public bool IsValid
        {
            get { return ErrorMessages.Count == 0; ; }
        }

        private List<string> errorMessages=new List<string>();
        public List<string> ErrorMessages
        {
            get { return errorMessages; }
        }

        public ValidManager(params string[] validItems)
        {
            foreach (string item in validItems)
            {
                if (item.IsNotEmpty())
                {
                    errorMessages.Add(item);
                }
            }
        }

        public string ToOutPut()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in ErrorMessages)
            {
                sb.AppendLine(item);
            }
            return sb.ToString();
        }
    }
}
