using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Domain.Entities;
using Moq;
using AutoMapper;
using FluentAssertions;

namespace GeminiAdvancedAPI.Tests.Handlers
{
    public class CreateProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateProduct()
        {
            // Arrange (Düzenleme)
            //var mockUnitOfWork = new Mock<IUnitOfWork>(); // Artık IUnitOfWork kullanmıyoruz
            var mockProductRepository = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();

            // Mock ProductRepository'nin AddAsync metodunun davranışını ayarlayın.
            mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) =>
                {
                    product.Id = Guid.NewGuid(); // Yeni bir GUID atayın
                    return product;
                });

            // IProductRepository Setup (GetAll için - eğer kullanılıyorsa)
            // mockProductRepository.Setup(repo => repo.GetAll()).Returns(new List<Product>().AsQueryable()); //Eğer GetAll kullanılıyorsa


            mockProductRepository.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // SaveChangesAsync için

            // AutoMapper'ın Map metodunun davranışını ayarlayın.
            mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>()))
                .Returns((CreateProductCommand source) => new Product { Name = source.Name, Description = source.Description, Price = source.Price, Stock = source.Stock }); //Basit bir map işlemi

            //var handler = new CreateProductCommandHandler(mockUnitOfWork.Object, mockMapper.Object); // Artık IUnitOfWork kullanmıyoruz.
            var handler = new CreateProductCommandHandler(mockProductRepository.Object, mockMapper.Object);  // IProductRepository kullanıyoruz.
            var command = new CreateProductCommand("Test Product", "Description", 10.99m, 100);

            // Act (Eylem)
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert (Doğrulama)
            result.Should().NotBeEmpty(); //FluentAssertion
            mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once); //Repository çağrılmış mı kontrolü
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); // SaveChangesAsync çağrılmış mı?
        }
    }
}
/*
    Arrange: Test için gerekli ortamı hazırlama (nesneleri oluşturma, bağımlılıkları mock'lama vb.).
    Act: Test edilen metodu çağırma.
    Assert: Sonucun beklendiği gibi olup olmadığını kontrol etme.
 */
