using AutoMapper;
using BidListing.API.Models;
using BidListing.Core.Domain;
using BidListing.Core.Services;
using EAuction.Common.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BidListing.API.Controllers
{
    [ApiController]
    [EAuctionAuthorize]
    [Route("api/v1")]
    public class BidListingController : ControllerBase
    {
        private readonly IBidListingService bidListingService;
        private readonly IMapper mapper;

        public BidListingController(IBidListingService bidListingService, IMapper mapper)
        {
            this.bidListingService = bidListingService;
            this.mapper = mapper;
        }


        [HttpGet("seller/show-bids/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductBidDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(string productId)
        {
            ProductAndBidDetails result = await this.bidListingService.GetProductAndBidDetails(productId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<ProductBidDetailsModel>(result));
        }

        [HttpGet("IsAlive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public bool IsAlive()
        {
            return true;
        }

    }
}
