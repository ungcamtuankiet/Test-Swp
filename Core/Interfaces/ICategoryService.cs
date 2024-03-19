using be_artwork_sharing_platform.Core.Entities;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category GetById(long id);
        int CreateCategory(Category category);
        int Delete(long id);
        Task<IEnumerable<string>> GetCategortNameListAsync();
    }
}
