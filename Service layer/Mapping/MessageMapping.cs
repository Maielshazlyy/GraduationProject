using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Message;

namespace Service_layer.Mapping
{
    public static class MessageMapping
    {
        public static MessageResponseDTO ToDto(this Message m)
        {
            return new MessageResponseDTO
            {
                MessageId = m.MessageId,
                SenderType = m.SenderType,
                Content = m.Content,
                SentAt = m.SentAt,

                InteractionId = m.InteractionId,
                AgentId = m.UserId,
                AgentName = m.User?.FullName ?? "",

                Sentiment = m.Sentiment?.ToDto()
            };
        }

        public static IEnumerable<MessageResponseDTO> ToDtoList(this IEnumerable<Message> list)
            => list.Select(m => m.ToDto());
    }
}
