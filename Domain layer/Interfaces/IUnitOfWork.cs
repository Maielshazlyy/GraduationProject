using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Interfaces
{
    public interface IUnitOfWork
    {
        IBusinessRepository Businesses { get; }
        Task<int> CompleteAsync();
    }
}
