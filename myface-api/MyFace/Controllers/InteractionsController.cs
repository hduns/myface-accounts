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
    [Route("/interactions")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]

    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IUsersRepo _users;

        public InteractionsController(IInteractionsRepo interactions, IUsersRepo users)
        {
            _interactions = interactions;
            _users = users;
        }
    
        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search)
        {
            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById([FromRoute] int id)
        {
            var interaction = _interactions.GetById(id);
            return new InteractionResponse(interaction);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateInteractionRequest newUser)
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
            return Ok(user);

            var formatInteraction = new FormatCreateInteractionRequest
            {
                InteractionType = newUser.InteractionType,
                PostId = newUser.PostId,
                UserId = user.Id
            };
        
            var interaction = _interactions.Create(formatInteraction);

            var url = Url.Action("GetById", new { id = interaction.Id });
            var responseViewModel = new InteractionResponse(interaction);
            return Created(url, responseViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _interactions.Delete(id);
            return Ok();
        }
    }
}
