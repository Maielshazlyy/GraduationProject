using Domain_layer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Report
    {
        public string Id { get; set; }
        public string ReportId { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public ReportType ReportType { get; set; }

        public DateTime GeneratedAt { get; set; }

        public string FilePath { get; set; }

        public string BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
