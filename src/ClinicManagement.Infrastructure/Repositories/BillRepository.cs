using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Bill entity
/// </summary>
public class BillRepository : IBillRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(ClinicDbContext context, ILogger<BillRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all bills");
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
            _logger.LogInformation("Retrieving bill with ID {BillId}", id);
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

    public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bills for patient {PatientId}", patientId);
            return await _context.Bills
                .AsNoTracking()
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

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new bill for patient {PatientId}", bill.PatientId);
            bill.CreatedDate = DateTime.UtcNow;
            bill.IsActive = true;

            await _context.Bills.AddAsync(bill, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added bill with ID {BillId}", bill.Id);
            return bill;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding bill for patient {PatientId}", bill.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating bill with ID {BillId}", bill.Id);
            bill.ModifiedDate = DateTime.UtcNow;

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated bill with ID {BillId}", bill.Id);
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
            _logger.LogInformation("Deleting bill with ID {BillId}", id);
            var bill = await _context.Bills.FindAsync(new object[] { id }, cancellationToken);

            if (bill != null)
            {
                bill.IsActive = false;
                bill.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted bill with ID {BillId}", id);
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
            return await _context.Bills.AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of bill with ID {BillId}", id);
            throw;
        }
    }
}
