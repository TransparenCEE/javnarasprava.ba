
namespace JavnaRasprava.WEB.Models
{
    public class QustionRepresentativeModel
    {
        #region == Fields ==

        #endregion

        #region == Properties ==

        public int ID { get; set; }
       
        public string ImageRelativePath { get; set; }

        public string PartyName { get; set; }

        public string FullName { get; set; }

        public int AskedCount { get; set; }

        public bool Answered { get; set; }

        public AnswerModel Answer { get; set; }

        #endregion

        #region == Constructors ==

        #endregion

        #region == Methods ==

        #endregion
    }
}
