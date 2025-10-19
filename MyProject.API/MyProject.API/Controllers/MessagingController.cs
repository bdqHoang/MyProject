using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.API.Common;
using MyProject.Application.Features.Message.Command.Create;
using MyProject.Application.Features.Message.DTO;
using MyProject.Application.Features.Message.Queries;
using System.Security.Claims;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagingController(
        ISender sender
        ) : ControllerBase
    {
        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        [HttpPost]
        [Route("send")]
        [ProducesResponseType(typeof(MessageRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
        {
            command.SenderId = Guid.Parse(GetUserId());
            var result = await sender.Send(command);

            return Ok(ApiResponse<MessageRes>.SuccessResponse(result, "Message sent successfully"));
        }

        [HttpGet]
        [Route("conversation/{conversationId:guid}/messages")]
        [ProducesResponseType(typeof(List<MessageRes>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessagesByConversationId(
            [FromRoute] Guid conversationId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = new GetMessageQuery
            {
                ConversationId = conversationId,
                Page = page,
                PageSize = pageSize,
                UserId = Guid.Parse(GetUserId())
            };
            var result = await sender.Send(query);
            return Ok(ApiResponse<List<MessageRes>>.SuccessResponse(result, "Messages retrieved successfully"));
        }

        [HttpGet]
        [Route("conversations")]
        [ProducesResponseType(typeof(List<ConversationRes>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserConversations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetUserConversationsQuery
            {
                UserId = Guid.Parse(GetUserId()),
                Page = page,
                PageSize = pageSize
            };
            var result = await sender.Send(query);
            return Ok(ApiResponse<List<ConversationRes>>.SuccessResponse(result, "Conversations retrieved successfully"));
        }


    }
}
