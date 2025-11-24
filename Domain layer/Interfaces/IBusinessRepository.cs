using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
  public  interface IBusinessRepository:IRepository<Business>
    {
        Task<Business?> GetBusinessWithDetails(int id);
    }
}
