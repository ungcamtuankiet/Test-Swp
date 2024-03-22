using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
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

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<CategoryDto> GetById(long id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category is not null)
            {
                var categoryDto = new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedAt = category.CreatedAt,
                    UpdateAt = category.UpdatedAt,
                    IsActive = category.IsActive,
                    IsDeleted = category.IsDeleted,
                };
                return categoryDto;
            }
            return null;
        }

        public async Task CreateCategory(CreateCategory createCategory)
        {
            var newCategory = new Category()
            {
                Name = createCategory.Name
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<GeneralServiceResponseDto> Delete(long id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Category not found"
                };
            }
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Delete Category Successfully"
            };
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
