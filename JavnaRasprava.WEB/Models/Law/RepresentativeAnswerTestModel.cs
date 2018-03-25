using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
    public class RepresentativeAnswerTestModel
    {
        public int LawId { get; set; }
        public int QuestionId { get; set; }

        public int RepresentativeId { get; set; }

        public string Answer { get; set; }
    }
}