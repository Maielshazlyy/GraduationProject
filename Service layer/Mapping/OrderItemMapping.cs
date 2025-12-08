using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Order;

namespace Service_layer.Mapping
{
    public static class OrderitemMapping
    {
        public static OrderItemDetailDTO ToDetailDto(this OrderItem i)
        {
            if (i == null) return null; // null optional
            return new OrderItemDetailDTO
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItem?.Name ?? string.Empty,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            };
        }

    }
}
