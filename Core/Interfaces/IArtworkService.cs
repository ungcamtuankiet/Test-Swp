using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Dtos.Artwork;
using System.Security.Claims;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IArtworkService
    {
        Task<IEnumerable<ArtworkDto>> GetAll();
        Task<IEnumerable<Artwork>> GetArtworkForAdmin(string? getBy);
        Task<IEnumerable<Artwork>> SearchArtwork(string? search, string? searchBy, double? from, double? to, string? sortBy);
        Task<ArtworkDto> GetById(long id);
        Task<GeneralServiceResponseDto> Delete(long id);
        Task<GeneralServiceResponseDto> DeleteSelectedArtworks(List<long> selectedIds);
        Task<IEnumerable<GetArtworkByUserId>> GetArtworkByUserId(string user_Id);
        Task CreateArtwork(CreateArtwork artworkDto, string user_Id, string user_Name);
        Task<GeneralServiceResponseDto> AcceptArtwork(long id);
        Task<GeneralServiceResponseDto> RefuseArtwork(long id, RefuseArtwork refuseArtwork);
        Task UpdateArtwork(long id, UpdateArtwork updateArtwork);
        
    }
}
