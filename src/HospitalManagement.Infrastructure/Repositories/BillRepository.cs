using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(HospitalDbContext context, ILogger<BillRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .Include(b => b.Patient)
                .Include(b => b.Doctor)
                .Where(b => b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bills");
            throw;
        }
    }

    public async Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .Include(b => b.Patient)
                .Include(b => b.Doctor)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill with id {Id}", id);
            throw;
        }
    }

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Bills.AddAsync(bill, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return bill;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding bill");
            throw;
        }
    }

    public async Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill with id {Id}", bill.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var bill = await _context.Bills.FindAsync(new object[] { id }, cancellationToken);
            if (bill != null)
            {
                bill.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bill with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills.AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if bill exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .Include(b => b.Doctor)
                .Where(b => b.PatientId == patientId && b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills for patient {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .Include(b => b.Patient)
                .Where(b => b.DoctorId == doctorId && b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills for doctor {DoctorId}", doctorId);
            throw;
        }
    }
}
