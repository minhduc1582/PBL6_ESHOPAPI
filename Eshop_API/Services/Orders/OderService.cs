using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Helpers;
using eshop_api.Services.Images;
using eshop_pbl6.Helpers.Order;
using Microsoft.AspNetCore.Identity;
using eshop_pbl6.Models.DTO.Identities;
using eshop_pbl6.Services.Addresses;
using Eshop_API.Models.DTO.Adress;

namespace eshop_api.Services.Orders
{
    public class OderService : IOrderService
    {
        private readonly DataContext _context;
        public OderService(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Order> AddOrder(List<OrderDetailDTO> orderDetailDTOs, string username, int idAddress, int payment, int time)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            double temp = 0;
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                var product = _context.Products.FirstOrDefault(x=> x.Id == i.ProductId);
                temp += i.Quantity * product.Price;
            }
            Order order = new Order();
            OderDetailService add = new OderDetailService(_context);
            order.Status = Status.Pending.ToString();
            order.Total = temp;
            order.UserId = userId;
            order.AddressId = idAddress;
            order.PaymentMethod = payment;
            order.DeliveryTime = time;
            CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                await add.AddOrderDetail(i, order.Id);
            }
            return order;
        }

        public async Task<OrderView> GetCart(string username)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            OderDetailService get = new OderDetailService(_context);
            List<Order> orders = GetOrderByStatusOfEachUser(userId, 1).ToList();
            OrderView order = new OrderView();
            if(orders!=null)
            {
                foreach(Order i in orders)
                {
                    List<OrderDetailDTOs> details = await get.GetOrderDetailByOrderId(i.Id);
                    order.Id = i.Id;
                    order.Status = i.Status;
                    order.Total = i.Total;
                    order.Note = i.Note;
                    order.Check = i.Check;
                    order.CheckedAt = i.CheckedAt;
                    order.CheckedBy = i.CheckedBy;
                    order.CheckedComment = i.CheckedComment;
                    order.UserId = i.UserId;
                    order.list = details;
                    return await Task.FromResult(order);
                }
            }
            Order new_order = new Order();
            new_order.Status = Status.Cart.ToString();
            new_order.Total = 0;
            new_order.UserId = userId;
            await _context.Orders.AddAsync(new_order);
            await _context.SaveChangesAsync();
            order.Id = new_order.Id;
            order.Status = new_order.Status;
            order.Total = new_order.Total;
            order.UserId = new_order.UserId;
            return await Task.FromResult(order);
        }

        public async Task<Order> AddToCart(OrderDetailDTO detailDTOs, string username)
        {
            double temp = 0;
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            var product = _context.Products.FirstOrDefault(x=> x.Id == detailDTOs.ProductId);
            temp += detailDTOs.Quantity * product.Price;
            Boolean isExist = false;
            OderDetailService add = new OderDetailService(_context);
            List<Order> orders = GetOrdersByUserId(userId).ToList();
            foreach(Order i in orders)
            {
                if(i.Status == "Cart")
                {
                    var orderDetails = await add.GetOrderDetailByOrderId(i.Id);
                    foreach(OrderDetailDTOs j in orderDetails)
                    {
                        if(j.ProductId == detailDTOs.ProductId)
                        {
                            isExist = true;
                            j.Quantity += detailDTOs.Quantity;
                            CreateUpdateOrderDetail createUpdateOrderDetail = new CreateUpdateOrderDetail();
                            createUpdateOrderDetail.idOrder = j.OrderId;
                            createUpdateOrderDetail.ProductId = j.ProductId;
                            createUpdateOrderDetail.Quantity = j.Quantity;
                            createUpdateOrderDetail.Note = detailDTOs.Note;
                            await add.UpdateOrderDetail(createUpdateOrderDetail, j.Id);
                        }
                    }
                    if(isExist == false)
                    {   
                        await add.AddOrderDetail(detailDTOs, i.Id);
                    }
                    await UpdateTotal(i.Id);
                    var result = _context.Orders.Update(i);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            Order order = new Order();
            order.Status = Status.Cart.ToString();
            order.Total = temp;
            order.UserId = userId;
            CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
            var results = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await add.AddOrderDetail(detailDTOs, order.Id);
            return results.Entity;
        }

        public async Task<Order> ChangeStatus(int idOrder, int status)
        {
            var order = _context.Orders.FirstOrDefault(x=> x.Id == idOrder);
            switch(status)
            {
                case 1:
                    order.Status = Status.Cart.ToString();
                    break;
                case 2:
                    order.Status = Status.Pending.ToString();
                    break;
                case 3:
                    order.Status = Status.Shipping.ToString();
                    break;
                case 4:
                    order.Status = Status.Shipped.ToString();
                    break;
                case 5:
                    order.Status = Status.Cancel.ToString();
                    break;
                default:
                    order.Status = Status.Cart.ToString();
                    break;
            }
            var result = _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteOrderById(int idOrder)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == idOrder);
            if(order != null)
            {
                var result = _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order> DelFromCart(int idProduct, string username, int quantity)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            List<Order> order = GetOrderByStatusOfEachUser(userId, 1).ToList();
            OderDetailService del = new OderDetailService(_context);
            foreach(Order i in order)
            {
                List<OrderDetail> orderDetails = del.GetOrderDetailsByOrderId(i.Id).ToList();
                foreach(OrderDetail j in orderDetails)
                {
                    if(j.ProductId == idProduct)
                    {
                        int temp = j.Quantity - quantity;
                        if (temp == 0)
                            await del.DeleteOrderDetail(j.Id);
                        else
                        {
                            CreateUpdateOrderDetail createUpdateOrderDetail = new CreateUpdateOrderDetail();
                            createUpdateOrderDetail.idOrder = j.OrderId;
                            createUpdateOrderDetail.ProductId = j.ProductId;
                            createUpdateOrderDetail.Quantity = temp;
                            createUpdateOrderDetail.Note = j.Note;
                            await del.UpdateOrderDetail(createUpdateOrderDetail, j.Id);
                        }
                    }
                }
                await UpdateTotal(i.Id);
                var result = _context.Orders.Update(i);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            throw new NotImplementedException();
        }

        public List<Order> GetListOrders()
        {
            return _context.Orders.ToList();
        }

        public async Task<List<OrderView>> GetOrderById(int idOrder)
        {
            OderDetailService get = new OderDetailService(_context);
            AddressService get_address = new AddressService(_context);
            string payment = "";
            string time = "";
            var order = _context.Orders.FirstOrDefault(x => x.Id == idOrder);
            List<OrderView> list = new List<OrderView>();
            if(order!=null)
            {
                List<OrderDetailDTOs> details = await get.GetOrderDetailByOrderId(idOrder);
                List<CreateUpdateAddress> address = await get_address.GetAddressById(order.AddressId);
                if (order.PaymentMethod == 1) payment = "Banking";
                else payment = "COD";
                if (order.DeliveryTime == 1) time = "Anytime";
                else time = "Office hours only";
                    list.Add(new OrderView(){
                        Id = order.Id,
                        Status = order.Status,
                        Total = order.Total,
                        Note = order.Note,
                        Check = order.Check,
                        CheckedAt = order.CheckedAt,
                        CheckedBy = order.CheckedBy,
                        CheckedComment = order.CheckedComment,
                        UserId = order.UserId,
                        list = details,
                        address = address,
                        Time = time,
                        Payment = payment
                    });
                return await Task.FromResult(list);
            }
            throw null;
        }

        public List<Order> GetOrderByStatusOfEachUser(int userId, int status)
        {
            var order = GetOrdersByUserId(userId);
            string statuss = "";
            switch(status)
            {
                case 1:
                    statuss = Status.Cart.ToString();
                    break;
                case 2:
                    statuss = Status.Pending.ToString();
                    break;
                case 3:
                    statuss = Status.Shipping.ToString();
                    break;
                case 4:
                    statuss = Status.Shipped.ToString();
                    break;
                case 5:
                    statuss = Status.Cancel.ToString();
                    break;
            }
            List<Order> temp = new List<Order>();
            foreach(Order i in order)
            {
                if(i.Status == statuss)
                {
                    temp.Add(i);
                }
            }
            return temp;
        }

        public List<Order> GetOrdersByStatus(int status)
        {
            string statuss = "";
            switch(status)
            {
                case 1:
                    statuss = Status.Cart.ToString();
                    break;
                case 2:
                    statuss = Status.Pending.ToString();
                    break;
                case 3:
                    statuss = Status.Shipping.ToString();
                    break;
                case 4:
                    statuss = Status.Shipped.ToString();
                    break;
                case 5:
                    statuss = Status.Cancel.ToString();
                    break;
            }
            var order = _context.Orders.Where(x => x.Status == statuss);
            if(order != null)
            {
                return order.ToList();
            }
            throw null;
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            var order = _context.Orders.Where(x => x.UserId == userId);
            if(order != null)
            {
                return order.ToList();
            }
            throw null;
        }

        public async Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, int idOrder)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == idOrder);
            if(order != null)
            {
                order.Note = createUpdateOrder.Note;
                order.Check = createUpdateOrder.Check;
                order.CheckedAt = createUpdateOrder.CheckedAt;
                order.CheckedBy = createUpdateOrder.CheckedBy;
                order.CheckedComment = createUpdateOrder.CheckedComment;
                order.UserId = createUpdateOrder.UserId;
                var result = _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                var temp = await UpdateTotal(order.Id);
                return result.Entity;
            }
            else
            {
                double temp = 0;
                foreach(OrderDetailDTO i in createUpdateOrder.listpro)
                {
                    var product = _context.Products.FirstOrDefault(x=> x.Id == i.ProductId);
                    temp += i.Quantity * product.Price;
                }
                order = new Order();
                OderDetailService add = new OderDetailService(_context);
                order.Status = Status.Pending.ToString();
                order.Total = temp;
                order.Note = createUpdateOrder.Note;
                order.Check = createUpdateOrder.Check;
                order.CheckedAt = createUpdateOrder.CheckedAt;
                order.CheckedBy = createUpdateOrder.CheckedBy;
                order.CheckedComment = createUpdateOrder.CheckedComment;
                order.UserId = createUpdateOrder.UserId;
                CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
                var result = await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                foreach(OrderDetailDTO i in createUpdateOrder.listpro)
                {
                    await add.AddOrderDetail(i, order.Id);
                }
                return result.Entity;
            }
        }

        public async Task<bool> UpdateTotal(int idOrder)
        {
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