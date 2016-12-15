using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CMICT.CSP.Model.InspectionModels
{
    [Serializable]
    public class CUSTOMSINSPECTIONS
    {

        [XmlElement(ElementName = "CONTAINER")]
        public List<container> CONTAINER { get; set; }
    }
    [Serializable]
    public class container
    {
        public string CONTAINERNO { get; set; }
        public string CONTAINERSIZE { get; set; }
        public string CONTAINERTYPE { get; set; }
        public string BLNO { get; set; }
        public string INAIM { get; set; }
        public string ENVESSELNAME { get; set; }
        public string VOYAGE { get; set; }
        public string TYPE { get; set; }
        public string ISWASTER { get; set; }
        public string ISREEFER { get; set; }
        public string ISDEAL { get; set; }
        public string RESERVED { get; set; }
        public string RESERVEDATE { get; set; }
        public string ISPREPARE { get; set; }
        public string PLANYARDCELL { get; set; }
        public string ISCHECKED { get; set; }

    }
}
