using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]

    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;

        public FeedController(IPostsRepo posts)
        {
            _posts = posts;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
        {
            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
