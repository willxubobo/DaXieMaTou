using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.Model
{
    public class NavigationNodeModel
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public List<NavigationNodeModel> ChildNodes { get; set; }
    }
}
