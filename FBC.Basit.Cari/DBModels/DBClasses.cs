using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
namespace FBC.Basit.Cari.DBModels
{
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
    }
    

    public class DB : DbContext
    {
        public DbSet<CariKart> CariKart { get; set; }
        public DbSet<CariHareket> CariHareket { get; set; }

        public string DbPath { get; }

        public DB()
        {

            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = Path.Join(path, "basitcari.db");
            //DbPath = ":memory:";
            DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "basitcari.db");
        }
        static DB()
        {
            try
            {
                using (var db = new DB())
                {
                    db.Database.Migrate();
                }
            } catch
            {

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
