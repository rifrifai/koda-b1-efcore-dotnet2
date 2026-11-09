using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public record class UpdateProductRequest(
    [Required(ErrorMessage = "Nama produk wajib diisi.")]
    [StringLength(120, ErrorMessage = "Nama produk maksimal 120 karakter.")]
    string Name,

    [Required(ErrorMessage = "Harga wajib diisi.")]
    [Range(typeof(decimal), "0", "99999999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Harga harus di antara 0 - 99,999,999,999.99.")]
    decimal Price, 
    
    [Required(ErrorMessage = "Stok wajib diisi.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok tidak boleh bernilai negatif.")]
    int Stock,

    [Required, StringLength(30)]
    string Category,

    string Description
);