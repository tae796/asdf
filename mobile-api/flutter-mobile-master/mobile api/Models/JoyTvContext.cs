using Microsoft.EntityFrameworkCore;

namespace JoyTvApi.Models
{
    public class JoyTvContext : DbContext
    {
        public string DbPath { get; private set; } 
        
        public JoyTvContext(DbContextOptions<JoyTvContext> options)
            // : base(options)
        {
            DbPath = "server=dbwriter.flutterup.net;database=flutter;user=root;password=zzVNIM3Mqw44sUVa;charset=utf8;SslMode=none";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(DbPath, ServerVersion.AutoDetect(DbPath));

	    public DbSet<AgencyTotal.MEMB_MO_INF> MEMB_MO_INF { get; set;}
        public DbSet<AgencyTotal.MEMB_MO_INF2> MEMB_MO_INF2 { get; set;}
        public DbSet<AgencyTotal.MEMB_INF> MEMB_INF { get; set; }
        public DbSet<AgencyTotal.MEMB> MEMB { get; set; }
        public DbSet<AgencyTotal.CAMP_INF> CAMP_INF { get; set; }
        public DbSet<AgencyTotal.BNK_MEMB_INF> BNK_MEMB_INF { get; set; }
        public DbSet<AgencyTotal.BNK_TERM_INF> BNK_TERM_INF { get; set; }
        public DbSet<AgencyTotal.COUP_INF> COUP_INF { get; set; }
        public DbSet<AgencyTotal.COUP_INF2> COUP_INF2 { get; set; }
        public DbSet<AgencyTotal.AUTH_INF> AUTH_INF { get; set; }
        public DbSet<AgencyTotal.EVENT_INF> EVENT_INF { get; set; }
        public DbSet<AgencyTotal.NOTICE_INF> NOTICE_INF { get; set; }
        public DbSet<AgencyTotal.STO_INF> STO_INF { get; set; }
        public DbSet<AgencyTotal.STO_MO_INF> STO_MO_INF { get; set; }
        public DbSet<AgencyTotal.BEACON_INF> BEACON_INF { get; set; }
        public DbSet<AgencyTotal.SINSA_MEMB_INF> SINSA_MEMB_INF { get; set; }
        public DbSet<AgencyTotal.SINSA_HST> SINSA_HST { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgencyTotal.BNK_MEMB_INF>()
                .HasKey(p => new {p.Memb_inf_cd, p.Pnt_bnk_inf_cd});
            modelBuilder.Entity<AgencyTotal.BankStoInfo>()
               .HasKey(p => new { p.Memb_inf_cd, p.Pnt_bnk_inf_cd });
            //.IsUnique(true);
        }


      
       
        
    }
}
