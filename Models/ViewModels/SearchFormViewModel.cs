using System.ComponentModel.DataAnnotations;

namespace BookingCase.Models.ViewModels
{
    public class SearchFormViewModel
    {
        [Required] public string City { get; set; } = "Milano";
        [DataType(DataType.Date)] public DateTime CheckIn { get; set; } = DateTime.Today.AddDays(30);
        [DataType(DataType.Date)] public DateTime CheckOut { get; set; } = DateTime.Today.AddDays(31);
        [Range(1, 10)] public int Adults { get; set; } = 2;
        [Range(1, 10)] public int Rooms { get; set; } = 1;
        public int PageNumber { get; set; } = 1;         // RapidAPI default 1
        public string Currency { get; set; } = "USD";    // veya TRY


        public string? UserName { get; set; }

    }
}
