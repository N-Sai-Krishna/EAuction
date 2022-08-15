using AutoMapper;
using EAuction.Common.Authorization;
using EAuction.Seller.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seller.Core.Domain;
using Seller.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Controllers
{
    [ApiController]
    [EAuctionAuthorize]
    [Route("api/v1/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISellerService sellerService;

        public SellerController(IMapper mapper, ISellerService sellerService)
        {
            this.mapper = mapper;
            this.sellerService = sellerService;
        }

        [HttpPost("add-product")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(AuctionProductModel product)
        {
            if (await this.sellerService.AddProductAsync(mapper.Map<AuctionProduct>(product.Product), mapper.Map<AuctionProductSeller>(product.Seller)))
            {
                return Ok(new ResponseModel() { Message = "Your request has been processed successfully."});
            }

            return BadRequest();
        }


        [HttpDelete("delete/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string productId)
        {
            if (await this.sellerService.DeleteProductFromAuctionSync(productId))
            {
                return Ok(
                    new ResponseModel() { Message = "Your request has been received successfully. It will be processed shortly." }
                    );
            }

            return BadRequest();
        }

        [HttpGet("IsAlive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public bool IsAlive()
        {
            return true;
        }
    }
}
