using API.DTOs;
using API.Models;
using API.Repository;

namespace API.Services;

public sealed class ProductService(IProductRepository repo) : IProductService
{
	private readonly IProductRepository _repo = repo;
	public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(bool activeOnly, CancellationToken ck)
	{
		var products = await _repo.GetAllAsync(activeOnly, ck);

		return products.Select(ProductResponse.FromModel).ToArray();
	}

	public async Task<ProductResponse?> CreateAsync(ProductRequest req, CancellationToken ck)
	{
		var product = new Product(req.ProductCode, req.Name, req.Price, req.Stock, req.Category, req.Description);
		var created = await _repo.TryAddAsync(product, ck);

		return created ? ProductResponse.FromModel(product) : null;
	}

	public Task<bool> DeleteAsync(string productCode, CancellationToken ck)
	{
		if (string.IsNullOrWhiteSpace(productCode) || productCode.Trim().Length > 30)
			return Task.FromResult(false);
		return _repo.TryDeleteAsync(productCode, ck);
	}

	public async Task<ProductResponse?> GetByCodeAsync(string productCode, CancellationToken ck)
	{
		if (string.IsNullOrWhiteSpace(productCode) || productCode.Trim().Length > 30)
			return null;
		var product = await _repo.GetByCodeAsync(productCode, ck);
		return product is null ? null : ProductResponse.FromModel(product);
	}


	public async Task<ProductResponse?> UpdateAsync(string productCode, UpdateProductRequest req, CancellationToken ck)
	{
		var product = new Product(productCode, req.Name, req.Price, req.Stock, req.Category, req.Description);
		var updated = await _repo.TryUpdateAsync(product, ck);
		if (!updated) return null;

		var current = await _repo.GetByCodeAsync(productCode, ck);
		return current is null ? null : ProductResponse.FromModel(current);
	}

    public Task<ProductResponse?> ActivateAsync(string productCode, CancellationToken ct)
	{
		return SetActiveAsync(productCode, true, ct);
	}

    public Task<ProductResponse?> DeactivateAsync(string productCode, CancellationToken ct)
	{
		return SetActiveAsync(productCode, false, ct);
	}

	private async Task<ProductResponse?> SetActiveAsync(string productCode, bool isActive, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(productCode) || productCode.Trim().Length > 30)
			return null;
		
		var product = await _repo.SetActiveAsync(productCode, isActive, ct);
		return product is null ? null : ProductResponse.FromModel(product);
	} 
}