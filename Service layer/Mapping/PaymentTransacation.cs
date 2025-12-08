using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.PaymentTranscation;

namespace Service_layer.Mapping
{
    public static class PaymentTransacation
    {
        public static PaymentTransacationResponseDTO ToDto(this PaymentTransaction p)
        {
            return new PaymentTransacationResponseDTO
            {
                Id = p.Id,
                PaymentId = p.PaymentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                TransactionDate = p.TransactionDate,
                Status = p.Status.ToString(),

                SubscriptionId = p.SubscriptionId,
            };
        }
    }
}
