using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace be_artwork_sharing_platform.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(long id)
        {
            return _context.Categories.Find(id) ?? throw new Exception("Category not found");
        }

        public int CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges();
        }

        public int Delete(long id)
        {
            var category = _context.Categories.Find(id) ?? throw new Exception("Category not found");
            _context.Categories.Remove(category);
            return _context.SaveChanges();
        }

        public async Task<IEnumerable<string>> GetCategortNameListAsync()
        {
            var categortName = await _context.Categories
                .Select(q => q.Name)
                .ToListAsync();

            return categortName;
        }
    }
}
