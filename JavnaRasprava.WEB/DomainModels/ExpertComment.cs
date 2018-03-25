using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
    public class ExpertComment
    {
        public int ExpertCommentID { get; set; }

        public string Text { get; set; }

        public int LawID { get; set; }

        public virtual Law Law { get; set; }

        public int ExpertID { get; set; }

        public virtual Expert Expert { get; set; }
    }
}