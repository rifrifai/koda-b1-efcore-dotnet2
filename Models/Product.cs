using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
	public sealed class Product(string productCode, string name, decimal price, int stock, string category)
	{
		// public Guid ProductID { get; }
		public string ProductCode { get; private set; } = productCode;
		public string Name { get; private set; } = name;
		public decimal Price { get; private set; } = price;
		public int Stock { get; private set; } = stock;
		public string Category { get; private set; } = category;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

		public string GetInfo()
		{
			return $"Product Code: {ProductCode}, Name: {Name}, Price: {Price}, Stock: {Stock}, Category: {Category}";
		}

		public void UpdateDetail(string name, decimal price, int stock, string category)
		{
			var validated = new Product(productCode, name, price, stock, category);

			Name = validated.Name;
			Price = validated.Price;
			Stock = validated.Stock;
			Category = validated.Category;
		}
	}
}