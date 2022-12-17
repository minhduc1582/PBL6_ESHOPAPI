using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Helpers;
using eshop_api.Helpers.Mapper;
using Eshop_API.Repositories.Orders;
using eshop_api.Service.Products;
using Eshop_API.Repositories.Products;

namespace eshop_api.Services.Orders
{
    public class OderDetailService : IOderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public OderDetailService(IOrderDetailRepository orderDetailRepository,
                                IOrderRepository orderRepository,
                                IProductRepository productRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        public async Task<OrderDetail> AddOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = createUpdateOrderDetail.idOrder;
            orderDetail.ProductId = createUpdateOrderDetail.ProductId;
            orderDetail.Quantity = createUpdateOrderDetail.Quantity;
            orderDetail.Note = createUpdateOrderDetail.Note;
            var result = await _orderDetailRepository.Add(orderDetail);
            await _orderDetailRepository.SaveChangesAsync();
            await _orderRepository.UpdateTotal(orderDetail.OrderId);
            // var temp = await UpdateTotal(orderDetail.OrderId);
            return result;
        }
        public async Task<OrderDetail> AddOrderDetail(OrderDetailDTO orderDetailDTO, Guid idOrder)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = idOrder;
            orderDetail.ProductId = orderDetailDTO.ProductId;
            orderDetail.Quantity = orderDetailDTO.Quantity;
            orderDetail.Note = orderDetailDTO.Note;
            var result = await _orderDetailRepository.Add(orderDetail);
            await _orderDetailRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteOrderDetail(int idOrderDetail)
        {
            var orderDetail = await _orderDetailRepository.FirstOrDefault(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                var result = _orderDetailRepository.Remove(orderDetail);
                await _orderDetailRepository.SaveChangesAsync();
                var temp = await _orderRepository.UpdateTotal(orderDetail.OrderId);
                return true;
            }
            return false;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsById(int idOrderDetail)
        {
            var orderDetail = await _orderDetailRepository.Find(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                return orderDetail.ToList();
            }
            throw null;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(Guid idOrder)
        {
            var orderDetail =await _orderDetailRepository.Find(x => x.OrderId == idOrder);
            if(orderDetail != null)
            {
                return orderDetail.ToList();
            }
            throw null;
        }
        public async Task<List<OrderDetailDTOs>> GetOrderDetailByOrderId(Guid idOrder)
        {
            var orderDetail = await _orderDetailRepository.Find(x => x.OrderId == idOrder);
            if(orderDetail != null)
            {
                List<OrderDetailDTOs> list = new List<OrderDetailDTOs>();
                foreach(OrderDetail i in orderDetail)
                {
                    var product = await _productRepository.FirstOrDefault(x => x.Id == i.ProductId);
                    OrderDetailDTOs orderDetailDTOs = OrderDetailMapper.toOrderDetailDto(i, product);
                    list.Add(orderDetailDTOs);
                }
                return await Task.FromResult(list);
            }
            throw null;
        }

        public async Task<OrderDetail> UpdateOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail, int idOrderDetail)
        {
            var orderDetail = await _orderDetailRepository.FirstOrDefault(x => x.Id == idOrderDetail);
            if(orderDetail != null)
            {
                orderDetail.ProductId = createUpdateOrderDetail.ProductId;
                orderDetail.Quantity = createUpdateOrderDetail.Quantity;
                orderDetail.Note = createUpdateOrderDetail.Note;
                var result = await _orderDetailRepository.Update(orderDetail);
                await _orderDetailRepository.SaveChangesAsync();
                var temp = await _orderRepository.UpdateTotal(orderDetail.OrderId);
                return result;
            }
            else
            {
                orderDetail = new OrderDetail();
                orderDetail.ProductId = createUpdateOrderDetail.ProductId;
                orderDetail.Quantity = createUpdateOrderDetail.Quantity;
                orderDetail.Note = createUpdateOrderDetail.Note;
                var result = await _orderDetailRepository.Add(orderDetail);
                await _orderDetailRepository.SaveChangesAsync();
                var temp = await _orderRepository.UpdateTotal(orderDetail.OrderId);
                return result;
            }
        }
    }
}