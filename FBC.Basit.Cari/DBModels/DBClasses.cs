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
        public DbSet<DovizKuru> DovizKuru { get; set; }
        public DbSet<DovizKuruHareket> DovizKuruHareket { get; set; }

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
                            IsCanEditData = true
                        });
                        db.SaveChanges();
                    }
                    if (!db.DovizKuru.Any())
                    {
                        Console.WriteLine("AddDovizKuru");
                        var tarih = new DateTime(1982, 09, 06);
                        var dovizler = new List<DovizKuru>
                        {
                            //new DovizKuru { DovizCinsi = "TRY", DovizAdi = "Türk Lirası", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "USD", DovizAdi = "Amerikan Doları", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "EUR", DovizAdi = "Euro", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "XAU", DovizAdi = "Gram Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "GBP", DovizAdi = "İngiliz Sterlini", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "CHF", DovizAdi = "İsviçre Frangı", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "JPY", DovizAdi = "Japon Yeni", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "AUD", DovizAdi = "Avustralya Doları", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "CAD", DovizAdi = "Kanada Doları", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "DKK", DovizAdi = "Danimarka Kronu", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "NOK", DovizAdi = "Norveç Kronu", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "SEK", DovizAdi = "İsveç Kronu", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "RUB", DovizAdi = "Rus Rublesi", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
/*
 Gram Altın
Cumhuriyet Altını
Tam Altın
Yarım Altın
Çeyrek Altın
Reşat Altın
22 Ayar
18 Ayar
14 Ayar
Gremse Altın
Hamit Altın
Gümüş
Platin
 */
                            new DovizKuru { DovizCinsi = "M_CUMHURIYET_ALTINI", DovizAdi = "Cumhuriyet Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_TAM_ALTIN", DovizAdi = "Tam Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_YARIM_ALTIN", DovizAdi = "Yarım Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_CEYREK_ALTIN", DovizAdi = "Çeyrek Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_RESAT_ALTINI", DovizAdi = "Reşat Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_KULPLU_RESAT_ALTINI", DovizAdi = "Kulplu Reşat Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_22_AYAR_ALTIN", DovizAdi = "22 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_18_AYAR_ALTIN", DovizAdi = "18 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_14_AYAR_ALTIN", DovizAdi = "14 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_GREMSE_ALTIN", DovizAdi = "Gremse Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_ATA_ALTIN", DovizAdi = "Ata Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_HAS_ALTIN", DovizAdi = "Has Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_HAMIT_ALTIN", DovizAdi = "Hamit Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_GUMUS", DovizAdi = "Gümüş", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
                            new DovizKuru { DovizCinsi = "M_PLATIN", DovizAdi = "Platin", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih }
                        };
                        db.DovizKuru.AddRange(dovizler);
                        db.SaveChanges();
                        dovizler = db.DovizKuru.ToList();
                        db.DovizKuruHareket.AddRange(dovizler.Select(x => new DovizKuruHareket
                        {
                            Tarih = x.GuncellemeTarihi,
                            Alis = x.GuncelKurAlis,
                            Satis = x.GuncelKurSatis,
                            Kaynak = "İlk Kur",
                            DovizKuruId = x.DovizKuruId
                        }));
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DovizKuru>()
                .HasIndex(d => d.DovizCinsi)
                .IsUnique();

            modelBuilder.Entity<DovizKuruHareket>()
                .HasIndex(d => new { d.Tarih, d.DovizKuruId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public string SaveChangesAndBackup()
        {
            SaveChanges();

            ExecuteCheckpointAndClose();
            return CreateBackup();
        }
        private void ExecuteCheckpointAndClose()
        {
            using (var connection = Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    // Set the database to single user mode
                    //Sanıyorum ki .db-shm .db-val dosyalarını bu siliyor.
                    command.CommandText = "PRAGMA journal_mode = DELETE;";
                    command.ExecuteNonQuery();

                    // Perform a checkpoint to ensure all changes are written
                    command.CommandText = "PRAGMA wal_checkpoint(FULL);";
                    command.ExecuteNonQuery();

                    command.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
                    command.ExecuteNonQuery();

                    command.CommandText = "VACUUM;";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            // Ensure all finalizers have run and resources are released
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private string CreateBackup()
        {
            string backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseBackups");
            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            string backupPath = Path.Combine(backupDir, $"basitcari_backup_{DateTime.Now:yyyyMMddHHmmss}.db");
            File.Copy(DbPath, backupPath, true);
            return backupPath;
        }
    }
}
