using CMICT.CSP.Model.InspectionModels;
using NET.Framework.Common.AD;
using NET.Framework.Common.Data;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class BusinessOnLineComponent:BaseComponent
    {
        public APPOINTMENT GetAppointment(string xml)
        {
            return SerializeHelper.FromXml<APPOINTMENT>(xml);
        }


        public string FormatContainers(Containers containers)
        {
            return SerializeHelper.ToXml(containers);
        }

        public CUSTOMSINSPECTIONS GetCustomsInspections(string xml)
        {
            return SerializeHelper.FromXml<CUSTOMSINSPECTIONS>(xml);
        }

        public string GetCustomsInspections(CUSTOMSINSPECTIONS xml)
        {
            return SerializeHelper.ToXml(xml);
        }

        public Company GetCompanyInfo(string loginName)
        {
            Company company = new Company();
            if (loginName == "")
                return company;

            loginName = loginName.Split('\\')[1];

            var de = ADHelper.GetDirectoryEntryByAccount(loginName);
            if (de == null)
            {
                BaseComponent.Error("LoginName:"+loginName+" cannot find in AD");
                return company;
            }
            //TODO: 
            company.Costomer = ADHelper.GetProperty(de, "company");
            //company.linkMan = ADHelper.GetProperty(de, "givenName");
            company.phone = ADHelper.GetProperty(de, "mobile");
            return company;
        }
    }
}
