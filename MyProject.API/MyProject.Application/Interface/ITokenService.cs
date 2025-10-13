using MyProject.Application.Features.User.DTO;

namespace MyProject.Application.Interface
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate JWT access token from UserDetailViewModel
        /// </summary>
        string GenerateAccessToken(UserDetailRes user);

        /// <summary>
        /// Generate refresh token
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// generate email validation token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GenerateValidateEmailToken(string email);

        /// <summary>
        /// validate email token and extract email
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        bool ValidateEmailToken(string token, out string email);
    }
}
