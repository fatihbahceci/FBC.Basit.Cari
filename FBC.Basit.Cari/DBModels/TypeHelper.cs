using System.ComponentModel;
using System.Reflection;
using System.Text;

/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
namespace FBC.Basit.Cari.DBModels
{
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
        static string versionInfo = null;
        public static string GetAppVersionInfo()
        {
            if (versionInfo == null)
            {
                StringBuilder versionInfoSB = new StringBuilder("Version: ");
                try
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    versionInfoSB.Append(Assembly.GetExecutingAssembly()
                                                    .GetName()
                                                    .Version
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                                    .ToString());
                }
                catch
                {
                    versionInfoSB.Append("?.?.?.?");
                }
                try
                {
                    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                    versionInfoSB.Append(" Build: " + buildDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                catch
                {
                    versionInfoSB.Append(" Build: " + "UNKNOWN");
                }
                versionInfo = versionInfoSB.ToString();
            }
            return versionInfo;

        }
    }
}
