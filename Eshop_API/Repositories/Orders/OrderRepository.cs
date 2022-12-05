using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.Orders
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> UpdateTotal(int idOrder){
             double temp = 0;
            var order = _context.Orders.FirstOrDefault(x=> x.Id == idOrder);
            if(order == null) return false;
            var orderDetail = _context.OrderDetails.Where(x=> x.OrderId == order.Id).ToList();
            if(orderDetail != null)
            {
                foreach(OrderDetail i in orderDetail)
                {
                    var product = _context.Products.FirstOrDefault(x=> x.Id == i.ProductId);
                    temp += i.Quantity * product.Price;
                }
            }
            order.Total = temp;
            var result = _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}