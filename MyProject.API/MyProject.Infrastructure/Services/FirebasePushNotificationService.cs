using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyProject.Application.Interface.Data;
using MyProject.Application.Interface.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Infrastructure.Services
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FirebasePushNotificationService> _logger;
        private readonly string _serverKey;

        public FirebasePushNotificationService(
            IUnitOfWork unitOfWork,
            ILogger<FirebasePushNotificationService> logger,
            IConfiguration configuration
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serverKey = configuration["Firebase:ServerKey"]!;
        }


        public Task SendToDeviceAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            throw new NotImplementedException();
        }

        public Task SendToMuitipleDeviceAsync(List<string> deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            throw new NotImplementedException();
        }

        public Task SendToUserAsync(Guid userId, string title, string body, Dictionary<string, string> data = null)
        {
            throw new NotImplementedException();
        }
    }
}
