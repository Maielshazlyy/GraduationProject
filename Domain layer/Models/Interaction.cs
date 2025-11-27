using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Interaction
    {








        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        //user realtions
        public int? HandledByUserId { get; set; }
        public User? HandledByUser { get; set; }
        //customer relations
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }


    }
}
