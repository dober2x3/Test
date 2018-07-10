using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class CacheContext : DbContext
    {
        public CacheContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = "Cache.db", ForeignKeys = true }.ConnectionString
            }, true)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cache> Cache { get; set; }
    }

    [Table("Data")]
    public class Cache
    {
        [Key]
        [Column("Search",TypeName = "VARCHAR")]
        public string Search { get; set; }
        [Column("Response", TypeName = "VARCHAR")]
        public string Response { get; set; }
        
    }
}
