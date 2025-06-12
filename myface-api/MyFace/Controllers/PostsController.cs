using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]

    public class PostsController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public PostsController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            string encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':');
            var username = usernamePassword.Substring(0, seperatorIndex);
            var user = _users.GetByUsername(username);

            var post = new FormatNewPostRequest
            {
                ImageUrl = newPost.ImageUrl,
                Message = newPost.Message,
                PostedAt = DateTime.Now,
                UserId = user.Id,
            };
            
            var formattedPost = _posts.Create(post);

            var url = Url.Action("GetById", new { id = post.UserId });
            var postResponse = new PostResponse(formattedPost);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _posts.Delete(id);
            return Ok();
        }
    }
}