using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
    public class DefaultContext : DbContext
    {
        #region == Properties ==

        public DbSet<Expert> Experts { get; set; }

        public DbSet<ExpertComment> ExpertComments { get; set; }

        public DbSet<Law> Laws { get; set; }

        public DbSet<LawComment> LawComments { get; set; }

        public DbSet<LawCustomVote> LawCustomVotes { get; set; }

        public DbSet<LawSection> LawSections { get; set; }

        public DbSet<LawSectionCustomVote> LawSectionCustomVotes { get; set; }

        public DbSet<LawSectionVote> LawSectionVotes{ get; set; }

        public DbSet<LawVote> LawVotes { get; set; }

        public DbSet<Parliament> Parliaments { get; set; }

        public DbSet<ParliamentHouse> ParliamentHouses { get; set; }

        public DbSet<Party> Parties { get; set; }

        public DbSet<Representative> Representatives { get; set; }

        #endregion

        #region == Constructors ==

        public DefaultContext()
            : base( "DefaultContext" )
        {

        }

        #endregion

        #region == Methods ==

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #endregion
    }
}