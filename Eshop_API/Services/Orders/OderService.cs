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
using Eshop_API.Helpers.Order;
using Newtonsoft.Json;
using Eshop_API.Services.VNPAY;
using Eshop_API.Models.DTO.VNPAY;

namespace eshop_api.Services.Orders
{
    public class OderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly IOderDetailService _orderDetailService;
        private readonly IAddressService _addressService;
        private readonly IVnPayService _vnPayService;
        public OderService(DataContext context,
                            IOderDetailService orderDetailService,
                            IAddressService addressService,
                            IVnPayService vnPayService)
        {
            _context = context;
            _orderDetailService = orderDetailService;
            _addressService = addressService;
            _vnPayService = vnPayService;
        }
        
        public async Task<OrderDto> AddOrder(List<OrderDetailDTO> orderDetailDTOs, string username, int idAddress, PaymentMethod payment, int time,string ipaddr)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Username == username);
            double temp = 0;
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                var product = _context.Products.FirstOrDefault(x=> x.Id == i.ProductId);
                temp += i.Quantity * product.Price;
                await DelFromCart(i.ProductId, username, i.Quantity);
            }
            Order order = new Order();
            order.Status = Status.Pending.ToString();
            order.Total = temp;
            order.UserId = user.Id;
            order.AddressId = idAddress;
            order.PaymentMethod = payment;
            order.DeliveryTime = time;
            CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
            var result = (await _context.Orders.AddAsync(order)).Entity;
            await _context.SaveChangesAsync();
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                await _orderDetailService.AddOrderDetail(i, order.Id);
            }
            result.User = null;
            var jsonOrder = JsonConvert.SerializeObject(result);
            var orderDto = JsonConvert.DeserializeObject<OrderDto>(jsonOrder);
            if(payment == PaymentMethod.Online){
                ModelPayDto modalPayDto = new ModelPayDto{
                    Amount = result.Total,
                    Email = user.Email,
                    Name = user.FirstName + user.LastName,
                    Content = "Thanh toan don hang " + result.Id,
                    Tnx_Ref = result.Id
                };
                orderDto.PaymentURL = await _vnPayService.CreateRequestUrl(modalPayDto,ipaddr);
                return orderDto;
            }
            return orderDto;
        }

        public async Task<OrderView> GetCart(string username)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            var orders = _context.Orders.Where(x => x.Status == Status.Cart.ToString()).ToList();
            OrderView order = new OrderView();
            if(orders!=null)
            {
                foreach(Order i in orders)
                {
                    List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
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
            var product = _context.Products.FirstOrDefault(x => x.Id == detailDTOs.ProductId);
            temp += detailDTOs.Quantity * product.Price;
            Boolean isExist = false;
            var orders = _context.Orders.Where(x => x.UserId == userId).ToList();
            foreach (Order i in orders)
            {
                if(i.Status == "Cart")
                {
                    var orderDetails = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
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
                            await _orderDetailService.UpdateOrderDetail(createUpdateOrderDetail, j.Id);
                        }
                    }
                    if(isExist == false)
                    {   
                        await _orderDetailService.AddOrderDetail(detailDTOs, i.Id);
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
            await _orderDetailService.AddOrderDetail(detailDTOs, order.Id);
            return results.Entity;
        }

        public async Task<Order> ChangeStatus(Guid idOrder, int status)
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

        public async Task<bool> DeleteOrderById(Guid idOrder)
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
            var order = _context.Orders.Where(x => x.Status == Status.Cart.ToString()).ToList();
            //List<Order> order = GetOrderByStatusOfEachUser(userId, 1).ToList();
            foreach (Order i in order)
            {
                List<OrderDetail> orderDetails = _orderDetailService.GetOrderDetailsByOrderId(i.Id).ToList();
                foreach(OrderDetail j in orderDetails)
                {
                    if(j.ProductId == idProduct)
                    {
                        int temp = j.Quantity - quantity;
                        if (temp == 0)
                            await _orderDetailService.DeleteOrderDetail(j.Id);
                        else
                        {
                            CreateUpdateOrderDetail createUpdateOrderDetail = new CreateUpdateOrderDetail();
                            createUpdateOrderDetail.idOrder = j.OrderId;
                            createUpdateOrderDetail.ProductId = j.ProductId;
                            createUpdateOrderDetail.Quantity = temp;
                            createUpdateOrderDetail.Note = j.Note;
                            await _orderDetailService.UpdateOrderDetail(createUpdateOrderDetail, j.Id);
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

        public async Task<List<OrderView>> GetListOrders(bool getDetails)
        {
            string payment = "";
            string time = "";
            var order = _context.Orders.Take(5).ToList();
            List<OrderView> list = new List<OrderView>();
            if (order != null)
            {
                foreach (Order i in order)
                {
                    if (i.Status != Status.Cart.ToString())
                    {
                        if (getDetails == true)
                        {
                            List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = details,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                            //var orderView = await GetOrderById(i.Id);
                            //foreach(OrderView j in orderView)
                            //    list.Add(j);
                        }
                        else
                        {
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = null,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                        }
                    }
                }
                return await Task.FromResult(list);
            }
            throw null;
        }


        public async Task<List<OrderView>> GetOrderById(Guid idOrder)
        {
            string payment = "";
            string time = "";
            var order = _context.Orders.FirstOrDefault(x => x.Id == idOrder);
            List<OrderView> list = new List<OrderView>();
            if(order!=null)
            {
                List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(idOrder);
                List<AddressView> address = await _addressService.GetAddressById((int)order.AddressId);
                if (order.PaymentMethod == PaymentMethod.Online) payment = "Banking";
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

        public async Task<List<OrderView>> GetOrderByStatusOfEachUser(string username, int status, bool getDetails)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            var order = await GetOrdersByUserId(userId, getDetails);
            string statuss = "";
            switch (status)
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
            List<OrderView> temp = new List<OrderView>();
            foreach (OrderView i in order)
            {
                if (i.Status == statuss)
                {
                    temp.Add(i);
                }
            }
            return temp;
        }
        public async Task<List<OrderView>> GetOrdersByStatus(int status, bool getDetails)
        {
            string payment = "";
            string time = "";
            string statuss = "";
            switch (status)
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
            var order = _context.Orders.Where(x => x.Status == statuss).Take(5).ToList();
            List<OrderView> list = new List<OrderView>();
            if (order != null)
            {
                foreach (Order i in order)
                {
                    if (i.Status != Status.Cart.ToString())
                    {
                        if (getDetails == true)
                        {
                            List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = details,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                            //var orderView = await GetOrderById(i.Id);
                            //foreach(OrderView j in orderView)
                            //    list.Add(j);
                        }
                        else
                        {
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = null,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                        }
                    }
                }
                return await Task.FromResult(list);
            }
            throw null;
        }
        public async Task<List<OrderView>> GetOrdersByUserId(int userId, bool getDetails)
        {
            string payment = "";
            string time = "";
            var order = _context.Orders.Where(x => x.UserId == userId).Take(5).ToList();
            List<OrderView> list = new List<OrderView>();
            if (order != null)
            {
                foreach (Order i in order)
                {
                    if (i.Status != Status.Cart.ToString())
                    {
                        if (getDetails == true)
                        {
                            List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = details,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                            //var orderView = await GetOrderById(i.Id);
                            //foreach(OrderView j in orderView)
                            //    list.Add(j);
                        }
                        else
                        {
                            List<AddressView> address = await _addressService.GetAddressById((int)i.AddressId);
                            if (i.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                            else payment = "COD";
                            if (i.DeliveryTime == 1) time = "Anytime";
                            else time = "Office hours only";
                            list.Add(new OrderView()
                            {
                                Id = i.Id,
                                Status = i.Status,
                                Total = i.Total,
                                Note = i.Note,
                                Check = i.Check,
                                CheckedAt = i.CheckedAt,
                                CheckedBy = i.CheckedBy,
                                CheckedComment = i.CheckedComment,
                                UserId = i.UserId,
                                list = null,
                                address = address,
                                Time = time,
                                Payment = payment
                            });
                        }
                    }
                }
                return await Task.FromResult(list);
            }
            throw null;
        }

        public async Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, Guid idOrder)
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
                    await _orderDetailService.AddOrderDetail(i, order.Id);
                }
                return result.Entity;
            }
        }

        public async Task<bool> UpdateTotal(Guid idOrder)
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

        public List<Order> GetListOrders()
        {
            throw new NotImplementedException();
        }

        
    }
}