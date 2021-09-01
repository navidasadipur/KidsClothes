using KidsClothes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsClothes.Infrastructure.Repositories
{
    public class ProductSizesRepository : BaseRepository<ProductColor, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ProductSizesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public ProductSize GetProductSize(int id)
        {
            return _context.ProductSizes.Where(s => s.Id == id).FirstOrDefault();
        }

        public ProductSize GetFirstSizeByProductId(int productId)
        {
            return _context.ProductSizes.FirstOrDefault(c => c.IsDeleted == false && c.ProductId == productId);
        }
    }
}
