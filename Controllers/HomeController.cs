using Microsoft.AspNetCore.Mvc;
using BookingCase.Services;
using BookingCase.Models.ViewModels;
using System.Diagnostics;
using BookingCase.Models;

namespace BookingCase.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookingApiService _api;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IBookingApiService api, ILogger<HomeController> logger)
        {
            _api = api;
            _logger = logger;
        }

        // Formu göster
        [HttpGet]
        public IActionResult Index() => View(new SearchFormViewModel());

        // Form post: city -> dest_id -> otelleri getir
        [HttpPost]
        public async Task<IActionResult> Index(SearchFormViewModel m)
        {
            if (!ModelState.IsValid) return View(m);

            var destId = await _api.GetDestinationIdAsync(m.City);
            if (string.IsNullOrEmpty(destId))
            {
                ModelState.AddModelError("", "Þehir bulunamadý.");
                return View(m);
            }

            var req = new HotelSearchRequest
            {
                DestId = destId,
                SearchType = "CITY",
                CheckIn = m.CheckIn,
                CheckOut = m.CheckOut,
                Adults = m.Adults,
                Rooms = m.Rooms,
                PageNumber = m.PageNumber <= 0 ? 1 : m.PageNumber,
                Currency = m.Currency
            };

            var hotels = await _api.SearchHotelsAsync(req);

            // Detay linkinin kullanacaðý deðerler
            ViewBag.City = m.City;
            ViewBag.DestId = destId;
            ViewBag.CheckIn = m.CheckIn.ToString("yyyy-MM-dd");
            ViewBag.CheckOut = m.CheckOut.ToString("yyyy-MM-dd");
            ViewBag.Currency = m.Currency;

            return View("Results", hotels);
        }

        // Tek otel detay sayfasý
        [HttpGet]
        public async Task<IActionResult> Details(string id, string destId, string checkin, string checkout, string currency)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(destId))
                return NotFound();

            if (!DateTime.TryParse(checkin, out var dtIn)) dtIn = DateTime.Today;
            if (!DateTime.TryParse(checkout, out var dtOut)) dtOut = dtIn.AddDays(1);
            currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency;

            var vm = await _api.GetHotelDetailsAsync(destId, id, dtIn, dtOut, currency);
            if (vm == null) return NotFound();

            return View(vm); // Views/Home/Details.cshtml
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
