using System.Data.Entity;

namespace BoolTest.DataBase
{
    public partial class ModelKeyBTcontext : DbContext
    {
        public ModelKeyBTcontext()
            : base("name=Model1")
        {
        }

        public virtual DbSet<BillTbl> BillTbls { get; set; }
        public virtual DbSet<Materials> BookTbls { get; set; }
        public virtual DbSet<Jornal> Jornals { get; set; }
        public virtual DbSet<Seria> Serias { get; set; }
        public virtual DbSet<UserTbl> UserTbls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seria>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}
