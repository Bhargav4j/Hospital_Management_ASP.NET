using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(ClinicDbContext context, ILogger<BillRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .AsNoTracking()
                .Include(b => b.Patient)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.BillDate)
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
                .AsNoTracking()
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill with ID {BillId}", id);
            throw;
        }
    }

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            bill.CreatedDate = DateTime.UtcNow;
            bill.IsActive = true;
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Bill added successfully with ID {BillId}", bill.Id);
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
            bill.ModifiedDate = DateTime.UtcNow;
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Bill updated successfully with ID {BillId}", bill.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill with ID {BillId}", bill.Id);
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
                bill.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Bill soft-deleted successfully with ID {BillId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bill with ID {BillId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if bill exists with ID {BillId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .AsNoTracking()
                .Include(b => b.Patient)
                .Where(b => b.PatientId == patientId && b.IsActive)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills for patient {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetUnpaidBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .AsNoTracking()
                .Include(b => b.Patient)
                .Where(b => !b.IsPaid && b.IsActive)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid bills");
            throw;
        }
    }
}
