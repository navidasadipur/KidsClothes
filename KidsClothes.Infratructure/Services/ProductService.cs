using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KidsClothes.Core.Models;
using KidsClothes.Core.Utility;
using KidsClothes.Infrastructure;
using KidsClothes.Infrastructure.Repositories;
using KidsClothes.Infratructure.Dtos.Product;

namespace KidsClothes.Infratructure.Services
{
    public class ProductService
    {
        private readonly InvoicesRepository _InvoiceRepo;
        private readonly ProductsRepository _productRepo;
        private readonly ProductMainFeaturesRepository _productMainFeatureRepo;
        private readonly DiscountsRepository _discountRepo;
        private readonly MyDbContext _context;
        private readonly ProductSizesRepository _productSizesRepo;
        private readonly ProductColorsRepository _productColorsRepo;

        public ProductService(
            ProductsRepository productRepo
            , InvoicesRepository invoiceRepo
            , ProductMainFeaturesRepository productMainFeatureRepo
            , DiscountsRepository discountRepo
            , MyDbContext context
            , ProductSizesRepository productSizesRepo
            , ProductColorsRepository productColorsRepo
            )
        {
            _productRepo = productRepo;
            _InvoiceRepo = invoiceRepo;
            _productMainFeatureRepo = productMainFeatureRepo;
            _discountRepo = discountRepo;
            _context = context;
            _productSizesRepo = productSizesRepo;
            _productColorsRepo = productColorsRepo;
        }

        public List<Product> GetHighRatedProducts(int take)
        {
            var products = _context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.Rate).Take(take).ToList();

            return products;
        }

        public List<Product> GetTopSoldProducts(int take)
        {
            var products = new List<Product>();
            var TopSoldProducts = _InvoiceRepo.GertTopSoldProducts(take);
            if (TopSoldProducts.Any() == false)
                products = _productRepo.GetNewestProducts(take);
            else
            {
                products = TopSoldProducts;
            }

            return products;
        }

        public int GetProductSoldCount(Product product)
        {
            var amount = 0;
            var invoices = _context.InvoiceItems.Where(i => i.ProductId == product.Id && i.IsDeleted == false).ToList();
            if (invoices.Any())
                amount += invoices.Sum(i => i.Quantity);
            return amount;
        }

        public int GetProductStockCount(int productId)
        {
            var inStock = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(productId);
            if (mainFeature != null)
                inStock += mainFeature.Quantity;
            return inStock;
        }
        public int GetProductStockCount(int productId, int mainFeatureId)
        {
            var inStock = 0;
            var mainFeature = _productMainFeatureRepo.Get(mainFeatureId);
            if (mainFeature != null)
                inStock += mainFeature.Quantity;
            return inStock;
        }

        public void DecreaseStockProductCount(int productId, int mainFeatureId, int count)
        {
            var inStock = 0;
            var mainFeature = _productMainFeatureRepo.Get(mainFeatureId);
            if (mainFeature != null)
            {
                mainFeature.Quantity -= count;
                _productMainFeatureRepo.Update(mainFeature);
            }
        }

        public long GetProductPrice(Product product)
        {
            long price = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(product.Id);
            if (mainFeature != null && mainFeature.Quantity > 0)
            {
                price = mainFeature.Price;
            }

            return price;
        }
        public long GetProductPrice(Product product, int mainFeatureId)
        {
            long price = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(product.Id, mainFeatureId);
            if (mainFeature != null && mainFeature.Quantity > 0)
            {
                price = mainFeature.Price;
            }

            return price;
        }
        public long GetProductPriceAfterDiscount(Product product)
        {
            var productPrice = GetProductPrice(product);
            var priceAfterDiscount = productPrice;

            // Checking For Product Discount
            var discount = _discountRepo.GetProductDiscount(product.Id);

            // Checking For ProductGroupDiscount
            if (discount == null)
                discount = _discountRepo.GetProductGroupDiscount(product.ProductGroupId ?? 0);

            // Checking For Brand Discount
            if (discount == null)
                discount = _discountRepo.GetBrandDiscount(product.BrandId ?? 0);

            if (discount != null)
            {
                if (discount.DiscountType == (int)DiscountType.Amount)
                {
                    priceAfterDiscount -= discount.Amount;
                }
                else if (discount.DiscountType == (int)DiscountType.Percentage)
                {
                    var discountAmount = (discount.Amount * productPrice / 100);
                    priceAfterDiscount -= discountAmount;
                }
            }

            return priceAfterDiscount;
        }
        public long GetProductPriceAfterDiscount(Product product, int mainFeatureId)
        {
            var productPrice = GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = productPrice;

            // Checking For Product Discount
            var discount = _discountRepo.GetProductDiscount(product.Id);

            // Checking For ProductGroupDiscount
            if (discount == null)
                discount = _discountRepo.GetProductGroupDiscount(product.ProductGroupId ?? 0);

            // Checking For Brand Discount
            if (discount == null)
                discount = _discountRepo.GetBrandDiscount(product.BrandId ?? 0);

            if (discount != null)
            {
                if (discount.DiscountType == (int)DiscountType.Amount)
                {
                    priceAfterDiscount -= discount.Amount;
                }
                else if (discount.DiscountType == (int)DiscountType.Percentage)
                {
                    var discountAmount = (discount.Amount * productPrice / 100);
                    priceAfterDiscount -= discountAmount;
                }
            }

            return priceAfterDiscount;
        }

