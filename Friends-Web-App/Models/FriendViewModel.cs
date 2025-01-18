namespace Friends_Web_App.Models
{
    public class FriendViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int age { get; set; }
        public required int Year { get; set; }
        public string? Phone { get; set; }
        public GenderType Gender { get; set; }
        public Hometown? hometown { get; set; }
    }
}
