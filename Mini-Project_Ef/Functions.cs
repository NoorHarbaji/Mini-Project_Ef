using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Ef
{
    class Functions
    {
        static CompanyAssetsContext context = new CompanyAssetsContext();

        public static void ClearDatabase()
        {
            context.RemoveRange(context.CompanyAssetsS);


            context.SaveChanges();
        }



        public static void AddSomeItems()
        {

            CompanyAsset companyAssetA = new CompanyAsset(400, new DateTime(2018, 8, 30), "Iphone", "china", "phone");
            CompanyAsset companyAsset1 = new CompanyAsset(400, new DateTime(2018, 8, 30), "Iphone", "china", "phone");
            
            CompanyAsset companyAsset2 = new CompanyAsset(100, new DateTime(2018, 2, 28), "Samsung", "poland", "phone");
            
            CompanyAsset companyAsset3 = new CompanyAsset(200, new DateTime(2021, 5, 4), "Nokia", "denemark", "phone");
            
            CompanyAsset companyAsset4 = new CompanyAsset(100, new DateTime(2018, 8, 30), "ASUS", "china", "laptop");
           
            CompanyAsset companyAsset5 = new CompanyAsset(100, new DateTime(2018, 2, 28), "MacBook", "poland", "laptop");
           
            CompanyAsset companyAsset6 = new CompanyAsset(100, new DateTime(2021, 5, 4), "Lenovo", "denemark", "laptop");
        
            using (var context = new CompanyAssetsContext())
            {
                context.CompanyAssetsS.AddRange(companyAssetA, companyAsset1);
                context.CompanyAssetsS.AddRange(companyAsset2, companyAsset3);
                context.CompanyAssetsS.AddRange(companyAsset4, companyAsset5);
                context.CompanyAssetsS.Add(companyAsset6);
                context.SaveChanges();
            }
        }
    }
}

