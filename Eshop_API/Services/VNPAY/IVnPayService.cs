using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Eshop_API.Models.DTO.VNPAY;

namespace Eshop_API.Services.VNPAY
{
    public interface IVnPayService
    {
        Task<string> CreateRequestUrl(ModelPayDto payInfo,string IpAddress);
        Task<object> ChecksumReponse(NameValueCollection queryString);
    }
}