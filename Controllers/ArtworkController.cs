using AutoMapper;
using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_artwork_sharing_platform.Core.Services;
using be_project_swp.Core.Dtos.Artwork;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Web.Http.Results;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtworkService _artworkService;
        private readonly IAuthService _authService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ArtworkController(IArtworkService artworkService, IAuthService authService, ILogService logService, IMapper mapper, ICategoryService categoryService)
        {
            _artworkService = artworkService;
            _authService = authService;
            _logService = logService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var artworks = await _artworkService.GetAll();
            return Ok(artworks);
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(string? search, string? searchBy, double? from, double? to, string? sortBy)
        {
            var artworks = await _artworkService.SearchArtwork(search, searchBy, from, to, sortBy);
            return Ok(_mapper.Map<List<ArtworkDto>>(artworks));
        }

        [HttpGet]
        [Route("get-by-userId")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetByUserIdAsync()
        {
            string userName = HttpContext.User.Identity.Name;
            string userId = await _authService.GetCurrentUserId(userName);
            var artworks = await _artworkService.GetArtworkByUserId(userId);
            return Ok(artworks);

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute]long id)
        {
            try
            {
                var artwork = await _artworkService.GetById(id);
                if (artwork == null) return NotFound("Artwork not found");
                return Ok(artwork);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> Create(CreateArtwork artworkDto)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string userNickNameCurrent = await _authService.GetCurrentNickName(userName);
                await _artworkService.CreateArtwork(artworkDto, userId, userNickNameCurrent);
                await _logService.SaveNewLog(userName, "Create New Artwork");
                return Ok("Create new Artwork Successfully");
            }
            catch
            {
                return BadRequest("Create new Artwork Failed");
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<GeneralServiceResponseDto>> Delete(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var result = await _artworkService.Delete(id);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-by-id-select")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<GeneralServiceResponseDto>> Delete([FromBody] List<long> ids)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var deletedArtworks = await _artworkService.DeleteSelectedArtworks(ids);
                if(deletedArtworks is not null)
                {
                    await _logService.SaveNewLog(userName, $"Deleted {deletedArtworks} Artwork(s)");
                    return Ok(deletedArtworks);
                }
                else
                {
                    return NotFound(deletedArtworks);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update-artwork")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> UpdateArtwork(long id, UpdateArtwork artworkDt)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _logService.SaveNewLog(userName, "Update Artwork");
                await _artworkService.UpdateArtwork(id, artworkDt);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest("Update Failed");
            }
        }
    }
}
