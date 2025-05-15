using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FineController : ControllerBase
    {

        private readonly IFineService _fineService;

        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }


        [HttpPost]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> Create([FromBody] FineCreateDto fineCreateDto)
        {
            var result = await _fineService.CreateAsync(fineCreateDto);
            return Ok(result);
        }



    }
}
