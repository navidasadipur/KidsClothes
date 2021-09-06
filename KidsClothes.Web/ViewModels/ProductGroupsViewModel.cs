using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KidsClothes.Core.Models;

namespace KidsClothes.Web.ViewModels
{
    public class NewProductGroupViewModel
    {
        public NewProductGroupViewModel()
        {
            BrandIds = new List<int>();
            ProductGroupFeatureIds = new List<int>();
        }

        public string Title { get; set; }
        public List<int> BrandIds { get; set; }
        public int ParentGroupId { get; set; }
        public List<int> ProductGroupFeatureIds { get; set; }
    }
    public class UpdateProductGroupViewModel
    {
        public UpdateProductGroupViewModel()
        {
            BrandIds = new List<int>();
            ProductGroupFeatureIds = new List<int>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> BrandIds { get; set; }
        public int ParentGroupId { get; set; }
        public List<int> ProductGroupFeatureIds { get; set; }
    }

    public class FeaturesObjViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
    public class BrandsObjViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SubFeaturesObjViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            ProductGroups = new List<ProductGroup>();
            Features = new List<Feature>();
            Brands = new List<Brand>();
            Sizes = new List<ProductSize>();
        }

        public int? SelectedGroupId { get; set; }
        public int ProductCount { get; set; }
        public List<ProductGroup> ProductGroups { get; set; }
        public List<Feature> Features { get; set; }
        public List<Brand> Brands { get; set; }
        public List<ProductSize> Sizes { get; set; }
    }
}