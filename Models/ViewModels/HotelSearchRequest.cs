namespace BookingCase.Models.ViewModels
{
    public class HotelSearchRequest
    {
        public string DestId { get; set; } = "";
        public string SearchType { get; set; } = "CITY";
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Adults { get; set; }
        public int Rooms { get; set; }
        public int PageNumber { get; set; } = 1;
        public string Currency { get; set; } = "USD";
    }
}
