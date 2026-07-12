using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Business
{
    public class BusinessResponseDTO
    {
        public string Id { get; set; }
        public string BusinessId { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }

        // Contact Information
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }

        // Location
        public string? City { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Restaurant Information
        public string? Description { get; set; }
        public string? CuisineType { get; set; }
        public string? PriceRange { get; set; }
        public string? LogoUrl { get; set; }
        public string? CoverImageUrl { get; set; }

        // Features & Services
        public bool HasDelivery { get; set; }
        public bool HasTakeout { get; set; }
        public bool HasParking { get; set; }
        public bool HasWiFi { get; set; }
        public bool HasOutdoorSeating { get; set; }
        public bool AcceptsReservations { get; set; }

        // Payment Methods
        public string? PaymentMethods { get; set; }

        // Status
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }

        // Working Hours
        public List<WorkingHoursResponseDTO> WorkingHours { get; set; } = new List<WorkingHoursResponseDTO>();

        public DateTime CreatedAt { get; set; }

        // Optional statistics
        public int TotalUsers { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalTickets { get; set; }
    }

    public class WorkingHoursResponseDTO
    {
        public string WorkingHoursId { get; set; } = string.Empty;
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty; // "Sunday", "Monday", etc.
        public string? OpenTime { get; set; } // Format: "HH:mm" e.g., "09:00"
        public string? CloseTime { get; set; } // Format: "HH:mm" e.g., "22:00"
        public bool IsClosed { get; set; }
    }
}
