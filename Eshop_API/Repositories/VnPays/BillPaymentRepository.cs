using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.VnPays
{
    public class BillPaymentRepository: GenericRepository<BillPay>, IBillPaymentRepository
    {
         private readonly DataContext _context;
        public BillPaymentRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}