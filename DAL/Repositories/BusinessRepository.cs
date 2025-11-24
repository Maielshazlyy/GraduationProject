using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;

using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
  public class BusinessRepository:Repository<Business>,IBusinessRepository
    {
        private readonly AppDbContext _context;
        public BusinessRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Business?> GetBusinessWithDetails(int id)
        {
            return await _context.Businesses
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
