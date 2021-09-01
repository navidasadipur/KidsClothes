using KidsClothes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsClothes.Infrastructure.Repositories
{
    public class ProductColorsRepository : BaseRepository<ProductColor, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ProductColorsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public ProductColor GetProductColor(int id)
        {
            return _context.ProductColors.Where(c => c.Id == id).FirstOrDefault();
        }

        public ProductColor GetFirstColorByProductId(int productId)
        {
            return _context.ProductColors.FirstOrDefault(c => c.IsDeleted == false && c.ProductId == productId);
        }
    }
}
