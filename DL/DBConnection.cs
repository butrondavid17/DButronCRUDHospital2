namespace DL
{
    public class DBConnection
    {
        public static string GetConnection()
        {
            return "Server=.; Database= DButronHospital; Trusted_Connection=True; User ID=sa; Password=pass@word1; TrustServerCertificate=true;";
        }
    }
}