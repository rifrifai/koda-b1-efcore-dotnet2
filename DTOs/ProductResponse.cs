using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs;

public sealed record ProductResponse(string ProductCode, string Name, decimal Price, int Stock, string Category, bool IsActive, DateTime? DeactivateAtUtc)
{
    public static ProductResponse FromModel(Product product) => new(
        product.ProductCode, 
        product.Name, 
        product.Price, 
        product.Stock, 
        product.Category, 
        product.IsActive, 
        product.DeactivatedAtUtc);
}