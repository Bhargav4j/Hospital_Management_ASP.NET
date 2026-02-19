using AutoMapper;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Generic base repository that bridges Domain entities and scaffolded DbModels.
/// Queries the database using TDbModel (which EF tracks), then maps to/from
/// TDomain (which the Application/Domain layers consume).
/// </summary>
public class Repository<TDomain, TDbModel> : IRepository<TDomain>
    where TDomain : class
    where TDbModel : class
{
    protected readonly ClinicManagementDbContext _context;
    protected readonly DbSet<TDbModel> _dbSet;
    protected readonly IMapper _mapper;

    public Repository(ClinicManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TDbModel>();
        _mapper = mapper;
    }

    public virtual async Task<TDomain?> GetByIdAsync(int id)
    {
        var dbModel = await _dbSet.FindAsync(id);
        return dbModel != null ? _mapper.Map<TDomain>(dbModel) : null;
    }

    public virtual async Task<IEnumerable<TDomain>> GetAllAsync()
    {
        var dbModels = await _dbSet.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<TDomain>>(dbModels);
    }

    public virtual async Task<TDomain> AddAsync(TDomain entity)
    {
        var dbModel = _mapper.Map<TDbModel>(entity);
        await _dbSet.AddAsync(dbModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<TDomain>(dbModel);
    }

    public virtual async Task UpdateAsync(TDomain entity)
    {
        var dbModel = _mapper.Map<TDbModel>(entity);

        foreach (var entry in _context.ChangeTracker.Entries<TDbModel>().ToList())
        {
            entry.State = EntityState.Detached;
        }

        _dbSet.Update(dbModel);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var dbModel = await _dbSet.FindAsync(id);
        if (dbModel != null)
        {
            _dbSet.Remove(dbModel);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        var dbModel = await _dbSet.FindAsync(id);
        return dbModel != null;
    }
}
