using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;

namespace eshop_api.Services.Orders
{
    public interface IOrderService
    {
        List<Order> GetListOrders();
        Task<List<OrderView>> GetOrderById(int idOrder);
        Task<OrderView> GetCart(string username);
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetOrdersByStatus(int status);
        List <Order> GetOrderByStatusOfEachUser(int userId, int status);
        Task<Order> AddOrder(List<OrderDetailDTO> orderDetailDTOs, string username);
        Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, int idOrder);
        Task<Order> ChangeStatus(int idOrder, int status);
        Task<bool> DeleteOrderById(int idOrder);
        Task<bool> UpdateTotal(int idOrder);
        Task<Order> AddToCart(OrderDetailDTO orderDetailDTOs, string username);
        Task<Order> DelFromCart(int idProduct, string username);
        

    }
}