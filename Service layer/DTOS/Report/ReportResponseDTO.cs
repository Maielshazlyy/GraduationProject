using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Report
{
    public class ReportResponseDTO
    {
        public string Id { get; set; }
        public string ReportId { get; set; }          // GUID for tracking externally
        public string Title { get; set; }

        public string ReportType { get; set; }         // Daily, Weekly, TicketSummary, Orders, Performance, AI Analytics...

        public DateTime GeneratedAt { get; set; }

        public string FilePath { get; set; }           // Path or URL to download report
        public string FileUrl { get; set; }            // Optional (API → Signed URL, Cloud)

        // Business relation summary
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }

        // Optional insights
        public string? Summary { get; set; }
    }
}
