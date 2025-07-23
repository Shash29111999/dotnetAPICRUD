using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.AppDataContext;
using TodoAPI.Services;
using TodoAPICS.Contracts;
using TodoAPICS.Interfaces;
using TodoAPICS.Models;

public class ProductServices : IProductServices
{
    private readonly TodoDbContext _context;
    private readonly ILogger<TodoServices> _logger;
    private readonly IMapper _mapper;

    public ProductServices(TodoDbContext context, ILogger<TodoServices> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
        try
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            throw;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return false;

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        try
        {
            var products = await _context.Product.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _context.Product.FindAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving product with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        try
        {
            var products = await _context.Product
                .Where(p => p.Category.ToLower() == category.ToLower())
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving products in category {category}");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            searchTerm = searchTerm?.ToLower() ?? "";

            var products = await _context.Product
                .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                            p.Description.ToLower().Contains(searchTerm) ||
                            p.Category.ToLower().Contains(searchTerm))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error searching products with term '{searchTerm}'");
            throw;
        }
    }

    public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
    {
        try
        {
            var existing = await _context.Product.FindAsync(id);
            if (existing == null) return null;

            _mapper.Map(productDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _context.Product.Update(existing);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(existing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating product with ID {id}");
            throw;
        }
    }
}
