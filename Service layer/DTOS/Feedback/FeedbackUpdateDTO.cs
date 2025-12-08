using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Feedback
{
    public class FeedbackUpdateDTO
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
