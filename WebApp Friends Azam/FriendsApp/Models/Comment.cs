namespace FriendsApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;  // Add this namespace for ForeignKey

    public class Comment
    {
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(500, ErrorMessage = "Content cannot be longer than 500 characters.")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Use UTC time for consistency

        public int PostId { get; set; } // Foreign key to the Post

        public int UserId { get; set; } // Foreign key to the User

        // Explicitly defining foreign keys (optional, EF Core can infer these)
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
