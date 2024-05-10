using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITicketService
    {
         Task<List<TicketInformation>> GetTicketInformation(Guid webbUid);
        Task<List<TicketKundInfo>> GetTicketKundInfo(Guid webbUid);

    }
}