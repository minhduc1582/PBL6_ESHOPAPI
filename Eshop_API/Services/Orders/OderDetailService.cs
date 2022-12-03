using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Helpers;
using eshop_api.Helpers.Mapper;

namespace eshop_api.Services.Orders
{
    public class OderDetailService : IOderDetailService
    {
        private readonly DataContext _context;
        public OderDetailService(DataContext context)
        {
            _context = context;
        }
        public async Task<OrderDetail> AddOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = createUpdateOrderDetail.idOrder;
            orderDetail.ProductId = createUpdateOrderDetail.ProductId;
            orderDetail.Quantity = createUpdateOrderDetail.Quantity;
            orderDetail.Note = createUpdateOrderDetail.Note;
            var result = await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            OderService change = new OderService(_context);
            await change.UpdateTotal(orderDetail.OrderId);
            // var temp = await UpdateTotal(orderDetail.OrderId);
            return result.Entity;
        }
        public async Task<OrderDetail> AddOrderDetail(OrderDetailDTO orderDetailDTO, string idOrder)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = idOrder;
            orderDetail.ProductId = orderDetailDTO.ProductId;
            orderDetail.Quantity = orderDetailDTO.Quantity;
            orderDetail.Note = orderDetailDTO.Note;
            var result = await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteOrderDetail(int idOrderDetail)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                var result = _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
                OderService change = new OderService(_context);
                var temp = await change.UpdateTotal(orderDetail.OrderId);
                return true;
            }
            return false;
        }

        public List<OrderDetail> GetOrderDetailsById(int idOrderDetail)
        {
            var orderDetail = _context.OrderDetails.Where(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                return orderDetail.ToList();
            }
            throw null;
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(string idOrder)
        {
            var orderDetail = _context.OrderDetails.Where(x => x.OrderId == idOrder);
            if(orderDetail != null)
            {
                return orderDetail.ToList();
            }
            throw null;
        }
        public async Task<List<OrderDetailDTOs>> GetOrderDetailByOrderId(string idOrder)
        {
            var orderDetail = _context.OrderDetails.Where(x => x.OrderId == idOrder).ToList();
            if(orderDetail != null)
            {
                List<OrderDetailDTOs> list = new List<OrderDetailDTOs>();
                foreach(OrderDetail i in orderDetail)
                {
                    var product = _context.Products.FirstOrDefault(x => x.Id == i.ProductId);
                    OrderDetailDTOs orderDetailDTOs = OrderDetailMapper.toOrderDetailDto(i, product);
                    list.Add(orderDetailDTOs);
                }
                return await Task.FromResult(list);
            }
            throw null;
        }

        public async Task<OrderDetail> UpdateOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail, int idOrderDetail)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                orderDetail.ProductId = createUpdateOrderDetail.ProductId;
                orderDetail.Quantity = createUpdateOrderDetail.Quantity;
                orderDetail.Note = createUpdateOrderDetail.Note;
                var result =  _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
                OderService change = new OderService(_context);
                var temp = await change.UpdateTotal(orderDetail.OrderId);
                return result.Entity;
            }
            else
            {
                orderDetail = new OrderDetail();
                orderDetail.ProductId = createUpdateOrderDetail.ProductId;
                orderDetail.Quantity = createUpdateOrderDetail.Quantity;
                orderDetail.Note = createUpdateOrderDetail.Note;
                var result = await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
                OderService change = new OderService(_context);
                var temp = await change.UpdateTotal(orderDetail.OrderId);
                return result.Entity;
            }
        }

       
    }
}