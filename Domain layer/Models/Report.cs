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
        public int ReportId { get; set; }

        public string Title { get; set; }

        public ReportType ReportType { get; set; }

        public DateTime GeneratedAt { get; set; }

        public string FilePath { get; set; }

        public int BusinessIdFk { get; set; }
        public Business Business { get; set; }

    }
}
