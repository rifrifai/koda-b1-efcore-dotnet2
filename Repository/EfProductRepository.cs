using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class EfProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(bool activeOnly, CancellationToken ck)
    {
        IQueryable<Product> query = db.Products.AsNoTracking();
        if (activeOnly) query = query.Where(p => p.IsActive);

        return await query.OrderBy(p => p.ProductCode).ToListAsync(ck);
    }

    public async Task<Product?> GetByCodeAsync(string productCode, CancellationToken ck)
    {
        var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductCode == productCode, ck);
        return product;
    }

    public async Task<Product?> SetActiveAsync(string productCode, bool isActive, CancellationToken ct)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == productCode, ct);
        if (product is null) return null;

        if(isActive)
            product.Activate();
        else
            product.Deactivate();
        
        await db.SaveChangesAsync(ct);
        return product;
    }

    public async Task<bool> TryAddAsync(Product product, CancellationToken ck)
    {
        db.Products.Add(product);
        try
        {
            await db.SaveChangesAsync(ck);
            return true;
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException {Number: 2601 or 2627})
        {
            return false;
        }
    }

    public async Task<bool> TryDeleteAsync(string productCode, CancellationToken ck)
    {
        var code = productCode.Trim().ToUpperInvariant();
        var existing = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == code, ck);
        if (existing is null) return false;

        db.Products.Remove(existing);
        await db.SaveChangesAsync(ck);
        return true;

    }

    public async Task<bool> TryUpdateAsync(Product product, CancellationToken ck)
    {
        var existing = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == product.ProductCode, ck);
        if (existing is null) return false;

        existing.UpdateDetail(product.Name, product.Price, product.Stock, product.Category, product.Description);

        await db.SaveChangesAsync(ck);

        return true;
    }
}