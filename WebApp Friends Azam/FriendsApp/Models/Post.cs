namespace FriendsApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(1, ErrorMessage = "Content cannot be empty.")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Default to current time when post is created

        public DateTime? UpdatedAt { get; set; }  // Nullable, as it is only set when the post is updated

        public int UserId { get; set; }  // Foreign key for the User

        // Navigation property to related User
        public virtual User User { get; set; }

        // Navigation property for comments
        public virtual ICollection<Comment> Comments { get; set; } // Make sure this is present
    }



}
