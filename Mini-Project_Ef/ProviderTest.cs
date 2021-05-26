using System;

namespace Mini_Project_Ef
{
    class ProviderTest
    {
        public void Test()
        {
            var provider = new CompanyAssetProvider(new CompanyAssetContext("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = CompanyAsset; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"));
            var asset = provider.Get(1);
            Console.WriteLine($"Test Prpvider: {asset.ModelName} {asset.Office}");
        }
    }
}
