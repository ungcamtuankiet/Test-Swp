using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<CategoryDto> GetById(long id);
        Task CreateCategory(CreateCategory createCategory);
        Task<GeneralServiceResponseDto> Delete(long id);
        Task<IEnumerable<string>> GetCategortNameListAsync();
    }
}
