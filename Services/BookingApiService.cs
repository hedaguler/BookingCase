using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using BookingCase.Models.ViewModels;

namespace BookingCase.Services
{
    public class BookingApiService : IBookingApiService
    {
        private readonly IHttpClientFactory _http;
        public BookingApiService(IHttpClientFactory http) => _http = http;

        // 1) Şehir -> dest_id
        public async Task<string?> GetDestinationIdAsync(string city)
        {
            var client = _http.CreateClient("booking");
            var url = QueryHelpers.AddQueryString(
                "api/v1/hotels/searchDestination",
                new Dictionary<string, string?>
                {
                    ["query"] = city,
                    ["languagecode"] = "en-us"   // snippet ile uyum
                });

            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();

            if (body.TrimStart().StartsWith("["))
            {
                var arr = JArray.Parse(body);
                return arr.FirstOrDefault()?["dest_id"]?.ToString();
            }

            var obj = JObject.Parse(body);
            return obj.SelectToken("$.data[0].dest_id")?.ToString()
                ?? obj.SelectToken("$.destinations[0].dest_id")?.ToString();
        }

        // 2) Otel araması (snippet opsiyonelleri eklendi)
        public async Task<List<HotelCardViewModel>> SearchHotelsAsync(HotelSearchRequest req)
        {
            var client = _http.CreateClient("booking");
            var url = QueryHelpers.AddQueryString(
                "api/v1/hotels/searchHotels",
                new Dictionary<string, string?>
                {
                    ["dest_id"] = req.DestId,
                    ["search_type"] = req.SearchType, // "CITY"
                    ["arrival_date"] = req.CheckIn.ToString("yyyy-MM-dd"),
                    ["departure_date"] = req.CheckOut.ToString("yyyy-MM-dd"),
                    ["adults"] = req.Adults.ToString(),
                    ["room_qty"] = req.Rooms.ToString(),
                    ["page_number"] = req.PageNumber.ToString(),
                    ["currency_code"] = req.Currency,   // USD / TRY
                    // --- snippet ile uyum için opsiyoneller ---
                    ["units"] = "metric",
                    ["temperature_unit"] = "c",
                    ["languagecode"] = "en-us",
                    ["location"] = "US"            // sorun çıkarırsa ilk kaldıracağımız bu
                });

            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();

            var root = JObject.Parse(body);
            var hotels = (JArray?)(root["result"] ?? root["results"] ?? root["data"]?["hotels"]) ?? new JArray();

            var list = new List<HotelCardViewModel>();
            foreach (var h in hotels)
            {
                var p = h["property"] ?? h;

                var photo =
                     p["max_photo_url"]?.ToString()
                  ?? p["main_photo_url"]?.ToString()
                  ?? p["photoMainUrl"]?.ToString()
                  ?? (p["photoUrls"] as JArray)?.FirstOrDefault()?.ToString()
                  ?? p.SelectToken("photoUrl")?.ToString()
                  ?? "";

                list.Add(new HotelCardViewModel
                {
                    HotelId = p["hotel_id"]?.ToString() ?? p["id"]?.ToString(),
                    HotelName = p["hotel_name"]?.ToString() ?? p["name"]?.ToString(),
                    Address = p["address"]?.ToString(),
                    Score = (double?)(p["review_score"] ?? p["reviewScore"]) ?? 0,
                    Price = (decimal?)(
                                  p.SelectToken("price_breakdown.gross_price")
                               ?? p.SelectToken("priceBreakdown.grossPrice.value")),
                    PhotoUrl = string.IsNullOrWhiteSpace(photo) ? "/img/no-photo.png" : photo


                });
            }
            return list;
        }

        // 3) Detay (opsiyoneller yine eklendi)
        public async Task<HotelDetailViewModel?> GetHotelDetailsAsync(
            string destId, string hotelId, DateTime checkIn, DateTime checkOut, string currency)
        {
            var client = _http.CreateClient("booking");

            var url = QueryHelpers.AddQueryString(
    "api/v1/hotels/searchHotels",
    new Dictionary<string, string?>
    {
        ["dest_id"] = destId,
        ["search_type"] = "CITY",
        ["arrival_date"] = checkIn.ToString("yyyy-MM-dd"),
        ["departure_date"] = checkOut.ToString("yyyy-MM-dd"),
        ["adults"] = "2",
        ["room_qty"] = "1",
        ["page_number"] = "1",
        ["currency_code"] = currency
        // istersen buraya opsiyonelleri de ekleyebilirsin:
        // ["languagecode"] = "en-us",
        // ["units"] = "metric",
        // ["temperature_unit"] = "c",
        // ["location"] = "US"
    });



            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();

            var root = JObject.Parse(body);
            var hotels = (JArray?)(root["result"] ?? root["results"] ?? root["data"]?["hotels"]) ?? new JArray();

            var match = hotels.FirstOrDefault(h =>
                (h?["hotel_id"]?.ToString() ?? h?["property"]?["id"]?.ToString() ?? "") == hotelId);
            if (match == null) return null;

            var p = match["property"] ?? match;

            return new HotelDetailViewModel
            {
                HotelId = hotelId,
                HotelName = p["hotel_name"]?.ToString() ?? p["name"]?.ToString() ?? "",
                Name = p["name"]?.ToString(),
                Description = p["accessibilityLabel"]?.ToString(),
                Address = p["address"]?.ToString(),
                Score = p["reviewScore"]?.ToObject<double?>() ?? p["review_score"]?.ToObject<double?>(),
                ReviewCount = p["reviewCount"]?.ToObject<int?>(),
                Price = p.SelectToken("priceBreakdown.grossPrice.value")?.ToObject<decimal?>()
                             ?? p.SelectToken("price_breakdown.gross_price")?.ToObject<decimal?>(),
                Currency = currency,
                CheckinFrom = p.SelectToken("checkin.fromTime")?.ToString(),
                CheckoutUntil = p.SelectToken("checkout.untilTime")?.ToString(),
                PhotoUrls = (p["photoUrls"] as JArray)?.Select(x => x?.ToString() ?? "")
                                   .Where(s => !string.IsNullOrWhiteSpace(s))
                                   .Take(10).ToList() ?? new List<string>(),
                CheckIn = checkIn,
                CheckOut = checkOut
            };
        }
    }
}
