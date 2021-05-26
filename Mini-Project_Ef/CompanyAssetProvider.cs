using System.Linq;

namespace Mini_Project_Ef
{
    internal class CompanyAssetProvider : ICompanyAssetProvider
    {
        private readonly CompanyAssetContext companyAssetContext;
        public CompanyAssetProvider(CompanyAssetContext companyAssetContext)
        {
            this.companyAssetContext = companyAssetContext;
        }
        public CompanyAsset Get(int id)
        {
            return companyAssetContext.CompanyAssets.Where(e => e.AssetId == id).FirstOrDefault();
        }                               
    }
}
