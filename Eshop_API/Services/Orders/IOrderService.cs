using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using Eshop_API.Helpers.Order;

namespace eshop_api.Services.Orders
{
    public interface IOrderService
    {
        Task<List<OrderView>> GetListOrders(); //Lấy danh sách đơn hàng
        Task<List<OrderView>> GetOrderById(Guid idOrder); //Lấy đơn hàng theo id
        Task<OrderView> GetCart(string username); // Lấy giỏ hàng của người dùng
        Task<List<OrderView>> GetOrdersByUserId(int userId); //Lấy đơn hàng theo user id
        Task<List<OrderView>> GetOrdersByStatus(int status); // Lấy đơn hàng theo trạng thái
        Task<List <OrderView>> GetOrderByStatusOfEachUser(string username, int status); // Lấy đơn hàng theo trạng thái của mỗi người dùng
        Task<OrderDto> AddOrder(List<OrderDetailDTO> orderDetailDTOs, string username, int idAddress, PaymentMethod payment, int time,string ipaddr); // Thêm đơn hàng
        Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, Guid idOrder); //Sửa đơn hàng
        Task<Order> ChangeStatus(Guid idOrder, int status); // đổi trạng thái
        Task<bool> DeleteOrderById(Guid idOrder); //Xóa đơn hàng theo id
        Task<bool> UpdateTotal(Guid idOrder); //Cập nhật tổng tiền giỏ hàng
        Task<Order> AddToCart(OrderDetailDTO orderDetailDTOs, string username); //Thêm vào giỏ hàng
        Task<Order> DelFromCart(int idProduct, string username, int quantity); //Xóa khỏi giỏ hàng
        

    }
}