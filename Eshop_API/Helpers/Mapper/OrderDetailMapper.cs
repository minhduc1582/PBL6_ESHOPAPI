using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Models.DTO.Products;

namespace eshop_api.Helpers.Mapper
{
    public static class OrderDetailMapper
    {
        public static OrderDetailDTOs toOrderDetailDto(OrderDetail orderDetail, Product product){
            OrderDetailDTOs orderDetailDTOs = new OrderDetailDTOs();
            orderDetailDTOs.Id = orderDetail.Id;
            orderDetailDTOs.OrderId = orderDetail.OrderId;
            orderDetailDTOs.ProductId = orderDetail.ProductId;
            orderDetailDTOs.AvtImageUrl = product.AvtImageUrl;
            orderDetailDTOs.ProductName = product.Name;
            orderDetailDTOs.StorageQuantity = product.ImportQuantity - product.ExportQuantity;
            orderDetailDTOs.Price = product.Price;
            orderDetailDTOs.Quantity = orderDetail.Quantity;
            orderDetailDTOs.Note = orderDetail.Note;
            return orderDetailDTOs;
        }
    }
}