using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
    public class Party
    {
        public int PartyID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public virtual List<Representative> Representatives { get; set; }
    }
}