        public DiscountInfo GetProductDiscount(Product product)
        {
            DiscountInfo discountInfo = new DiscountInfo();

            var productPrice = GetProductPrice(product);
            var priceAfterDiscount = productPrice;

            // Checking For Product Discount
            var discount = _discountRepo.GetProductDiscount(product.Id);

            // Checking For ProductGroupDiscount
            if (discount == null)
                discount = _discountRepo.GetProductGroupDiscount(product.ProductGroupId ?? 0);

            // Checking For Brand Discount
            if (discount == null)
                discount = _discountRepo.GetBrandDiscount(product.BrandId ?? 0);

            if (discount != null)
            {
                if (discount.DiscountType == (int)DiscountType.Amount)
                {
                    priceAfterDiscount -= discount.Amount;
                    discountInfo.DiscountAmount = discount.Amount;
                    discountInfo.DiscountValue = discount.Amount;
                    discountInfo.DiscountType = DiscountType.Amount;

                }
                else if (discount.DiscountType == (int)DiscountType.Percentage)
                {
                    var discountAmount = (discount.Amount * productPrice / 100);
                    priceAfterDiscount -= discountAmount;
                    discountInfo.DiscountAmount = discount.Amount;
                    discountInfo.DiscountValue = discount.Amount;
                    discountInfo.DiscountType = DiscountType.Percentage;
                }
            }

            return discountInfo;
        }

        public List<ProductWithPriceDto> GetTopSoldProductsWithPrice(int take)
        {
            var productsDto = new List<ProductWithPriceDto>();
            var products = GetTopSoldProducts(take);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var price = GetProductPrice(product);
                var productDiscount = GetProductDiscount(product);
                var productDto = new ProductWithPriceDto()
                {
                    Id = product.Id,
                    ShortTitle = product.ShortTitle,
                    Image = product.Image,
                    Price = price,
                    PriceAfterDiscount = price - productDiscount.DiscountValue,
                    DiscountAmount = productDiscount.DiscountAmount,
                    DiscountType = productDiscount.DiscountType,
                    Rate = product.Rate,
                };
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }

        public List<ProductWithPriceDto> GetHighRatedProductsWithPrice(int take)
        {
            var productsDto = new List<ProductWithPriceDto>();
            var products = GetHighRatedProducts(take);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var price = GetProductPrice(product);
                var productDiscount = GetProductDiscount(product);
                var productDto = new ProductWithPriceDto()
                {
                    Id = product.Id,
                    ShortTitle = product.ShortTitle,
                    Image = product.Image,
                    Price = price,
                    PriceAfterDiscount = price - productDiscount.DiscountValue,
                    DiscountAmount = productDiscount.DiscountAmount,
                    DiscountType = productDiscount.DiscountType,
                    Rate = product.Rate,
                };
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }

