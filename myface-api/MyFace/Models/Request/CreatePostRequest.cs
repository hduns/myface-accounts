using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyFace.Models.Request
{
    public class CreatePostRequest
    {
        [Required]
        [StringLength(140)]
        public string Message { get; set; }
        
        public string ImageUrl { get; set; }

        public string AuthorizationHeader {get; set;}
        
        // [HiddenInput]
        // [Required]
        // [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        // public int UserId { get; set; }
    }
}