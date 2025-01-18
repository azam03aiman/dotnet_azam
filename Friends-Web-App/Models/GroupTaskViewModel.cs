using System.ComponentModel.DataAnnotations;

namespace Friends_Web_App.Models
{
    public class GroupTaskViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public required int Priority { get; set; }
        public required DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
