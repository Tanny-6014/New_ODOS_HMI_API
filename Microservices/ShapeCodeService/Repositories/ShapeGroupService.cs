using ShapeCodeService.Context;
using ShapeCodeService.Interfaces;
using ShapeCodeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ShapeCodeService.Repositories
{
    public class ShapeGroupService : IShapeGroup
    {
        private ShapeCodeServiceContext _dbContext;

        public ShapeGroupService(ShapeCodeServiceContext dbContext)
        {
            _dbContext = dbContext;
        }
                
        public async Task<List<Shapegroup>> GetShapeGroupListAsync()
        {
           // return await _dbContext.Shapegroup.ToListAsync();
            return await _dbContext.Shapegroup.OrderByDescending(a => a.SG_IDENT).ToListAsync();
        }
        public async Task<bool> CheckduplicateShapeGroupAsync(string ShapeGroupName)
        {
            return await _dbContext.Shapegroup.Where(x => x.SG_CODE == ShapeGroupName).AnyAsync();

        }

        public async Task<Shapegroup> AddShapeGroupAsync(Shapegroup shapegroup)
        {
            try
            {
                if (shapegroup.SG_IDENT != 0)
                {
                    _dbContext.Shapegroup.Update(shapegroup);
                }
                else
                {

                    shapegroup.SG_IDENT =null;
                    _dbContext.Shapegroup.Add(shapegroup);
                }
                await _dbContext.SaveChangesAsync();
                return shapegroup;
            }
            catch (Exception e)
            {
                return shapegroup;

            }
           
        }

        public async Task<int> DeleteShapeGroupAsync(int id)
        {
            Shapegroup itemToRemove = await _dbContext.Shapegroup.SingleOrDefaultAsync(x => x.SG_IDENT == id); //returns a single item.
             _dbContext.Shapegroup.Remove(itemToRemove);
             return await _dbContext.SaveChangesAsync();
           
        }

    }
}
