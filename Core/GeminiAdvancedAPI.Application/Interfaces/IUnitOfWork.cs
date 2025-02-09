using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        // Diğer repository'ler buraya eklenecek (ör. IOrderRepository Orders { get; } )

        Task<int> SaveChangesAsync();
	}
}
