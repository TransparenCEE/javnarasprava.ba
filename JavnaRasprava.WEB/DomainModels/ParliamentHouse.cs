using System.Collections.Generic;

namespace JavnaRasprava.WEB.DomainModels
{
    public class ParliamentHouse
    {
        public int  ParliamentHouseID { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public virtual Parliament Parliament { get; set; }
        
        public int ParliamentID { get; set; }

        public virtual ICollection<Representative> Representatives { get; set; }
    }
}