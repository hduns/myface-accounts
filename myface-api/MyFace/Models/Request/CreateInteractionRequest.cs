using System.ComponentModel.DataAnnotations;
using MyFace.Models.Database;

namespace MyFace.Models.Request
{
    public class CreateInteractionRequest
    {
        [Required]
        public InteractionType InteractionType { get; set; }

        // [Required]
        // public int UserId { get; set; }
        public string AuthorizationHeader {get; set;}
        
        [Required]
        public int PostId { get; set; }
    }
}