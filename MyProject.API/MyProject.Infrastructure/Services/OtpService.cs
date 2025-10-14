using MyProject.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Infrastructure.Service
{
    public class OtpService(
        IRedisService redisService
        ) : IOtpService
    {
        private readonly TimeSpan expiryMinutes = TimeSpan.FromMinutes(5);

        public async Task<string> GenerateOtpAsync(string email)
        {
            // crate a random 6-digit OTP best securely
            var otpBytes = new byte[4];
            RandomNumberGenerator.Fill(otpBytes);
            int otpValue = BitConverter.ToInt32(otpBytes, 0) % 1000000;
            var otp = Math.Abs(otpValue).ToString("D6");
            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}"));
            await redisService.SetAsync(key, otp, expiryMinutes);

            return otp;
        }

        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}"));
            var storedOtp = await redisService.GetAsync(key);
            if (string.IsNullOrEmpty(storedOtp))
            {
                throw new InvalidOperationException("OTP expired or not found.");
            }
            if (!storedOtp.Equals(otp))
            {
                throw new InvalidOperationException("Invalid OTP.");
            }
            await redisService.DeleteAsync(key);
            key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:verified"));
            await redisService.SetAsync(key, otp, expiryMinutes);
            return true;
        }
    }
}
