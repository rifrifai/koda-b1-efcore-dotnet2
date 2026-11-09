using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Services;
public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync(bool activeOnly, CancellationToken ck);
    Task<ProductResponse?> GetByCodeAsync(string productCode, CancellationToken ck);
    Task<ProductResponse?> CreateAsync(ProductRequest request, CancellationToken ck);
    Task<ProductResponse?> UpdateAsync(string productCode, UpdateProductRequest request, CancellationToken ck);
    Task<bool> DeleteAsync(string productCode, CancellationToken ck);
    Task<ProductResponse?> ActivateAsync(string productCode, CancellationToken ct);
    Task<ProductResponse?> DeactivateAsync(string productCode, CancellationToken ct);
}