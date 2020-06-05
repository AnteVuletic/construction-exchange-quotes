using ConstructionExchangeQuotes.Server.Models;
using ConstructionExchangeQuotes.Server.Repositories;
using ConstructionExchangeQuotes.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IActionResult AddQuote(double taxRatePercentage, string customerEmail, List<QuoteElement> quoteElements)
        {
            var addedQuote = _quoteRepository.AddQuote(taxRatePercentage, customerEmail, quoteElements);

            return Ok(addedQuote);
        }

        [HttpGet("get")]
        public IActionResult GetAllQuotes()
        {
            var quotes = _quoteRepository.GetAllQuotes();

            return Ok(quotes);
        }

        [HttpGet("archived")]
        public IActionResult GetArchivedQuotes()
        {
            var quotes = _quoteRepository.GetArchivedQuotes();

            return Ok(quotes);
        }

        [HttpGet("get/date")]
        public IActionResult GetQuotesByDate(string date)
        {
            var isDateValid = DateTime.TryParse(date, out var filterDate);

            if (!isDateValid)
            {
                return BadRequest();
            }

            var quotes = _quoteRepository.GetQuotesByDate(filterDate);

            return Ok(quotes);
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteQuote(int id)
        {
            var isDeleteSuccessful = _quoteRepository.DeleteQuote(id);

            if (!isDeleteSuccessful)
            {
                return NotFound();
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
    }
}
