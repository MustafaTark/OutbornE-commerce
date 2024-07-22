﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Repositories.BaseRepositories
{
	public interface IBaseRepository<T> where T : class
	{
        Task<IEnumerable<T>> FindAllAsync(string[] includes, bool withNoTracking = true);
		//Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
		Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> criteria, string[] includes = null);

        Task<T?> Find(Expression<Func<T, bool>> expression, bool trackChanges);
		Task<T> Create(T entity);
		void Delete(T entity);
		void Update(T entity);
		Task SaveAsync(CancellationToken cancellationToken);
	}
}
