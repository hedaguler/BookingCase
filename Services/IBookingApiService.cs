using BookingCase.Models.ViewModels;

namespace BookingCase.Services
{
    public interface IBookingApiService
    {
        Task<string?> GetDestinationIdAsync(string city);
        Task<List<HotelCardViewModel>> SearchHotelsAsync(HotelSearchRequest req);

        // YENİ: Detay metodu
        Task<HotelDetailViewModel?> GetHotelDetailsAsync(
            string destId, string hotelId, DateTime checkIn, DateTime checkOut, string currency);
    }
}
