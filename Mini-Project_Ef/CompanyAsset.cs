using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Ef
{
    class CompanyAsset
    {
        public CompanyAsset(double price, DateTime purchaseDate, string modelName, string office, string type)

        {
            Price = price;
            PurchaseDate = purchaseDate;
            ModelName = modelName;
            Office = office;
            Type = type;
        }

        [Key]
        public int AssetId { set; get; }
        public double Price { set; get; }
        public DateTime PurchaseDate { set; get; }
        public string ModelName { set; get; }
        public string Office { set; get; }
        public string Type { set; get; }

    }
}
