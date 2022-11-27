using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Services.Hub
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(List<string> message);
    }
}