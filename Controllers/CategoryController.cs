using AutoMapper;
using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ILogService _logService;
        private readonly IAuthService _authService;

        public CategoryController(IMapper mapper, ICategoryService categoryService, ILogService logService, IAuthService authService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _logService = logService;
            _authService = authService;
        }

        [HttpGet]
        [Route("get-all-category")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(_mapper.Map<List<CategoryDto>>(categories));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _categoryService.GetById(id);
                if (result == null) return NotFound("Category not found");
                return Ok(result);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategory createCategory)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                await _categoryService.CreateCategory(createCategory);
                await _logService.SaveNewLog(userName, "Create New Category");
                return Ok("Create Category Successfully");
            }
            catch
            {
                return BadRequest("Create Category Failed");
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<GeneralServiceResponseDto>> DeleteCategory(long id)
        {
            try
            {
                var result = await _categoryService.Delete(id);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("categorynames")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategoryNameList()
        {
            var categoryNames = await _categoryService.GetCategortNameListAsync();
            return Ok(categoryNames);
        }
    }
}
