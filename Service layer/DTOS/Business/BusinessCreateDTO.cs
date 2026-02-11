using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Business
{
    public class BusinessCreateDTO
    {
        // Basic Information
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

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
        public bool HasDelivery { get; set; } = false;
        public bool HasTakeout { get; set; } = false;
        public bool HasParking { get; set; } = false;
        public bool HasWiFi { get; set; } = false;
        public bool HasOutdoorSeating { get; set; } = false;
        public bool AcceptsReservations { get; set; } = false;

        // Payment Methods (comma-separated: "Cash,Card,Mobile Payment")
        public string? PaymentMethods { get; set; }

        // Working Hours
        public List<WorkingHoursDTO> WorkingHours { get; set; } = new List<WorkingHoursDTO>();
    }

    public class WorkingHoursDTO
    {
        public int DayOfWeek { get; set; } // 0 = Sunday, 1 = Monday, ..., 6 = Saturday
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; } = false;
    }
}