        public List<ProductWithPriceDto> GetRelatedProducts(int productId, int take)
        {
            var productt = _productRepo.Get(productId);
            var relatedProducts = _context.Products
                .Where(p => p.ProductGroupId == productt.ProductGroupId && p.IsDeleted == false && p.Id != productId)
                .Take(take).ToList();
            var productsDto = new List<ProductWithPriceDto>();

            #region Getting Product Price And Discount

            foreach (var product in relatedProducts)
            {
                var price = GetProductPrice(product);
                var priceAfterDiscount = GetProductPriceAfterDiscount(product);
                var productDto = new ProductWithPriceDto()
                {
                    Id = product.Id,
                    ShortTitle = product.ShortTitle,
                    Image = product.Image,
                    Price = price,
                    PriceAfterDiscount = priceAfterDiscount,
                    Rate = product.Rate,
                };
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }

        public List<ProductWithPriceDto> GetComplementaryProducts(int productId, int take)
        {
            List<Product> products = (from p in _context.Products
                                      join s in _context.SimilarProducts
                                      on p.Id equals s.SimilarProductId
                                      where s.ProductId == productId
                                      select p).ToList();

            var productsDto = new List<ProductWithPriceDto>();

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var price = GetProductPrice(product);
                var priceAfterDiscount = GetProductPriceAfterDiscount(product);
                var productDto = new ProductWithPriceDto()
                {
                    Id = product.Id,
                    ShortTitle = product.ShortTitle,
                    Image = product.Image,
                    Price = price,
                    PriceAfterDiscount = priceAfterDiscount,
                    Rate = product.Rate,
                };
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;

        }

        public List<ProductWithPriceDto> GetLatestProductsWithPrice(int take)
        {
            var productsDto = new List<ProductWithPriceDto>();
            var products = _productRepo.GetNewestProducts(take);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var price = GetProductPrice(product);
                var productDiscount = GetProductDiscount(product);
                var productDto = new ProductWithPriceDto()
                {
                    Id = product.Id,
                    ShortTitle = product.ShortTitle,
                    Image = product.Image,
                    Price = price,
                    PriceAfterDiscount = price - productDiscount.DiscountValue,
                    DiscountAmount = productDiscount.DiscountAmount,
                    DiscountType = productDiscount.DiscountType,
                    Rate = product.Rate,
                };
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public ProductWithPriceDto CreateProductWithPriceDto(int productId)
        {
            var product = _productRepo.Get(productId);
            var price = GetProductPrice(product);
            //var priceAfterDiscount = GetProductPriceAfterDiscount(product);
            var productDiscount = GetProductDiscount(product);
            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortTitle,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = price - productDiscount.DiscountValue,
                DiscountAmount = productDiscount.DiscountAmount,
                DiscountType = productDiscount.DiscountType,
                Rate = product.Rate,
            };
            return productDto;
        }
        public ProductWithPriceDto CreateProductWithPriceDto(int productId, int mainFeatureId)
        {
            var product = _productRepo.Get(productId);
            var price = GetProductPrice(product, mainFeatureId);
            //var priceAfterDiscount = GetProductPriceAfterDiscount(product,mainFeatureId);
            var productDiscount = GetProductDiscount(product);
            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortTitle,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = price - productDiscount.DiscountValue,
                DiscountAmount = productDiscount.DiscountAmount,
                DiscountType = productDiscount.DiscountType,
                Rate = product.Rate,
            };
            return productDto;
        }
        public ProductWithPriceDto CreateProductWithPriceDto(Product product)
        {
            var price = GetProductPrice(product);
            //var priceAfterDiscount = GetProductPriceAfterDiscount(product);
            var productDiscount = GetProductDiscount(product);
            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortTitle,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = price - productDiscount.DiscountValue,
                DiscountAmount = productDiscount.DiscountAmount,
                DiscountType = productDiscount.DiscountType,
                Rate = product.Rate,
            };
            return productDto;
        }
        public ProductWithPriceDto CreateProductWithPriceDto(Product product, int mainFeatureId)
        {
            var price = GetProductPrice(product, mainFeatureId);
            //var priceAfterDiscount = GetProductPriceAfterDiscount(product, mainFeatureId);
            var productDiscount = GetProductDiscount(product);
            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortTitle,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = price - productDiscount.DiscountValue,
                DiscountAmount = productDiscount.DiscountAmount,
                DiscountType = productDiscount.DiscountType,
                Rate = product.Rate,
            };
            return productDto;
        }
        public List<int> GetAllChildrenProductGroupIds(int id)
        {
            var ids = new List<int>();
            ids.AddRange(_context.ProductGroups.Where(p => p.IsDeleted == false && p.ParentId == id).Select(p => p.Id).ToList());
            foreach (var item in ids.ToList())
            {
                var childIds = GetAllChildrenProductGroupIds(item);
                if (childIds.Any())
                {
                    ids.AddRange(childIds);
                }
            }
            return ids;
        }
        #region Get Products Grid

        public List<Product> GetProductsGrid(List<int> groupIds = null, List<int> brandIds = null, List<int> sizeIds = null, List<int> colorIds = null, List<int> subFeatureIds = null, long? fromPrice = null, long? toPrice = null, string searchString = null)
        {
            var allProducts = new List<Product>();
            var allFilteredProducts = new List<Product>();

            var productsFilteredByGroup = new List<Product>();
            var productsFilteredByBrand = new List<Product>();
            var productsFilteredBySize = new List<Product>();
            var productsFilteredByColor = new List<Product>();
            var productsFilteredByFeature = new List<Product>();

            //if searched
            if (!string.IsNullOrEmpty(searchString))
            {
                allProducts = _context.Products
                    .Where(p => p.IsDeleted == false
                    && (p.ShortTitle.Trim().ToLower().Contains(searchString.Trim().ToLower())
                    || p.Title.Trim().ToLower().Contains(searchString.Trim().ToLower())
                    || searchString.Trim().ToLower().Contains(p.ShortTitle.Trim().ToLower())
                    || searchString.Trim().ToLower().Contains(p.Title.Trim().ToLower())
                    )
                    )
                    .Include(p => p.ProductMainFeatures)
                    .Include(p => p.ProductFeatureValues)
                    .Include(p => p.ProductGroup)
                    .Include(p => p.ProductSizes)
                    .Include(p => p.ProductColors)
                    .OrderByDescending(p => p.InsertDate).ToList();

                if ((groupIds != null && groupIds.Any())
                    || (brandIds != null && brandIds.Any())
                    || (sizeIds != null && sizeIds.Any())
                    || (colorIds != null && colorIds.Any())
                    || (subFeatureIds != null && subFeatureIds.Any(f => f != 0)))
                {
                    if (groupIds != null && groupIds.Any())
                    {
                        foreach (var groupId in groupIds)
                            productsFilteredByGroup.AddRange(allProducts.Where(p => p.IsDeleted == false && p.ProductGroupId == groupId).OrderByDescending(p => p.InsertDate));
                    }
                    if (brandIds != null && brandIds.Any())
                    {
                        foreach (var brandId in brandIds)
                            productsFilteredByBrand.AddRange(allProducts.Where(p => p.IsDeleted == false && p.BrandId == brandId).OrderByDescending(p => p.InsertDate));
                    }
                    if (sizeIds != null && sizeIds.Any())
                    {
                        foreach (var sizeId in sizeIds)
                        {
                            var size = _productSizesRepo.GetProductSize(sizeId);
                            productsFilteredBySize.AddRange(allProducts.Where(p => p.IsDeleted == false && p.Id == size.ProductId).OrderByDescending(p => p.InsertDate));
                        }
                    }
                    if (colorIds != null && colorIds.Any())
                    {
                        foreach (var colorId in colorIds)
                        {
                            var color = _productColorsRepo.GetProductColor(colorId);
                            productsFilteredByColor.AddRange(allProducts.Where(p => p.IsDeleted == false && p.Id == color.ProductId).OrderByDescending(p => p.InsertDate));
                        }
                    }
                    if (subFeatureIds != null && subFeatureIds.Any(f => f != 0))
                    {
                        foreach (var subFeature in subFeatureIds.Where(f => f != 0))
                            productsFilteredByFeature.AddRange(
                                allProducts.Where(p => p.ProductFeatureValues.Any(pf => pf.SubFeatureId == subFeature) || p.ProductMainFeatures.Any(pf => pf.SubFeatureId == subFeature))
                                .OrderByDescending(p => p.InsertDate).ToList());
                    }

                    allFilteredProducts = allFilteredProducts
                        .Union(productsFilteredByGroup)
                        .Union(productsFilteredByBrand)
                        .Union(productsFilteredBySize)
                        .Union(productsFilteredByColor)
                        .Union(productsFilteredByFeature).ToList();
                }
                else
                {
                    allFilteredProducts = allProducts;
                }
            }
            else
            {
                allProducts = _context.Products
                    .Include(p => p.ProductMainFeatures)
                    .Include(p => p.ProductFeatureValues)
                    .Include(p => p.ProductGroup)
                    .Include(p => p.ProductSizes)
                    .Include(p => p.ProductColors)
                    .Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate).ToList();


                if ((groupIds != null && groupIds.Any())
                    || (brandIds != null && brandIds.Any())
                    || (sizeIds != null && sizeIds.Any())
                    || (colorIds != null && colorIds.Any())
                    || (subFeatureIds != null && subFeatureIds.Any(f => f != 0)))
                {
                    if (groupIds != null && groupIds.Any())
                    {
                        foreach (var group in groupIds)
                            productsFilteredByGroup.AddRange(allProducts.Where(p => p.IsDeleted == false && p.ProductGroupId == group).OrderByDescending(p => p.InsertDate));
                    }
                    if (brandIds != null && brandIds.Any())
                    {
                        foreach (var brand in brandIds)
                            productsFilteredByBrand.AddRange(allProducts.Where(p => p.IsDeleted == false && p.BrandId == brand).OrderByDescending(p => p.InsertDate));
                    }
                    if (sizeIds != null && sizeIds.Any())
                    {
                        foreach (var sizeId in sizeIds)
                        {
                            var size = _productSizesRepo.GetProductSize(sizeId);
                            productsFilteredBySize.AddRange(allProducts.Where(p => p.IsDeleted == false && p.Id == size.ProductId).OrderByDescending(p => p.InsertDate));
                        }
                    }
                    if (colorIds != null && colorIds.Any())
                    {
                        foreach (var colorId in colorIds)
                        {
                            var color = _productColorsRepo.GetProductColor(colorId);
                            productsFilteredByColor.AddRange(allProducts.Where(p => p.IsDeleted == false && p.Id == color.ProductId).OrderByDescending(p => p.InsertDate));
                        }
                    }
                    if (subFeatureIds != null && subFeatureIds.Any(f => f != 0))
                    {
                        foreach (var subFeature in subFeatureIds.Where(f => f != 0))
                            productsFilteredByFeature.AddRange(allProducts.Where(p => p.ProductFeatureValues.Any(pf => pf.SubFeatureId == subFeature) || p.ProductMainFeatures.Any(pf => pf.SubFeatureId == subFeature)).OrderByDescending(p => p.InsertDate));
                    }

                    allFilteredProducts = allFilteredProducts
                        .Union(productsFilteredByGroup)
                        .Union(productsFilteredByBrand)
                        .Union(productsFilteredBySize)
                        .Union(productsFilteredByColor)
                        .Union(productsFilteredByFeature).ToList();
                }
                else
                {
                    allFilteredProducts = allProducts;
                }
            }

                if (fromPrice != null)
                    allFilteredProducts = allFilteredProducts.Where(p => GetProductPriceAfterDiscount(p) >= fromPrice).ToList();

                if (toPrice != null)
                    allFilteredProducts = allFilteredProducts.Where(p => GetProductPriceAfterDiscount(p) <= toPrice).ToList();

                foreach (var product in allFilteredProducts)
                {
                    product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
                }

                return allFilteredProducts;

                //    if (productGroupId == null || productGroupId == 0)
                //{
                //    if (string.IsNullOrEmpty(searchString))
                //    {
                //        products = _context.Products.Include(p => p.ProductMainFeatures).Include(p => p.ProductFeatureValues).Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate).ToList();

                //        foreach (var product in products)
                //        {
                //            product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
                //        }
                //    }
                //    else
                //    {
                //        products = _context.Products.Include(p => p.ProductMainFeatures)
                //            .Include(p => p.ProductFeatureValues)
                //            .Where(p => p.IsDeleted == false && (p.ShortTitle.Trim().ToLower().Contains(searchString.Trim().ToLower()) || p.Title.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                //            .OrderByDescending(p => p.InsertDate).ToList();

                //        foreach (var product in products)
                //        {
                //            product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
                //        }
                //    }
                //}
                //else
                //{
                //    products = _context.Products.Include(p=>p.ProductMainFeatures).Include(p=>p.ProductFeatureValues).Where(p => p.IsDeleted == false && p.ProductGroupId == productGroupId).OrderByDescending(p => p.InsertDate).ToList();

                //    foreach (var product in products)
                //    {
                //        product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
                //    }

                //    var allChildrenGroups = GetAllChildrenProductGroupIds(productGroupId.Value);
                //    foreach (var groupId in allChildrenGroups)
                //        products.AddRange(_context.Products.Where(p => p.IsDeleted == false && p.ProductGroupId == groupId).OrderByDescending(p => p.InsertDate).ToList());
                //    if (string.IsNullOrEmpty(searchString) == false)
                //    {
                //        products = products
                //            .Where(p => p.IsDeleted == false && (p.ShortTitle.Trim().ToLower().Contains(searchString.Trim().ToLower()) || p.Title.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                //            .OrderByDescending(p => p.InsertDate).ToList();

                //        foreach (var product in products)
                //        {
                //            product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
                //        }
                //    }
                //}
            }
            #endregion
        }


        public class DiscountInfo
        {
            public long DiscountAmount { get; set; }
            public DiscountType DiscountType { get; set; }
            public long DiscountValue { get; set; }
        }
    }
