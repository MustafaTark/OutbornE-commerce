﻿using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using OutbornE_commerce.BAL.Dto;
using System;
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
		Task<PagainationModel<IEnumerable<T>>> FindAllAsyncByPagination(Expression<Func<T, bool>>? criteria = null, int pageNumber = 1, int pageSize = 10, string[] includes = null);
        //Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> criteria, string[] includes = null);

        Task<T?> Find(Expression<Func<T, bool>> expression, bool trackChanges = false, string[] includes = null);
        Task<T?> FindIncludesSplited(Expression<Func<T, bool>> expression, bool trackChanges = false, string[] includes = null);

        Task<T> Create(T entity);
		Task CreateRange(List<T> entities);

        void Delete(T entity);
		Task DeleteRange(Expression<Func<T, bool>> expression);
		void UpdateRange(List<T> entities);
        Task<int> ExecuteUpdate(Expression<Func<T, bool>> criteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
        void Update(T entity);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
        Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken);
	}
}
