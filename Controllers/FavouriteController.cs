using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Favourite;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        private readonly IArtworkService _artworkService;
        private readonly IAuthService _authService;
        private readonly ILogService _logService;
        private readonly ApplicationDbContext _context;

        public FavouriteController(IFavouriteService favouriteService, IAuthService authService, ILogService logService, ApplicationDbContext context, IArtworkService artworkService)
        {
            _favouriteService = favouriteService;
            _authService = authService;
            _logService = logService;
            _context = context;
            _artworkService = artworkService;
        }

        [HttpPost]
        [Route("add-favourite")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<AddFavourite>> AddFavourite(long artwork_Id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var result = await _favouriteService.AddToFavourite(userId, artwork_Id);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpDelete]
        [Route("remove-artwork")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> RemoveArtwork(long favourite_Id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string user_Id = await _authService.GetCurrentUserId(userName);
                var result = _favouriteService.RemoveArtwork(favourite_Id, user_Id);
                if (result == 0)
                {
                    return BadRequest(new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "You can not remove this artwork"
                    });
                }
                else
                {
                    await _logService.SaveNewLog(userName, "Remove Artwork from your Favourite Successfully");
                    return Ok(new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Remove Artwork from your favourite successfully"
                    });
                }
            }
            catch
            {
                return BadRequest("Remove Artwork from your favourite failed");
            }
        }

        [HttpGet]
        [Route("get-favourite")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetFavourite()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string user_Id = await _authService.GetCurrentUserId(userName);
                var favourites = _favouriteService.GetFavouritesByUserId(user_Id);
                if (favourites == null)
                {
                    return NotFound(new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "You do not have Artwork in your favourite"
                    });
                }
                else
                {
                    return Ok(favourites);
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
