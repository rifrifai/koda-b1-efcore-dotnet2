using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using API.Data;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public sealed class ProductController(IProductService service) : ControllerBase
	{
		private readonly IProductService _service = service;

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll([FromQuery] bool activeOnly, CancellationToken ck)
		{
			var products = await _service.GetAllAsync(activeOnly, ck);
			return Ok(products);
		}

		[HttpGet("{productCode}")]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductResponse>> GetByCode([FromRoute] string productCode, CancellationToken ck)
		{
			var product = await _service.GetByCodeAsync(productCode, ck);

			return product is not null ? Ok(product) : Problem(
					statusCode: StatusCodes.Status404NotFound,
					title: "Product does not exist"
				);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequest request, CancellationToken ck)
		{
			try
			{
				var created = await _service.CreateAsync(request, ck);

				if (created is null)
				{
					return Problem(
						statusCode: StatusCodes.Status409Conflict,
						title: "Product code has already exist"
					);
				}
				return CreatedAtAction(
					nameof(GetByCode),
					new { productCode =  created.ProductCode },
					created
				);
			}
			catch (ArgumentException e)
			{
				return Problem(
					statusCode: StatusCodes.Status400BadRequest,
					title: "Invalid Product Data",
					detail: e.Message
				);
			}
		}

		[HttpDelete("{productCode}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromRoute] string productCode, CancellationToken ck)
		{
			var deleted = await _service.DeleteAsync(productCode, ck);
		
			return deleted ? NoContent() : Problem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Product not found!",
				detail: $"No product was found for code {productCode}."
			);
		}

		[HttpPut("{productCode}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductResponse>> Update(
		[FromRoute] string productCode, [FromBody] UpdateProductRequest request, CancellationToken ck)
		{
			try
			{
				var existingProduct = await _service.UpdateAsync(productCode, request, ck);

				return existingProduct is null ? 
				Problem(
					statusCode: StatusCodes.Status404NotFound, 
					title: "Product not found", 
					detail: $"No product was found for ID {productCode}.") : Ok(existingProduct);
			}
			catch (ArgumentException e)
			{
				return Problem(
					statusCode: StatusCodes.Status400BadRequest,
					title: "Invalid product data!",
					detail: e.Message);
			}
		}

		[HttpPatch("{productCode}/activate")]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductResponse>> Activate([FromRoute] string productCode, CancellationToken ct)
		{
			var product = await _service.ActivateAsync(productCode, ct);

			return product is null ? Problem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Product not found",
				detail: $"No product was found for code {productCode}"
			) : Ok(product);
		}

		[HttpPatch("{productCode}/deactivate")]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductResponse>> Deactivate([FromRoute] string productCode, CancellationToken ct)
		{
			var product = await _service.DeactivateAsync(productCode, ct);

			return product is null ? Problem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Product not found",
				detail: $"No product was found for code {productCode}"
			) : Ok(product);
		}
	}
}