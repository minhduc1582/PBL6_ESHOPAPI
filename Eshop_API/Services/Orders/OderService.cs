using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Helpers;
using eshop_api.Services.Images;
using eshop_pbl6.Helpers.Orders;
using Microsoft.AspNetCore.Identity;
using eshop_pbl6.Models.DTO.Identities;
using eshop_pbl6.Services.Addresses;
using Eshop_API.Models.DTO.Adress;
using Eshop_API.Helpers.Orders;
using Newtonsoft.Json;
using Eshop_API.Services.VNPAY;
using Eshop_API.Models.DTO.VNPAY;
using System.Net.WebSockets;
using Sentry;
using AutoMapper;
using Eshop_API.Repositories.Orders;
using Eshop_API.Repositories.Products;
using Eshop_API.Repositories.Identities;
using CloudinaryDotNet.Actions;

namespace eshop_api.Services.Orders
{
    public class OderService : IOrderService
    {
        private readonly IOderDetailService _orderDetailService;
        private readonly IAddressService _addressService;
        private readonly IVnPayService _vnPayService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OderService( IOrderRepository orderRepository,
                            IConfiguration configuration,
                            IOderDetailService orderDetailService,
                            IAddressService addressService,
                            IVnPayService vnPayService,
                            IMapper mapper,
                            IProductRepository productRepository,
                            IOrderDetailRepository orderDetailRepository,
                            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailService = orderDetailService;
            _addressService = addressService;
            _vnPayService = vnPayService;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<OrderDto> AddOrder(List<OrderDetailDTO> orderDetailDTOs, int idUser, int idAddress, PaymentMethod payment, int time,string ipaddr,string UrlOrigin)
        {
            double temp = 0;
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                var product =await _productRepository.FirstOrDefault(x=> x.Id == i.ProductId);
                temp += i.Quantity * product.Price;
                product.ImportQuantity -= i.Quantity;
                product.ExportQuantity += i.Quantity;
                await _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();
                await DelFromCart(i.ProductId, idUser);
            }
            Order order = new Order();
            order.Status = Status.Pending.ToString();
            order.Total = temp;
            order.UserId = idUser;
            order.AddressId = idAddress;
            order.CreateAt = DateTime.Now;
            order.PaymentMethod = payment;
            order.DeliveryTime = time;
            CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
            var result = (await _orderRepository.Add(order));
            await _orderRepository.SaveChangesAsync();
            foreach(OrderDetailDTO i in orderDetailDTOs)
            {
                await _orderDetailService.AddOrderDetail(i, order.Id);
            }
            result.User = null;
            //var jsonOrder = JsonConvert.SerializeObject(result);
            //var orderDto = JsonConvert.DeserializeObject<OrderDto>(jsonOrder);
            var orderDto = _mapper.Map<Order, OrderDto>(result);
            string firstName = (await _userRepository.FirstOrDefault(x => x.Id == idUser)).FirstName;
            orderDto.FirstName = firstName;
            if(payment == PaymentMethod.Online){
                var user = await _userRepository.FirstOrDefault(x => x.Id == idUser);
                string url = (string.IsNullOrEmpty(UrlOrigin) == true ? _configuration["Vnpay:ReturnUrl"] : UrlOrigin) +  _configuration["Vnpay:EndPointPage"];
                ModelPayDto modalPayDto = new ModelPayDto{
                    Amount = result.Total,
                    Email = user.Email,
                    Name = user.FirstName + user.LastName,
                    Content = "Thanh toan don hang " + result.Id,
                    Tnx_Ref = result.Id,
                    UrlOrigin = url
                };
                orderDto.PaymentURL = await _vnPayService.CreateRequestUrl(modalPayDto,ipaddr);
                return orderDto;
            }
            return orderDto;
        }

        public async Task<OrderView> GetCart(string username)
        {
            int userId = (await _userRepository.FirstOrDefault(x => x.Username == username)).Id;
            List<Order> orders = await GetOrderByStatusOfEachUser(userId, 1);
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
                    order.CheckedAt = i.CheckedAt;
                    order.CreateAt = i.CreateAt;
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
            await _orderRepository.Add(new_order);
            await _orderRepository.SaveChangesAsync();
            order.Id = new_order.Id;
            order.Status = new_order.Status;
            order.Total = new_order.Total;
            order.UserId = new_order.UserId;
            return await Task.FromResult(order);
        }

