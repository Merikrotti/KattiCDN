using KattiCDN.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace KattiCDN.Services
{
    /// <summary>
    /// EF Core basic database setup
    /// </summary>
    public class CdnDatabase : DbContext
    {
        /// <summary>
        /// Database tables
        /// </summary>
        public DbSet<User> users { get; set; }
        public DbSet<UploadedData> uploadeddata { get; set; }
        public DbSet<RefreshToken> refreshtokens { get; set; }

        private readonly IConfiguration Configuration;

        /// <summary>
        /// Get Configuration for database credentials
        /// </summary>
        /// <param name="conf">Dependency injection configuration</param>
        public CdnDatabase(IConfiguration conf) {
            Configuration = conf;
        }

        /// <summary>
        /// Connect to database
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(Configuration.GetConnectionString("PSQL_DB"));
    }
}

/* SQL Command list before running;

create database cdn

create table users (
	id serial PRIMARY_KEY,
	username VARCHAR(50) UNIQUE NOT NULL,
	password VARCHAR(255) NOT NULL
)

create table refreshtokens (
	id serial PRIMARY KEY,
	user_id INT,
	token VARCHAR(255) UNIQUE NOT NULL,
	CONSTRAINT fk_refreshtokens
		FOREIGN KEY(user_id)
			REFERENCES users(id)
);

create table uploadeddata (
	id serial PRIMARY KEY,
	user_id INT,
	username VARCHAR(50) UNIQUE NOT NULL,
	password VARCHAR(50) UNIQUE NOT NULL,
	CONSTRAINT fk_UploadedData
		FOREIGN KEY(user_id)
			REFERENCES users(id)
);
 */