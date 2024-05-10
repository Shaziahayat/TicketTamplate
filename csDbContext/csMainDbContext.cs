using System.Collections.Generic;
using Configuration;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Configuration.AppConfig;
using System.Reflection.Emit;

namespace DbContext
{
    public class csMainDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<TicketInformation> Vy_TicketInformation { get; set; }
        public DbSet<TicketKundInfo> Vy_ticketKundInfo { get; set; }    

        #region Handle the context to use 
        public static DbContextOptionsBuilder<csMainDbContext> DbContextOptions(DbLoginDetail dbConnectionString)
        {
            var _optionsBuilder = new DbContextOptionsBuilder<csMainDbContext>();

            _optionsBuilder.UseSqlServer(dbConnectionString.DbConnectionString,
                        options => options.EnableRetryOnFailure());
            return _optionsBuilder;

            //unknown database type
            throw new InvalidDataException($"Database type  does not exist");
        }
        #endregion
        #region constructors
        public csMainDbContext() { }

        public csMainDbContext(DbContextOptions options) : base(options)
        { }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketInformation>().ToView("Vy_MobileTicket", "dbo").HasNoKey();
            modelBuilder.Entity<TicketKundInfo>().ToView("Vy_BookingInformation", "dbo").HasNoKey();
            base.OnModelCreating(modelBuilder);
        }


        #region DbContext for Sql Server database
        public static csMainDbContext DbContext(string dbConnectionString) => new csMainDbContext(csMainDbContext.DbContextOptions(AppConfig.GetDbLoginDetails(dbConnectionString)).Options);


        public class SqlServerDbContext : csMainDbContext
        {
            public SqlServerDbContext() { }
            public SqlServerDbContext(DbContextOptions options) : base(options)
            { }

            //Used only for CodeFirst Database Migration and database update commands
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {

                    var connectionString = AppConfig.ConfigurationRoot["DbLogins"];
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("The database connection string is not configured.");
                    }

                    // Retrieve the DbLoginDetail instance using the new AppConfig logic.
                    // You need to provide the appropriate key that matches the DbLogins in your configuration.
                    // This key should be the one used in your user secrets or appsettings.json for the connection string.
                    //var dbLoginKey = "YourDbLoginKey"; // Replace with your actual login key
                    var dbLoginDetail = AppConfig.GetDbLoginDetails(connectionString);

                    //// Use the connection string from the DbLoginDetail instance to configure the DbContext.
                    //var connectionString = dbLoginDetail.DbConnectionString;
                    optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
                }
                base.OnConfiguring(optionsBuilder);
            }

            protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
            {
                configurationBuilder.Properties<decimal>().HaveColumnType("money");
                configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

                base.ConfigureConventions(configurationBuilder);
            }

            #region Add your own modelling based on done migrations
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

                modelBuilder.Entity<TicketInformation>()
                    .Property(t => t.Pris)
                     .HasColumnType("decimal(18,2)");
                base.OnModelCreating(modelBuilder);
            }
            #endregion

        }
        #endregion
    }
}