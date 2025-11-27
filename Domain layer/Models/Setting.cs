using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Setting
    {





        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        
    }
}
