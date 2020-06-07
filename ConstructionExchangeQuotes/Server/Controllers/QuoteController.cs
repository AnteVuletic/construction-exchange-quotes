using ConstructionExchangeQuotes.Server.Repositories;
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
            var addedQuote = _quoteRepository.AddQuote(data.TaxRatePercentage, data.CustomerEmail, data.QuoteElements);

            if(addedQuote == null)
            {
                return BadRequest();
            }

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
