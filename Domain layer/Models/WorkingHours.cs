using System;

namespace Domain_layer.Models
{
    public class WorkingHours
    {
        public string WorkingHoursId { get; set; }
        
        // Day of week (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
        public int DayOfWeek { get; set; } // 0-6
        
        public TimeSpan? OpenTime { get; set; } // e.g., 09:00
        public TimeSpan? CloseTime { get; set; } // e.g., 22:00
        public bool IsClosed { get; set; } = false; // إذا كان المطعم مغلق في هذا اليوم
        
        // Business relation
        public string BusinessId { get; set; }
        public Business Business { get; set; }
    }
}

