using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.Artwork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace be_artwork_sharing_platform.Core.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ArtworkService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<ArtworkDto>> GetAll()
        {
            var artworks = await _context.Artworks.Where(a => a.IsActive == true && a.IsDeleted == false)
                .Select(a => new ArtworkDto
                {
                    Id = a.Id,
                    User_Id = a.User_Id,
                    Nick_Name = a.Nick_Name,
                    Category_Name = a.Category_Name,
                    Name = a.Name,
                    Description = a.Description,
                    Url_Image = a.Url_Image,
                    Price = a.Price,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsActive = a.IsActive,
                    IsDeleted = a.IsDeleted,
                }).ToListAsync();
            return artworks;
        }

        public async Task<IEnumerable<Artwork>> SearchArtwork(string? search, string? searchBy, double? from, double? to, string? sortBy)
        {
            var artworks = _context.Artworks.Include(a => a.Category).AsQueryable();
            artworks = artworks.Where(a => a.IsActive == true && a.IsDeleted == false);
            #region Filter
            if (searchBy is null)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    artworks = artworks.Where(a => a.Name.Contains(search));
                }
            }
            if (searchBy is not null)
            {
                if (searchBy.Equals("category_name"))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        artworks = artworks.Where(a => a.Category_Name.Contains(search));
                    }
                }
                else if (searchBy.Equals("nick_name"))
                    if (!string.IsNullOrEmpty(search))
                    {
                        artworks = artworks.Where(a => a.Nick_Name.Contains(search));
                    }
            }
            if (from.HasValue)
            {
                artworks = artworks.Where(a => a.Price >= from);
            }
            if (to.HasValue)
            {
                artworks = artworks.Where(a => a.Price <= to);
            }
            #endregion

            #region Sorting
            artworks = artworks.OrderBy(a => a.Name);

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price_asc":
                        artworks = artworks.OrderBy(a => a.Price);
                        break;
                    case "price_desc":
                        artworks = artworks.OrderByDescending(a => a.Price);
                        break;
                }
            }
            #endregion
            return artworks.ToList();
        }

        public async Task<IEnumerable<Artwork>> GetArtworkForAdmin(string? getBy)
        {
            var artworks = _context.Artworks.Include(a => a.Category).AsQueryable();
            #region Filter 
            if (getBy == null)
                artworks = artworks.OrderByDescending(a => a.CreatedAt);
            else
            {
                if (getBy.Equals("is_active_false"))
                {
                    artworks = artworks.Where(a => a.IsActive == false && a.IsDeleted == false);
                }
                else if (getBy.Equals("is_active_true"))
                {
                    artworks = artworks.Where(a => a.IsActive == true && a.IsDeleted == false);
                }
                else if (getBy.Equals("is_delete_true"))
                {
                    artworks = artworks.Where(a => a.IsDeleted == true && a.IsActive == false);
                }
            }
            #endregion
            return artworks.ToList();
        }

        public async Task<IEnumerable<GetArtworkByUserId>> GetArtworkByUserId(string user_Id)
        {
            var artworks = _context.Artworks.Where(a => a.User_Id == user_Id)
                .Select(a => new GetArtworkByUserId
                {
                    Id = a.Id,
                    User_Id = a.User_Id,
                    Nick_Name = a.Nick_Name,
                    Category_Name = a.Category_Name,
                    Name = a.Name,
                    Description = a.Description,
                    Url_Image = a.Url_Image,
                    Price = a.Price,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsActive = a.IsActive,
                    IsDeleted = a.IsDeleted,
                    ReasonRefuse = a.ReasonRefuse

                }).ToList()
                .OrderBy(a => a.CreatedAt);
            return artworks;
        }

        public async Task AcceptArtwork(long id, AcceptArtwork acceptArtwork)
        {
            var accept = await _context.Artworks.FirstOrDefaultAsync(a => a.Id == id);
            if (accept is not null)
            {
                accept.IsActive = acceptArtwork.IsActive;
                accept.ReasonRefuse = "Processed by Admin";
            }
            _context.Update(accept);
            _context.SaveChanges();
        }

        public async Task RefuseArtwork(long id, RefuseArtwork refuseArtwork)
        {
            var refuse = await _context.Artworks.FirstOrDefaultAsync(a => a.Id == id);
            if(refuse is not null)
            {
                refuse.IsDeleted = true;
                refuse.ReasonRefuse = refuseArtwork.Reason;
            }
            _context.Update(refuse);
            _context.SaveChanges();
        }

        public async Task<Artwork> GetById(long id)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);
            if(artwork != null)
            {
                var artworkDto = new ArtworkDto
                {
                    Id = artwork.Id,
                    User_Id = artwork.User_Id,
                    Nick_Name = artwork.Nick_Name,
                    Category_Name = artwork.Category_Name,
                    Name = artwork.Name,
                    Description = artwork.Description,
                    Url_Image = artwork.Url_Image,
                    Price = artwork.Price,
                    CreatedAt = artwork.CreatedAt,
                    UpdatedAt = artwork.UpdatedAt,
                    IsActive = artwork.IsActive,
                    IsDeleted = artwork.IsDeleted
                };
                return artwork;
            }
            else
            {
                return null;
            }
        }

        public async Task CreateArtwork(CreateArtwork artworkDto, string user_Id, string user_Name)
        {
            var artwork = new Artwork
            {
                User_Id = user_Id,
                Nick_Name = user_Name,
                Category_Name = artworkDto.Category_Name,
                Name = artworkDto.Name,
                Description = artworkDto.Description,
                Price = artworkDto.Price,
                Url_Image = artworkDto.Url_Image,
            };
            
            await _context.Artworks.AddAsync(artwork);
            await _context.SaveChangesAsync();
        }

        public int Delete(long id)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);
            _context.Remove(artwork);
            return _context.SaveChanges();
        }

        public async Task UpdateArtwork(long id, UpdateArtwork updateArtwork)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);
            if(artwork is not null)
            {
                artwork.Name = updateArtwork.Name;
                artwork.Category_Name = updateArtwork.Category_Name;
                artwork.Description = updateArtwork.Description;
                artwork.Url_Image = updateArtwork.Url_Image;
                artwork.Price = updateArtwork.Price;
                artwork.IsActive = false;
                artwork.ReasonRefuse = "Processing";
            }
            _context.Update(artwork);
            _context.SaveChanges();
        }

        public bool GetStatusIsActiveArtwork(long id)
        {
            var status = _context.Artworks.FirstOrDefault(b => b.Id == id);
            return status.IsActive;
        }

        public bool GetStatusIsDeleteArtwork(long id)
        {
            var status = _context.Artworks.FirstOrDefault(b => b.Id == id);
            return status.IsDeleted;
        }

        public int DeleteSelectedArtworks(List<long> selectedIds)
        {
            var artworksToDelete = _context.Artworks.Where(a => selectedIds.Contains(a.Id)).ToList();
            _context.RemoveRange(artworksToDelete);
            return _context.SaveChanges();
        }
    }
}
