using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Business;

namespace Service_layer.Mapping
{
    public static class BusinessMapping
    {
        public static BusinessResponseDTO ToDto(this Business b)
        {
            var dayNames = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            return new BusinessResponseDTO
            {
                Id = b.Id,
                BusinessId = b.BusinessId,
                Name = b.Name,
                Type = b.Type,
                Address = b.Address,
                Phone = b.Phone,
                
                // Contact Information
                Email = b.Email,
                Website = b.Website,
                FacebookUrl = b.FacebookUrl,
                InstagramUrl = b.InstagramUrl,

                // Location
                City = b.City,
                Country = b.Country,
                Latitude = b.Latitude,
                Longitude = b.Longitude,

                // Restaurant Information
                Description = b.Description,
                CuisineType = b.CuisineType,
                PriceRange = b.PriceRange,
                LogoUrl = b.LogoUrl,
                CoverImageUrl = b.CoverImageUrl,

                // Features & Services
                HasDelivery = b.HasDelivery,
                HasTakeout = b.HasTakeout,
                HasParking = b.HasParking,
                HasWiFi = b.HasWiFi,
                HasOutdoorSeating = b.HasOutdoorSeating,
                AcceptsReservations = b.AcceptsReservations,

                // Payment Methods
                PaymentMethods = b.PaymentMethods,

                // Status
                IsActive = b.IsActive,
                IsVerified = b.IsVerified,

                // Working Hours
                WorkingHours = b.WorkingHours?.Select(wh => new WorkingHoursResponseDTO
                {
                    WorkingHoursId = wh.WorkingHoursId,
                    DayOfWeek = wh.DayOfWeek,
                    DayName = wh.DayOfWeek >= 0 && wh.DayOfWeek < 7 ? dayNames[wh.DayOfWeek] : "",
                    OpenTime = wh.OpenTime,
                    CloseTime = wh.CloseTime,
                    IsClosed = wh.IsClosed
                }).ToList() ?? new List<WorkingHoursResponseDTO>(),

                CreatedAt = b.CreatedAt,

                TotalUsers = b.Users?.Count ?? 0,
                TotalCustomers = b.Customers?.Count ?? 0,
                TotalTickets = b.Tickets?.Count ?? 0
            };
        }
        public static IEnumerable<BusinessResponseDTO> ToDtoList(this IEnumerable<Business> list)
       => list.Select(b => b.ToDto());
    }
}