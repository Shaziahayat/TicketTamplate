using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Models;
using QRCoder;
using Services;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using TicketTamplate.Services;

namespace TicketTamplate.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITicketService _ticketService;

        private LanguageService _languageService;

        public List<TicketInformation> Tickets = new List<TicketInformation>();
        public List<TicketKundInfo> KundInfos = new List<TicketKundInfo>();
        public int TicketCount { get; set; }
        public string TicketText { get; set; }
        public string BookingNumberLabel { get; set; }
        public string NameLabel { get; set; }
        public string EmailLabel { get; set; }
        public string PhoneLabel { get; set; }
        public string TicketCountLabel { get; set; }
        public string TicketExpired { get; set; }
        public string TicketPurchaseLink { get; set; }
        public string NumberOfTicketsLabel { get; set; }
        public string DateLabel { get; set; }
        public string ExpirationDateLabel { get; set; }


        //private readonly Guid defaultWebbUid = new Guid("09149456-45dd-40d1-9513-2eb847359550"); // Expired
        private readonly Guid defaultWebbUid = new Guid("ba0560c4-0e7a-4ee8-8392-85f992d16a63");  //16
                                                                                                  //private readonly Guid defaultWebbUid = new Guid("24f12969-e4ac-4ebb-b70d-444ab7ec0f58"); //Expired

        public IndexModel(ILogger<IndexModel> logger, ITicketService ticketService, LanguageService localization)
        {
            _ticketService = ticketService;
            _logger = logger;
            _languageService = localization;

        }

        public async Task OnGetAsync()
        {
            Guid webbUid;

            BookingNumberLabel = _languageService.GetKey("BookingNumberLabel");
            NameLabel = _languageService.GetKey("NameLabel");
            EmailLabel = _languageService.GetKey("EmailLabel");
            PhoneLabel = _languageService.GetKey("PhoneLabel");
            TicketCountLabel = _languageService.GetKey("TicketCountLabel");
            TicketExpired = _languageService.GetKey("TicketExpired");
            TicketPurchaseLink = _languageService.GetKey("TicketPurchaseLink");
            NumberOfTicketsLabel = _languageService.GetKey("NumberOfTicketsLabel");
            DateLabel = _languageService.GetKey("DateLabel");
            ExpirationDateLabel = _languageService.GetKey("ExpirationDateLabel");
            TicketText = _languageService.GetKey("Tickets");

            if (!Guid.TryParse(Request.Query["webbUid"], out webbUid))
            {

                _logger.LogError("WebbUid is not provided or invalid, falling back to default.");

                webbUid = defaultWebbUid;
            }

            Tickets = await _ticketService.GetTicketInformation(webbUid);
            KundInfos = await _ticketService.GetTicketKundInfo(webbUid);

            TicketCount = Tickets.Count;


            if (IsIOSDevice())
            {
                _logger.LogInformation("iOS device detected.");

            }
        }

        public string GenerateQRCodeUri(string barcode)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(barcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(13);


            byte[] qrCodeBytes;
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                qrCodeBytes = stream.ToArray();
            }


            string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);


            return $"data:image/png;base64,{qrCodeBase64}";
        }
        public async Task<IActionResult> OnGetSaveToWalletAsync(Guid webbUid)
        {
            _logger.LogInformation("Generating pass for webbUid: {WebbUid}", webbUid);
            var passBytes = await GeneratePassForTicketAsync(webbUid);
            if (passBytes == null || passBytes.Length == 0)
            {
                _logger.LogError("Pass generation failed or returned empty.");
                return BadRequest("Failed to generate pass.");
            }
            return File(passBytes, "application/vnd.apple.pkpass", "ticket.pkpass");
        }



        private async Task<byte[]> GeneratePassForTicketAsync(Guid webbUid)
        {

            var samplePassContent = $"Sample pass for WebbUid: {webbUid}";

            var samplePassBytes = Encoding.UTF8.GetBytes(samplePassContent);

            return samplePassBytes;
        }
        public bool IsIOSDevice()
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            return userAgent.Contains("iPhone") || userAgent.Contains("iPad");
        }
    }

}
