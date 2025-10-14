using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.API.Common;
using MyProject.Application.Features.Auth.Command.FogotPassword;
using MyProject.Application.Features.Auth.Command.Loggin;
using MyProject.Application.Features.Auth.Command.Login;
using MyProject.Application.Features.Auth.Command.Register;
using MyProject.Application.Features.Auth.DTO;
using MyProject.Application.Features.User.DTO;
using MyProject.Application.Features.User.Queries;
using MyProject.Application.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController(
        ISender sender,
        IRedisService redisService) : ControllerBase
    {
        /// <summary>
        /// login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(LoginRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginReq login)
        {
            var command = new AccessTokenCommand(login);
            var result = await sender.Send(command);
            return Ok(ApiResponse<LoginRes>.SuccessResponse(result, "Login successfully"));
        }

        /// <summary>
        /// register user
        /// </summary>
        /// <param name="registerReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterReq registerReq)
        {
            var command = new RegisterCommand(registerReq);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Register successfully"));
        }

        /// <summary>
        /// get current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserDetailRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetUserByIdQuery(Guid.Parse(userId));
            var user = await sender.Send(query);
            return Ok(ApiResponse<UserDetailRes>.SuccessResponse(user, "Successfully"));
        }

        /// <summary>
        /// Logout (client-side token removal)
        /// </summary>
        [HttpPost]
        [Route("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            // get token from header
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("No token provided"));
            }

            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);
                var exp = jwtToken.ValidTo;
                var timeToExpire = exp - DateTime.UtcNow;

                // if token is not expired, add it to blacklist
                if (timeToExpire.TotalSeconds > 0)
                {
                    await redisService.BlackListTokenAsync(token, timeToExpire);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.ErrorResponse("An error occurred during logout"));
            }

            return NoContent();
        }

        /// <summary>
        /// refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(typeof(LoginRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var command = new RefreshTokenCommand(refreshToken);
            var result = await sender.Send(command);
            return Ok(ApiResponse<LoginRes>.SuccessResponse(result, "Refresh token successfully"));
        }

        /// <summary>
        /// sent otp to email for reset password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var command = new SendOtpCommand(email);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "If the email is registered, please check otp in email"));
        }

        /// <summary>
        /// verify otp
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-otp")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpReq req)
        {
            var command = new VerifyOtpCommand(req);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Otp verify successfully"));
        }

        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReq req)
        {
            var command = new ResetPasswordCommand(req);
            var result = await sender.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Reset password successfully"));
        }
    }
}
