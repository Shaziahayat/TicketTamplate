using Models;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using Configuration;
using DbContext;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TicketService : ITicketService
    {
        private readonly ILogger<TicketService> _logger;
        private readonly csMainDbContext _dbContext;

        // Define the default webbUid
      private readonly Guid defaultWebbUid = new Guid("45bd9c1d-2fb7-4677-8e69-f4857f0cc206"); // one day remains
       //private readonly Guid defaultWebbUid = new Guid("8bcaaea1-cca3-4580-baa0-834057d793ea");
       // private readonly Guid defaultWebbUid = new Guid("24f12969-e4ac-4ebb-b70d-444ab7ec0f58");
        public TicketService(ILogger<TicketService> logger, csMainDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<TicketInformation>> GetTicketInformation(Guid webbUid)
        {
            // Use the provided webbUid if available, otherwise use the default
            var query = _dbContext.Vy_TicketInformation.AsNoTracking()
                             .Where(ticket => ticket.WebbUid == (webbUid != Guid.Empty ? webbUid : defaultWebbUid));

            return await query.ToListAsync();
        }

        public async Task<List<TicketKundInfo>> GetTicketKundInfo(Guid webbUid)
        {
            // Use the provided webbUid if available, otherwise use the default
            var query = _dbContext.Vy_ticketKundInfo.AsNoTracking()
                             .Where(ticket => ticket.WebbUid == (webbUid != Guid.Empty ? webbUid : defaultWebbUid));
            return await query.ToListAsync();
        }
    }

}
