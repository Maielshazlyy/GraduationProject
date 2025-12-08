using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Report;

namespace Service_layer.Mapping
{
    public static class ReportMapping
    {
        public static ReportResponseDTO ToDto(this Report r)
        {
            if (r == null) return null;

            return new ReportResponseDTO
            {
                Id = r.Id,
                ReportId = r.ReportId,
                Title = r.Title,
                ReportType = r.ReportType.ToString(),
                GeneratedAt = r.GeneratedAt,
                FilePath = r.FilePath,

                BusinessId = r.BusinessId,
                BusinessName = r.Business?.Name ?? string.Empty
            };
        }

        // Optional for list mapping
        public static IEnumerable<ReportResponseDTO> ToDtoList(this IEnumerable<Report> reports)
        {
            return reports.Select(r => r.ToDto()).ToList();
        }
    }
}
