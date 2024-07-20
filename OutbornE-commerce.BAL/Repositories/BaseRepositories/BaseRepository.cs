using Microsoft.EntityFrameworkCore;
using OutbornE_commerce.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Repositories.BaseRepositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<T>> FindAll(bool trackChanges)
		=> trackChanges ?
		   await _context.Set<T>().ToListAsync() : await _context.Set<T>().AsNoTracking().ToListAsync();

		public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
		=> !trackChanges ?
			await _context.Set<T>().Where(expression).AsNoTracking().ToListAsync()
			: await _context.Set<T>().Where(expression).AsTracking(QueryTrackingBehavior.TrackAll).ToListAsync();
		public async Task<T?> Find(Expression<Func<T, bool>> expression, bool trackChanges)
		=> !trackChanges ?
			await _context.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync()
			: await _context.Set<T>().Where(expression).AsTracking(QueryTrackingBehavior.TrackAll).FirstOrDefaultAsync();
		public async Task<T> Create(T entity)
		{
			var result = await _context.Set<T>().AddAsync(entity);
			return result.Entity;
		}
		public void Delete(T entity) => _context.Set<T>().Remove(entity);
		public void Update(T entity) => _context.Set<T>().Update(entity);

		public async Task SaveAsync(CancellationToken cancellationToken)
		{
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
