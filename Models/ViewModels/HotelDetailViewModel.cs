namespace BookingCase.Models.ViewModels
{
    public class HotelDetailViewModel
    {
        public string HotelId { get; set; } = "";
        public string HotelName { get; set; } = "";  // listede kullanıyoruz
        public string? Name { get; set; }            // detay mapping'i için
        public string? Description { get; set; }
        public string? Address { get; set; }
        public double? Score { get; set; }
        public int? ReviewCount { get; set; }        // <-- eklendi
        public decimal? Price { get; set; }
        public string Currency { get; set; } = "USD";
        public string? CheckinFrom { get; set; }     // <-- eklendi
        public string? CheckoutUntil { get; set; }   // <-- eklendi
        public List<string> PhotoUrls { get; set; } = new();
        public List<string> Included { get; set; } = new();
        public List<string> Excluded { get; set; } = new();
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
