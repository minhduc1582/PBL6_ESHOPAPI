using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
namespace eshop_api.Services.Orders
{
    public interface IOderDetailService
    {
        List<OrderDetail> GetOrderDetailsById(int idOrderDetail);
        List<OrderDetail> GetOrderDetailsByOrderId(int idOrder);
        Task<List<OrderDetailDTOs>> GetOrderDetailByOrderId(int idOrder);
        Task<OrderDetail> AddOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail);
        Task<OrderDetail> AddOrderDetail(OrderDetailDTO orderDetailDTO, int idOrder);
        Task<OrderDetail> UpdateOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail, int idOrderDetail);
        Task<bool> DeleteOrderDetail (int idOrderDetail);
    }
}