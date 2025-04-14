using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FineController : ControllerBase
    {

        private readonly IFineService _fineService;

        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FineCreateDto fineCreateDto)
        {
            var result = await _fineService.CreateAsync(fineCreateDto);
            return Ok(result);
        }



    }
}
