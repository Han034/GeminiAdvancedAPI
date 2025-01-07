﻿using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Persistence.Contexts;
using GeminiAdvancedAPI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;
		private bool disposed = false;

		public IProductRepository Products { get; private set; }
		// Diğer repository'ler buraya eklenecek (ör. public IOrderRepository Orders { get; private set; } )

		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			Products = new ProductRepository(_dbContext);
			// Diğer repository'ler burada initialize edilecek (ör. Orders = new OrderRepository(_dbContext); )
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}