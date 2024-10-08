﻿
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services;

public class SellerService 
{
    private readonly SalesWebMvcContext _context;

    public SellerService(SalesWebMvcContext context)
    {
        _context = context;
    }

    public async Task<List<Seller>> FindAllAsync()
    {
        return await _context.Seller.ToListAsync();
    }

    public async Task InsertAsync(Seller obj)
    {
        //obj.Department = _context.Department.First();
        _context.Add(obj);
        await _context.SaveChangesAsync();
    }

    public async Task<Seller> FindByIdAsync(int id)
    {
        return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
    }

    public async Task RemoveAsync(int id)
    {
        try
        {
            var obj = _context.Seller.Find(id);
            _context.Remove(obj);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException err)
        {
            throw new IntegrityException(err.Message);
        }

    }

    public async Task UpdateAsync(Seller obj)
    {
        if (!await _context.Seller.AnyAsync(x => x.Id == obj.Id))
        {
            throw new NotFoundException("Id not found");
        }
        try
        {
            _context.Update(obj);
           await _context.SaveChangesAsync();
        } 
        catch (DbConcurrencyException err)
        {
            throw new DbConcurrencyException(err.Message);
        }
   
    }
}
