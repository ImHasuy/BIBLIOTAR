﻿using AutoMapper;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly IMapper _mapper;
        public BorrowController(IBorrowService borrowService, IMapper mapper)
        {
            _borrowService = borrowService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> CreateBorrowCont([FromBody] BorrowCreateDto borrowCreateDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _borrowService.CreateBorrow(borrowCreateDto);
                apiResponse.Data = response;
                apiResponse.Message = "Borrow created successfully";
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);
        }

    
        /// <summary>
        /// Egy borrow ID-ját várja ide is
        /// </summary>
        /// <param name="borrowExtendDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ExtendPeriod")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> ExtendPeriod([FromBody] BorrowExtendDto borrowExtendDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _borrowService.ExtendBorrowPeriod(borrowExtendDto);
                apiResponse.Message = response;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);

        }


        [HttpGet]
        [Route("GetAllBorrowForUser")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetallborrowForAUser()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var response = await _borrowService.GetAllBorrowForUser(new BorrowInputDto { Id = int.Parse(id)});
                apiResponse.Data = response;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);

        }

        [HttpGet]
        [Route("GetAllBorrow")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> Getallborrows()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _borrowService.GetAllBorrows();
                apiResponse.Data = response;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);

        }

        

        [HttpGet]
        [Route("GetAllBorrow/{borrowId}")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetallborrowbyuserId(int borrowId)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
             
                var response = await _borrowService.GetAllBorrowForUser(new BorrowInputDto { Id = borrowId });
                apiResponse.Data = response;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);

        }

        [HttpPost]
        [Route("UpdateBorrow")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> UpdateBorrow(BorrowStatusModifyDto borrowStatusModify)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _borrowService.ModifyBorrowStatus(borrowStatusModify);
                apiResponse.Message = response;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse);

        }

    }
}
