namespace BookingCase.Models.ViewModels
{
    public class HotelCardViewModel
    {
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string PhotoUrl { get; set; }
        public string Address { get; set; }
        public double? Score { get; set; }
        public decimal? Price { get; set; }
    }
}
