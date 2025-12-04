using MediatR;
using MyProject.Application.Interface.Data;
using MyProject.Core.Entities;
using MyProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Notification.Create
{
    public record RegisterDeviceTokenCommand : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public string DeviceInfo { get; set; } = string.Empty;

        [JsonIgnore]
        public Guid UserId { get; set; }

    };
    public class RegisterDeviceTokenCommandHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<RegisterDeviceTokenCommand, bool>
    {
        public async Task<bool> Handle(RegisterDeviceTokenCommand request, CancellationToken cancellationToken)
        {
            var exitToken = await unitOfWork.DeviceTokenRepository.GetByTokenAsync(request.Token);

            if ( exitToken != null)
            {
                await unitOfWork.DeviceTokenRepository.UpdateLastUsedAsync(request.Token);
                return true;
            }

            var deviceToken = new DeviceToken()
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Token = request.Token,
                DeviceType = request.DeviceType,
                DeviceInfo = request.DeviceInfo,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                Status = true
            };

            await unitOfWork.DeviceTokenRepository.AddAsync(deviceToken);
            await unitOfWork.CommitAsync();
            return true;
        }
    }
}
