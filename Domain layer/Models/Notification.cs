using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Notification
    {



        //business relations
        public int BusinessIdFk { get; set; }
        public Business Business { get; set; }

        //user relations
        public int UserIdFk { get; set; }
        public User User { get; set; }

    }
}
