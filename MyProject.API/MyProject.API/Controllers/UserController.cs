using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.API.Common;
using MyProject.Application.Features.User.Commands.Create;
using MyProject.Application.Features.User.Commands.Delete;
using MyProject.Application.Features.User.Commands.Update;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Features.User.Queries;
using MyProject.Application.Interface;
using MyProject.Core.Entities;
using System.Security.Claims;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager},{RoleName.User}")]
    public class UserController(
        ISender sender,
        IEmailService emailService) : ControllerBase
    {

        /// <summary>
        /// add user controller
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetailRes>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAsync([FromBody] CreateUserReq user)
        {
            var command = new CreateUserCommand(user);
            var userId = await sender.Send(command);

            // retrurn full dto
            var queryUser = new GetUserByIdQuery(userId);
            var result = await sender.Send(queryUser);

            return Ok(
                ApiResponse<UserDetailRes>.SuccessResponse(result!, "User created successfully")
            );
        }

        /// <summary>
        /// get user by id controller
        /// </summary>
        /// <param name="id"> Id user </param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await sender.Send(query);

            return Ok(ApiResponse<UserDetailRes>.SuccessResponse(result, "User fetched successfully"));
        }

        /// <summary>
        /// Get all user controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDetailRes>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var query = new GetAllUserQuery();
            var result = await sender.Send(query);
            return Ok(ApiResponse<IEnumerable<UserDetailRes>>.SuccessResponse(result, "Users fetched successfully"));
        }

        /// <summary>
        /// Delete user controller
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            var command = new DeleteUserCommand(id);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "User deleted successfully"));
        }

        /// <summary>
        /// remove range user
        /// </summary>
        /// <param name="lstId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRangeUserAsync([FromBody] List<Guid> lstId)
        {
            var command = new RemoveRangeUserCommand(lstId);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "User deleted successfully"));
        }

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Manager}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserReq user)
        {
            var command = new UpdateUserCommand(id, user);
            var result = await sender.Send(command);
            return NoContent();
        }

        /// <summary>
        /// send verify email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("send-verify-email")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendVerifyEmail()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await emailService.SendVerifyEmail(email!);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "If the email is registered, please check verify email in email"));
        }

        /// <summary>
        /// update verify email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("verify-email/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromRoute] string token)
        {
            var command = new VerifyEmailCommand(token);
            var result = await sender.Send(command);

            var successHtml = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Email Verified</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Arial;
                            background-color: #f0f2f5;
                            text-align: center;
                            margin-top: 100px;
                        }}
                        .card {{
                            display: inline-block;
                            padding: 40px;
                            background: #fff;
                            border-radius: 12px;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                        }}
                        h1 {{ color: #28a745; }}
                        p {{ color: #555; }}
                        a {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 12px 24px;
                            color: white;
                            background-color: #4f46e5;
                            border-radius: 8px;
                            text-decoration: none;
                            font-weight: 600;
                        }}
                        a:hover {{
                            background-color: #4338ca;
                        }}
                    </style>
                </head>
                <body>
                    <div class='card'>
                        <h1>Email Verified Successfully!</h1>
                        <p>Thank you, <strong>{result}</strong> has been verified successfully.</p>
                        <a href='https://yourfrontend.com/login'>Go to Login</a>
                    </div>
                </body>
                </html>";

            return Content(successHtml, "text/html");

        }
    }
}
