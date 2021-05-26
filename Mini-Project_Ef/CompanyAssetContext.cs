using Microsoft.EntityFrameworkCore;

namespace Mini_Project_Ef
{
    internal class CompanyAssetContext : DbContext
    {
        private readonly string connectionString;
        public CompanyAssetContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public DbSet<CompanyAsset> CompanyAssets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
