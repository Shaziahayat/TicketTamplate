using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketInformation
    {
        public string namn1 { get; set; }
        public string namn2 { get; set; }
        public DateTime datumStart { get; set; }
        public DateTime dateEnd { get; set; }
        public string Artikelnamn { get; set; }
        public string barcode { get; set; }
        public DateTime? Webbläst { get; set; }
        public string Namn { get; set; }
        public byte? status { get; set; }
        public decimal Pris { get; set; }
        public string? Venue { get; set; }
        public string? stolsrad { get; set; }
        public int? stolsnr { get; set; }
        public Guid platsbokadGUID { get; set; }
        public string Logorad1 { get; set; }
        public string Logorad2 { get; set; }
        public Guid WebbUid { get; set; }

    }

    public class vwTicketInfo
    {
        public List<TicketInformation> TicketInformation { get; set; } = new List<TicketInformation>();
    }
    public class TicketKundInfo
    {
        public decimal BokningsNr { get; set; }
        public string Bokatav { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Guid WebbUid { get; set; }
    }

}
