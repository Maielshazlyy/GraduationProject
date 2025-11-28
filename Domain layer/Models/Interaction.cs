using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Interaction
    {
        public int InteractionId { get; set; }

        //Customer realtions 
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }



        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        //user realtions
        public int? HandledByUserId { get; set; }
        public User? HandledByUser { get; set; }


        public string Channel { get; set; }

        public bool? IsEnded { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }


        public ICollection<Message> Messages { get; set; } = new List<Message>();
       
        }
    }
