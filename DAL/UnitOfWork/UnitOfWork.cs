using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;

using Domain_layer.Interfaces;

namespace DAL.UnitOfWork
{
  public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IBusinessRepository Businesses { get; }
        public UnitOfWork(AppDbContext context,IBusinessRepository businessRepository)
        {
            _context = context;
            Businesses = businessRepository;
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
