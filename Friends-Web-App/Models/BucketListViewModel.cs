using System.ComponentModel.DataAnnotations;

namespace Friends_Web_App.Models
{
    public class BucketListViewModel
    {
        public int BucketListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int rating { get; set; }
        public Status status { get; set; }
    }
}
