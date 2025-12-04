using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Interface.Infrastructure
{
    public interface IPushNotificationService
    {
        Task SendToDeviceAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null!);
        Task SendToUserAsync(Guid userId, string title, string body, Dictionary<string, string> data = null!);
        Task SendToMuitipleDeviceAsync(List<string> deviceToken, string title, string body, Dictionary<string,string> data = null!);
    }
}
