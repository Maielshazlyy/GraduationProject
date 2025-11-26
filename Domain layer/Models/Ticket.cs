using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Ticket
    {




        //business relations
        public int BusinessIdFk { get; set; }
        public Business Business { get; set; }

        //user relations
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

    }
}
