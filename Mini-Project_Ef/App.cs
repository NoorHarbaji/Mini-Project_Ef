using Microsoft.EntityFrameworkCore;
using Mini_Project_Ef;
using Mini_Project_Ef.Conversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mini_Project_Ef
{
    public class App
    {
        static CompanyAssetsContext db = new CompanyAssetsContext();
        object value;
        double curren = 0;
        double priceValue = 0;
        string splitCurr;
        public void Run()
        {
            var companyAssets = db.CompanyAssetsS;
            if (companyAssets == null)
            {
                Functions.AddSomeItems();
            }

            RunConvertion();
            Console.WriteLine();
            ShowCompaniesAsset();
            WriteToFile();

            MainMenu();
        }
        private void RunConvertion()
        {
            API_Obj Test;
            String URLString = "https://v6.exchangerate-api.com/v6/aeccd74cdf37986987dc6b47/latest/USD";

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(URLString);
                Test = JsonConvert.DeserializeObject<API_Obj>(json);
            }
            var nameOfProperty = "conversion_rates";
            var propertyInfo = Test.GetType().GetProperty(nameOfProperty);
            value = propertyInfo.GetValue(Test, null);
        }

        private void WriteToFile()
        {
            using (StreamWriter sw = File.CreateText("CurrencyCuntry.txt"))
            {
                sw.WriteLine("sweden SEK");
                sw.WriteLine("china CNY");
                sw.WriteLine("poland PLN");
                sw.WriteLine("denemark DKK");
            }
        }

        private void MainMenu()
        {
            Console.WriteLine("  \nChose one please");
            Console.WriteLine("a) Create new item ");
            Console.WriteLine("b) Delete");
            Console.WriteLine("c) Update");
            ConsoleKey command = Console.ReadKey(true).Key;

            if (command == ConsoleKey.A)
                CreateNewItem();

            if (command == ConsoleKey.B)
                DeleteExistItem();

            if (command == ConsoleKey.C)
                UpdateExistItem();
        }

        private void UpdateExistItem()
        {
            int val;
            while (true)
            {
                Console.WriteLine("Please insert the Id ");
                string itemId = Console.ReadLine();
                var isValid = int.TryParse(itemId, out val);
                if (isValid)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Not valid id");
                }
            }

            Console.WriteLine("Enter new model name");
            string modelName = "";
            while (true)
            {

                Console.Write("Model name: ");
                modelName = Console.ReadLine();
                if (string.IsNullOrEmpty(modelName))
                {
                    Console.WriteLine("You can't leave it empty");
                }
                else
                {
                    break;
                }
            }
            var updateItem = from p in db.CompanyAssetsS
                             where p.AssetId == val
                             select p;
            foreach (var v in updateItem)
                v.ModelName = modelName;

            db.SaveChanges();
            Run();
            Console.ReadLine();
        }

        private void DeleteExistItem()
        {
            int val;
            while (true)
            {
                Console.WriteLine("Please insert the Id ");
                string itemId = Console.ReadLine();
                var isValid = int.TryParse(itemId, out val);
                if (isValid)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("deleted");
                }
            }

            var delete = from p in db.CompanyAssetsS
                         where p.AssetId == val
                         select p;

            db.CompanyAssetsS.RemoveRange(delete);
            db.SaveChanges();
            Console.WriteLine("deleted");
            Console.WriteLine();
            Run();
        }

        private void CreateNewItem()
        {
            Console.WriteLine("\nSpecify what do you want to create");
            Console.WriteLine("a) LapTop");
            Console.WriteLine("b) Phone");
            ConsoleKey command = Console.ReadKey(true).Key;

            if (command == ConsoleKey.A)
                Create("lapTop");

            if (command == ConsoleKey.B)
                Create("phone");
        }

        private void Create(string type)
        {
            string modelName = "";
            while (true)
            {

                Console.Write("Model name: ");
                modelName = Console.ReadLine();
                if (string.IsNullOrEmpty(modelName))
                {
                    Console.WriteLine("You can't leave it empty");
                }
                else
                {
                    break;
                }
            }

            DateTime dt;
            while (true)
            {
                Console.Write("Purschase date in MM-DD-YYYY format: ");
                string purchaseDate = Console.ReadLine();
                var isValidDate = DateTime.TryParse(purchaseDate, out dt);
                if (isValidDate)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{dt} is not a valid date string");
                }
            }

            while (true)
            {
                Console.Write("Price in dollar: ");
                Console.Write("$");
                string price = Console.ReadLine();
                if (double.TryParse(price, out priceValue))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Flektigt formatt på price!");
                }
            }
            Console.Write("Office Location: ");
            string office = Console.ReadLine();
            CompanyAsset item = new CompanyAsset(priceValue, dt, modelName, office, type);
            db.CompanyAssetsS.Add(item);
            db.SaveChanges();
            Console.WriteLine();
            Run();
        }

        private void ShowCompaniesAsset()
        {
            Console.WriteLine();
            List<CompanyAsset> sortedCompanyAsset = db.CompanyAssetsS.
            OrderBy(companyAsset => companyAsset.Office).
               ThenBy(companyAsset => companyAsset.PurchaseDate).ToList();
            Console.WriteLine("Office".PadRight(15) + "Id".PadRight(15) + "Model Name".PadRight(15) + "Price".PadRight(20) + "Purchase Date".PadRight(10));
            Console.WriteLine("===========================================================================================");
            Console.WriteLine();
            foreach (CompanyAsset company in sortedCompanyAsset)
            {
                var s = File.ReadLines("CurrencyCuntry.txt")
                 .SkipWhile(line => !line.Contains(company.Office.ToLower()));
                string[] split = s.First().Split(" ");
                splitCurr = split[1];

                foreach (var cur in value.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any()))
                {
                    if (cur.Name == splitCurr)
                    {
                        curren = (double)cur.GetValue(value, null);
                    }
                }

                TimeSpan diff = expiryDate(company.PurchaseDate);
                if (diff.TotalDays < 90)
                {
                    Console.Write(company.Office.PadRight(15));
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(company.AssetId.ToString().PadRight(15) + company.ModelName.PadRight(15) +
                        (company.Price * curren).ToString("0.##").PadRight(10) +
                        splitCurr.PadRight(10).PadRight(10) + company.PurchaseDate + " ");
                    Console.ResetColor();
                }
                else if (diff.TotalDays < 180)
                {
                    Console.Write(company.Office.PadRight(15));
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(company.AssetId.ToString().PadRight(15) +
                        company.ModelName.PadRight(15) + (company.Price * curren).ToString("0.##").PadRight(10)
                        + splitCurr.PadRight(10) + company.PurchaseDate + " ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(company.Office.PadRight(15) + company.AssetId.ToString().PadRight(15)
                        + company.ModelName.PadRight(15)
                        + (company.Price * curren).ToString("0.##").PadRight(10)
                        + splitCurr.PadRight(10) + company.PurchaseDate + " ");
                }

            }

        }


        public TimeSpan expiryDate(DateTime yearPlustree)
        {
            DateTime dt = DateTime.Now;
            int incresedYear = yearPlustree.Year + 3;
            DateTime newdt = yearPlustree.AddYears(incresedYear - yearPlustree.Year);
            TimeSpan diff = newdt.Subtract(dt); ;
            return diff;
        }
    }
}
class CompanyAssetsContext : DbContext
{
    public DbSet<CompanyAsset> CompanyAssetsS { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog =CompanyAsset ; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //This setting overrides the [Key] attribute in Car class
        modelBuilder.Entity<CompanyAsset>(entity => entity.HasKey(e => e.AssetId));
        //This setting overrides the [Table("MyCars"] attribute in Car class
        modelBuilder.Entity<CompanyAsset>(entity => entity.ToTable("Assets"));
    }

}
