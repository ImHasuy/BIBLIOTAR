using BiblioTar.DTOs;
using BiblioTar.Entities;
using BiblioTar.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }



        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] AddressCreateDto addressCreateDto)
        {
            var result = await _addressService.CreateAsyncAddress(addressCreateDto);
            return Ok();
        }

    }
}
