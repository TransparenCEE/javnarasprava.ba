using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL
{
	public class AnonymousUserService
	{
		#region == Fields ==

		#endregion

		#region == Constructors ==

		public AnonymousUserService()
		{
			if ( HttpContext.Current.Session["VotedLaws"] == null )
				HttpContext.Current.Session["VotedLaws"] = new Dictionary<int, AnonymousVote>();

			if ( HttpContext.Current.Session["VotedLawSections"] == null )
				HttpContext.Current.Session["VotedLawSections"] = new Dictionary<int, AnonymousVote>();

			if ( HttpContext.Current.Session[ "LikedAnswer" ] == null )
				HttpContext.Current.Session[ "LikedAnswer" ] = new Dictionary<int,bool>();

			if ( HttpContext.Current.Session[ "LikedQuestion" ] == null )
				HttpContext.Current.Session[ "LikedQuestion" ] = new Dictionary<int, bool>();
		}

		#endregion

		#region == Methods ==

		public void VoteLaw( int lawId, int vote, string customVoteText  )
		{
			( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session["VotedLaws"] )
				.Add( lawId, new AnonymousVote { Vote = vote, CustomVote = customVoteText } );
		}

		public void VoteLawSection( int sectionId, int vote, string customVoteText )
		{
			( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session["VotedlawSections"] )
				.Add( sectionId, new AnonymousVote { Vote = vote, CustomVote = customVoteText } ); ;
		}

		public bool HasVotedLaw( int lawId )
		{
			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session["VotedLaws"] ).ContainsKey( lawId );
		}

		public bool HasVotedLawSection( int sectionId )
		{
			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session["VotedlawSections"] ).ContainsKey( sectionId );
		}

		internal int? GetUserLawVote( int id )
		{
			if ( !HasVotedLaw( id ) )
				return null;

			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedLaws" ] )[ id ].Vote;
		}

		internal int? GetUserLawSectionVote( int id )
		{
			if ( !HasVotedLawSection( id ) )
				return null;

			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedlawSections" ] )[ id ].Vote;
		}

		internal bool? GetUserLawVoteBool( int id )
		{
			if ( !HasVotedLaw( id ) )
				return null;

			var intValue = ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedLaws" ] )[ id ].Vote;

			return ConvertIntResultToBoolResult( intValue );
		}

		internal bool? GetUserLawSectionVoteBool( int id )
		{
			if ( !HasVotedLawSection( id ) )
				return null;

			var intValue = ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedlawSections" ] )[ id ].Vote;

			return ConvertIntResultToBoolResult( intValue );
		}

		private static bool? ConvertIntResultToBoolResult( int intValue )
		{
			if ( intValue == -3 )
				return true;
			else if ( intValue == -2 )
				return false;
			return null;
		}

		internal string GetUserLawVoteCustomText( int id )
		{
			if ( !HasVotedLaw( id ) )
				return null;

			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedLaws" ] )[ id ].CustomVote;
		}

		internal string GetUserLawSectionVoteCustomText( int id )
		{
			if ( !HasVotedLawSection( id ) )
				return null;

			return ( (Dictionary<int, AnonymousVote>)HttpContext.Current.Session[ "VotedlawSections" ] )[ id ].CustomVote;
		}

		public void LikeAnswer( int id, bool vote )
		{
			((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedAnswer" ])[ id ] = vote;
		}

		public bool HasLikedAnswer( int id )
		{
			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedAnswer" ]).ContainsKey( id );
		}

		internal bool? GetUserAnswerLike( int id )
		{
			if ( !HasLikedAnswer( id ) )
				return null;

			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedAnswer" ])[ id ];
		}

		public void LikeQuestion( int id, bool vote )
		{
			((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedQuestion" ])[ id ] = vote;
		}

		public bool HasLikedQuestion( int id )
		{
			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedQuestion" ]).ContainsKey( id );
		}

		internal bool? GetUserQuestionLike( int id )
		{
			if ( !HasLikedAnswer( id ) )
				return null;

			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedQuestion" ])[ id ];
		}

		public JavnaRasprava.WEB.DomainModels.ApplicationUser GetAnonymousUser(JavnaRasprava.WEB.DomainModels.ApplicationDbContext context)
		{
			return context.Users.Single( x => x.UserName == "anonymous" );
		}

		internal Dictionary<int, bool> GetUserAnswerLikes()
		{
			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedAnswer" ]);
		}

		internal Dictionary<int, bool> GetUserQuestionLikes()
		{
			return ((Dictionary<int, bool>)HttpContext.Current.Session[ "LikedQuestion" ]);
		}


		private class AnonymousVote
		{
			public int Vote { get; set; }

			public string CustomVote { get; set; }
		}

		

		#endregion
	}
}