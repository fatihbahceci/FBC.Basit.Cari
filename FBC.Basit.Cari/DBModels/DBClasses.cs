using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
namespace FBC.Basit.Cari.DBModels
{
    public class SysUser
    {
        public int SysUserId { get; set; }
        public string SysUserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SysUserPassword { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsCanEditData { get; set; }
        public int? CariKartId { get; set; }

        /// <summary>
        /// SysUserPassword'a dokunmaz. Onu ayrıca set etmeniz gerekir. 
        /// </summary>
        /// <param name="user"></param>
        public void Fill(SysUser user)
        {
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.SysUserName = user.SysUserName;
            this.IsAdmin = user.IsAdmin;
            this.IsCanEditData = user.IsCanEditData;
            this.CariKartId = user.CariKartId;
        }

        public void SetNewPassword(string newPassword)
        {
            SysUserPassword = SysUser.ToMD5(newPassword);
        }

        public static string ToMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }


    public class CariKart
    {
        public int CariKartId { get; set; }

        public string Isim { get; set; }
        public List<CariHareket> Hareketler { get; set; }
    }

    public class CariHareket
    {
        public int CariHareketId { get; set; }
        public DateTime? Tarih { get; set; }
        public string? Aciklama { get; set; }
        public string? EvrakNo { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public DateTime? VadeTarihi { get; set; }

        public int CariKartId { get; set; }
        public CariKart CariKart { get; set; }


        public void Fill(CariHareket h)
        {
            //public int CariHareketId { get; set; }
            //public int CariKartId { get; set; }
            Tarih = h.Tarih;
            Aciklama = h.Aciklama;
            EvrakNo = h.EvrakNo;
            Borc = h.Borc;
            Alacak = h.Alacak;
            VadeTarihi = h.VadeTarihi;
        }
    }


    public class DB : DbContext
    {
        public DbSet<CariKart> CariKart { get; set; }
        public DbSet<CariHareket> CariHareket { get; set; }
        public DbSet<SysUser> Users { get; set; }

        public string DbPath { get; }

        public DB()
        {

            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = Path.Join(path, "basitcari.db");
            //DbPath = ":memory:";
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            DbPath = Path.Combine(dir, "basitcari.db");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        static DB()
        {
            try
            {
                using (var db = new DB())
                {
                    db.Database.Migrate();
                    if (!db.Users.Any(x => x.IsAdmin == true))
                    {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
    public static class TypeHelper
    {
        public static string GetEnumDescription(this Enum? enumObj)
        {
            if (enumObj == null)
            {
                return "Null";
            }
            else
            {
                FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

                object[] attribArray = fieldInfo.GetCustomAttributes(false);

                if (attribArray.Length == 0)
                {
                    return enumObj.ToString();
                }
                else
                {
                    DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                    return attrib.Description;
                }
            }
        }

    }
}
