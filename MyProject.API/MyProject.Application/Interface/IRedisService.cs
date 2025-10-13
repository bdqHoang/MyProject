using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface
{
    public interface IRedisService
    {
        /// <summary>
        /// add token to black list
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task BlackListTokenAsync(string token, TimeSpan expiration);

        /// <summary>
        /// check token is blacklist
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> IsTokenBlackListAsync(string token);

        /// <summary>
        /// remove token from black list
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task RemoveTokenFromBlackList(string token);

        /// <summary>
        /// set string value witj expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task SetAsync(string key, string value, TimeSpan? expiration = null);

        /// <summary>
        /// get string value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// delete string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key);

        /// <summary>
        /// check if key exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string key);

    }
}
