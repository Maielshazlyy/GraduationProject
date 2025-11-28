using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Setting
    {

        public int SettingId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }


        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        
    }
}
