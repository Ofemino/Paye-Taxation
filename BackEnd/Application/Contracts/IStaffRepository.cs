using Paye.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Paye.Application.Contracts
{
    public interface IStaffRepository
    {
        Task<Staff?> GetByIdAsync(Guid id);
        Task AddAsync(Staff staff);
    }
}
