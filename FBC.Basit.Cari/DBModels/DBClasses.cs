using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
namespace FBC.Basit.Cari.DBModels
{


    public class DB : DbContext
    {
        public DbSet<CariKart> CariKart { get; set; }
        public DbSet<CariHareket> CariHareket { get; set; }
        public DbSet<SysUser> Users { get; set; }

        public string DbPath { get; }

        public DB()
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            DbPath = Path.Combine(dir, "basitcari.db");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = Path.Join(path, "basitcari.db");
            //DbPath = ":memory:";

        }

        public static void MigrateDB()
        {
            using (var db = new DB())
            {
                try
                {
                    //if (!File.Exists(db.DbPath))
                    //{
                    //    Console.WriteLine("EnsureCreated");

                    //    db.Database.EnsureCreated();

                    //}
                    //else 
                    
                    //if (db.Database.GetMigrations().Any())

                    //{
                        Console.WriteLine("Begin Migrate");

                        db.Database.Migrate();
                        Console.WriteLine("End Migrate");
                    //} else
                    //{
                    //    Console.WriteLine("The database is already up to date.");
                    //}

                    if (!db.Users.Any(x => x.IsAdmin == true))
                    {
                        Console.WriteLine("AddAdmin");

                        db.Users.Add(new SysUser()
                        {
                            SysUserName = "admin",
                            SysUserPassword = SysUser.ToMD5("admin"),
                            IsAdmin = true,
                            Name = "System",
                            Surname = "Admin",
                            IsCanEditData = false
                        });
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Migrate: " + ex.Message);
                    //Console.WriteLine(ex.StackTrace);
                }
                finally
                {

                }
            }
        }


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
