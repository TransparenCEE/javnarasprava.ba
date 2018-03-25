using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.QuestionMessage
{
    public class QuestionMessageModel
    {
        public int RepresentativeId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        public string TrimmedQuestionText
        {
            get
            {
                int count = QuestionText.Length > 200 ? 200 : QuestionText.Length;
                return QuestionText.Substring( 0, count ) + ( QuestionText.Length > 200 ? "..." : "" );
            }
        }

        public int AskedCount { get; set; }

        public int MessagesSentCount { get; set; }

        public bool Answered { get; set; }

        public bool QuestionVerified { get; set; }

        public int? LawId { get; set; }

        public string LawTitle { get; set; }
        public bool IsSuggested { get; internal set; }
    }
}