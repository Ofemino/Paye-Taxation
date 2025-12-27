using Paye.Application.Contracts;
using Paye.Infrastructure.Persistence;
using Paye.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Paye.Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly PayeDbContext _db;
        public StaffRepository(PayeDbContext db)
        {
            _db = db;
        }

        public async Task<Staff?> GetByIdAsync(Guid id)
        {
            return await _db.Staffs.Include(s => s.Submissions).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Staff staff)
        {
            _db.Staffs.Add(staff);
            await _db.SaveChangesAsync();
        }
    }
}
