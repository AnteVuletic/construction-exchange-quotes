﻿using ConstructionExchangeQuotes.Server.Repositories;
using ConstructionExchangeQuotes.Server.Utils;
using ConstructionExchangeQuotes.Shared;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ConstructionExchangeQuotes.Server.Controllers
{
    [ApiController]
    [Route("quote")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteRepository _quoteRepository;

        public QuoteController(QuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        [HttpPost("add")]
        public IActionResult AddQuote(AddQuoteData data)
        {
            var addedQuote = _quoteRepository.AddQuote(data.TaxRatePercentage, data.CustomerEmail, data.QuoteElements, data.ShouldNotifyByEmail);

            if(addedQuote == null)
            {
                return BadRequest();
            }

            return Ok(addedQuote);
        }

        [HttpGet("get")]
        public IActionResult GetQuotes([FromQuery] bool archived, [FromQuery] string customerEmail, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var quotes = _quoteRepository.GetQuotes(archived, customerEmail, dateFrom, dateTo);

            return Ok(quotes);
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteQuote(int id)
        {
            var isDeleteSuccessful = _quoteRepository.DeleteQuote(id);

            if (!isDeleteSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("archive/{id:int}")]
        public IActionResult ArchiveQuote(int id)
        {
            var IsArchiveSuccessful = _quoteRepository.ArchiveQuote(id);

            if (!IsArchiveSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("unarchive/{id:int}")]
        public IActionResult UnarchiveQuote(int id)
        {
            var IsArchiveSuccessful = _quoteRepository.UnarchiveQuote(id);

            if (!IsArchiveSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
