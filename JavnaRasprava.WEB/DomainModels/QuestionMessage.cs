using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
    public class QuestionMessage
    {
        public int QuestionMessageId { get; set; }

        public DateTime MessageCreatedTime { get; set; }

        public DateTime? MessageSentTime { get; set; }

        public string Recipients { get; set; }

        public string Subject { get; set; }

        public int AnswerTokenId { get; set; }
        public AnswerToken AnswerToken { get; set; }
    }
}