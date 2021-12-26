using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
namespace FBC.Basit.Cari.DBModels
{
    public class SysUser
    {
        [Key]
        public int SysUserId { get; set; }
        public string SysUserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SysUserPassword { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsCanEditData { get; set; }

        [ForeignKey(nameof(CariKart))]
        public int? CariKartId { get; set; }
        public CariKart? CariKart { get; set; }

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
}
