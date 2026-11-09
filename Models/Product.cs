using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models;
public sealed class Product(string productCode, string name, decimal price, int stock, string category, string? description, bool isActive = true)
{
	// public Guid ProductID { get; }
	public string ProductCode { get; private set; } = productCode;
	public string Name { get; private set; } = name;
	public decimal Price { get; private set; } = price;
	public int Stock { get; private set; } = stock;
	public string Category { get; private set; } = category;
	public string? Description { get; private set; } = description;
	public bool IsActive { get; private set; } = isActive;
	public DateTime? DeactivatedAtUtc { get; set; } = isActive ? null : DateTime.UtcNow;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	public void UpdateDetail(string name, decimal price, int stock, string category, string? description)
	{
		var validated = new Product(productCode, name, price, stock, category, description);

		Name = validated.Name;
		Price = validated.Price;
		Stock = validated.Stock;
		Category = validated.Category;
		Description = validated?.Description;
		IsActive = validated!.IsActive;
	}

	public void Activate()
	{
		if (IsActive) return;
		IsActive = true;
		DeactivatedAtUtc = null;
		UpdatedAt = DateTime.UtcNow;
	}

	public void Deactivate()
	{
		if (!IsActive) return;
		IsActive = false;
		DeactivatedAtUtc = DateTime.UtcNow;
		UpdatedAt = DateTime.UtcNow;
	}
}
	
