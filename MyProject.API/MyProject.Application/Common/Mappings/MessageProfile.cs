using AutoMapper;
using MyProject.Application.Features.Message.DTO;
using MyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Common.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageRes, Messages>();
            CreateMap<Messages, MessageRes>();
            CreateMap<ConversationRes, Conversations>();
        }
    }
}
