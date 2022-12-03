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
        Task<List<OrderView>> GetOrderById(string idOrder);
        Task<OrderView> GetCart(string username);
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetOrdersByStatus(int status);
        List <Order> GetOrderByStatusOfEachUser(int userId, int status);
        Task<Order> AddOrder(List<OrderDetailDTO> orderDetailDTOs, string username, int idAddress, int payment, int time);
        Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, string idOrder);
        Task<Order> ChangeStatus(string idOrder, int status);
        Task<bool> DeleteOrderById(string idOrder);
        Task<bool> UpdateTotal(string idOrder);
        Task<Order> AddToCart(OrderDetailDTO orderDetailDTOs, string username);
        Task<Order> DelFromCart(int idProduct, string username, int quantity);
        

    }
}