        public async Task<Order> AddToCart(OrderDetailDTO detailDTOs, string username)
        {
            double temp = 0;
            int userId = (await _userRepository.FirstOrDefault(x => x.Username == username)).Id;
            var product = await _productRepository.FirstOrDefault(x=> x.Id == detailDTOs.ProductId);
            temp += detailDTOs.Quantity * product.Price;
            Boolean isExist = false;
            List<Order> orders = await GetOrdersByUserId(userId);
            foreach(Order i in orders)
            {
                if(i.Status == "Cart" )
                {
                    var orderDetails = await _orderDetailService.GetOrderDetailByOrderId(i.Id);
                    foreach(OrderDetailDTOs j in orderDetails)
                    {
                        if(j.ProductId == detailDTOs.ProductId && j.SizeId == detailDTOs.SizeId && j.ColorId == detailDTOs.ColorId)
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
                    var result = await _orderRepository.Update(i);
                    await _orderRepository.SaveChangesAsync();
                    return result;
                }
            }
            Order order = new Order();
            order.Status = Status.Cart.ToString();
            order.Total = temp;
            order.UserId = userId;
            CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
            var results = await _orderRepository.Add(order);
            await _orderRepository.SaveChangesAsync();
            await _orderDetailService.AddOrderDetail(detailDTOs, order.Id);
            return results;
        }

        public async Task<List<OrderDto>> ChangeStatus(List<Guid> idOrder, int status, string note)
        {
            var order = await _orderRepository.GetAll();
            List<OrderDto> list = new List<OrderDto>();
            foreach (var i in order)
            {
                foreach (var j in idOrder)
                {
                    if (i.Id == j)
                    {
                        switch (status)
                        {
                            case 1:
                                i.Status = Status.Cart.ToString();
                                break;
                            case 2:
                                i.Status = Status.Pending.ToString();
                                break;
                            case 3:
                                i.Status = Status.Shipping.ToString();
                                break;
                            case 4:
                                i.Status = Status.Shipped.ToString();
                                break;
                            case 5:
                                i.Status = Status.Cancel.ToString();
                                break;
                            default:
                                i.Status = Status.Cart.ToString();
                                break;
                        }
                        if (note != null) i.Note = note;
                        i.CheckedAt = DateTime.Now;
                        var result = await _orderRepository.Update(i);
                        await _orderRepository.SaveChangesAsync();
                        OrderDto orderDto = _mapper.Map<Order, OrderDto>(result);
                        list.Add(orderDto);
                    }
                }
            }
            return list;
        }

        public async Task<OrderDto> CancelOrder(Guid idOrder, string note)
        {
            var order = await _orderRepository.FirstOrDefault(x => x.Id == idOrder);
            if(order != null)
            {
                List<OrderDetail> orderDetail = await _orderDetailService.GetOrderDetailsByOrderId(idOrder);
                foreach(var i in orderDetail)
                {
                    var product = await _productRepository.FirstOrDefault(x => x.Id == i.ProductId);
                    product.ImportQuantity += i.Quantity;
                    product.ExportQuantity -= i.Quantity;
                    await _productRepository.Update(product);
                    await _productRepository.SaveChangesAsync();
                }
                order.Status = Status.Cancel.ToString();
                if (note != null) order.Note = note;
                order.CheckedAt = DateTime.Now;
                await _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
                OrderDto orderDto = _mapper.Map<Order, OrderDto>(order);
                return orderDto;
            }
            throw null;
        }

        public async Task<bool> DeleteOrderById(Guid idOrder)
        {
            var order = await _orderRepository.FirstOrDefault(x => x.Id == idOrder);
            if(order != null)
            {
                var result = _orderRepository.Remove(order);
                await _orderRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order> DelFromCart(int idProduct, int idUser)
        {
            List<Order> order = await GetOrderByStatusOfEachUser(idUser, 1);
            foreach(Order i in order)
            {
                List<OrderDetail> orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(i.Id);
                foreach(OrderDetail j in orderDetails)
                {
                    if(j.ProductId == idProduct)
                    {
                        await _orderDetailService.DeleteOrderDetail(j.Id);
                    }
                }
                await UpdateTotal(i.Id);
                var result = await _orderRepository.Update(i);
                await _orderRepository.SaveChangesAsync();
                return result;
            }
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetListOrders()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order> UpdateCart(List<OrderDetailDTO> orderDetailDTOs, int idUser)
        {
            var order = await _orderRepository.FirstOrDefault(x => x.UserId == idUser && x.Status == Status.Cart.ToString());
            foreach (OrderDetailDTO i in orderDetailDTOs)
            {
                await DelFromCart(i.ProductId, idUser);
                await _orderDetailService.AddOrderDetail(i, order.Id);
            }
            await UpdateTotal(order.Id);
            return order;
        }

        public async Task<OrderView> GetOrderById(Guid idOrder)
        {
            string payment = "";
            string time = "";
            var order = await _orderRepository.FirstOrDefault(x => x.Id == idOrder);
            if(order!=null)
            {
                List<OrderDetailDTOs> details = await _orderDetailService.GetOrderDetailByOrderId(idOrder);
                List<AddressView> address = await _addressService.GetAddressById((int)order.AddressId);
                if (order.PaymentMethod == PaymentMethod.Online) payment = "Banking";
                else payment = "COD";
                if (order.DeliveryTime == 1) time = "Anytime";
                else time = "Office hours only";
                    OrderView orderView = new OrderView(){
                        Id = order.Id,
                        Status = order.Status,
                        Total = order.Total,
                        Note = order.Note,
                        CheckedAt = order.CheckedAt,
                        CheckedBy = order.CheckedBy,
                        CheckedComment = order.CheckedComment,
                        UserId = order.UserId,
                        list = details,
                        address = address,
                        Time = time,
                        Payment = payment
                    };
                return await Task.FromResult(orderView);
            }
            throw null;
        }

        public async Task<List<Order>> GetOrderByStatusOfEachUser(int userId, int status)
        {
            var order = await GetOrdersByUserId(userId);
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

        public async Task<List<OrderDto>> GetOrdersByStatus(int status)
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
            var order = await _orderRepository.Find(x => x.Status == statuss);
            if(order != null)
            {
                List<OrderDto> list = new List<OrderDto>();
                foreach (var i in order)
                {
                    //var jsonOrder = JsonConvert.SerializeObject(i);
                    //var orderDto = JsonConvert.DeserializeObject<OrderDto>(jsonOrder);
                    var orderDto = _mapper.Map<Order, OrderDto>(i);
                    string firstName = (await _userRepository.FirstOrDefault(x => x.Id == i.UserId)).FirstName;
                    orderDto.FirstName = firstName;
                    list.Add(orderDto);
                }
                return list;
            }
            throw null;
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            var order = await _orderRepository.Find(x => x.UserId == userId);
            if(order != null)
            {
                return order;
            }
            throw null;
        }

        public async Task<Order> UpdateOrder(CreateUpdateOrder createUpdateOrder, Guid idOrder)
        {
            var order = await _orderRepository.FirstOrDefault(x => x.Id == idOrder);
            if(order != null)
            {
                order.Note = createUpdateOrder.Note;
                order.CheckedAt = createUpdateOrder.CheckedAt;
                order.CheckedAt = createUpdateOrder.CreateAt;
                order.CheckedBy = createUpdateOrder.CheckedBy;
                order.CheckedComment = createUpdateOrder.CheckedComment;
                order.UserId = createUpdateOrder.UserId;
                var result = await _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
                var temp = await UpdateTotal(order.Id);
                return result;
            }
            else
            {
                double temp = 0;
                foreach(OrderDetailDTO i in createUpdateOrder.listProduct)
                {
                    var product = await _productRepository.FirstOrDefault(x=> x.Id == i.ProductId);
                    temp += i.Quantity * product.Price;
                }
                order = new Order();
                order.Status = Status.Pending.ToString();
                order.Total = temp;
                order.Note = createUpdateOrder.Note;
                order.CheckedAt = createUpdateOrder.CheckedAt;
                order.CheckedBy = createUpdateOrder.CheckedBy;
                order.CheckedComment = createUpdateOrder.CheckedComment;
                order.UserId = createUpdateOrder.UserId;
                CreateUpdateOrderDetail orderDetail = new CreateUpdateOrderDetail();
                var result = await _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();
                foreach(OrderDetailDTO i in createUpdateOrder.listProduct)
                {
                    await _orderDetailService.AddOrderDetail(i, order.Id);
                }
                return result;
            }
        }

        public async Task<bool> UpdateTotal(Guid idOrder)
        {
            double temp = 0;
            var order = await _orderRepository.FirstOrDefault(x=> x.Id == idOrder);
            if(order == null) return false;
            var orderDetail = await _orderDetailRepository.Find(x=> x.OrderId == order.Id);
            if(orderDetail != null)
            {
                foreach(OrderDetail i in orderDetail)
                {
                    var product =await _productRepository.FirstOrDefault(x=> x.Id == i.ProductId);
                    temp += i.Quantity * product.Price;
                }
            }
            order.Total = temp;
            var result = _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }
    }
}