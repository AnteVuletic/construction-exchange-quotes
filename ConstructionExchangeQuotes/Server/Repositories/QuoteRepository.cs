using ConstructionExchangeQuotes.Server.InfrastructureModels;
using ConstructionExchangeQuotes.Server.Models;
using ConstructionExchangeQuotes.Server.Utils;
using ConstructionExchangeQuotes.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConstructionExchangeQuotes.Server.Repositories
{
    public class QuoteRepository
    {
        private readonly QuotesDbContext _context;
        private readonly EmailSender _emailSender;

        public QuoteRepository(QuotesDbContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public Quote AddQuote(double taxRatePercentage, string customerEmail, List<QuoteElementDto> quoteElements, bool shouldNotifyByMail)
        {
            if(quoteElements.Count == 0)
            {
                return null;
            }

            var subTotal = quoteElements.Sum(qe => qe.Element.Rate * qe.Amount);
            var dateCreated = DateTime.Now;

            var quoteToAdd = new Quote
            {
                SubTotal = subTotal,
                TaxRatePercentage = taxRatePercentage,
                DateCreated = dateCreated,
                CustomerEmail = customerEmail,
                IsArchived = false
            };

            var addedQuote = _context.Quotes.Add(quoteToAdd);

            _context.SaveChanges();

            foreach (var quoteElement in quoteElements)
            {
                quoteElement.QuoteId = addedQuote.Entity.Id;
            }

            _context.QuoteElements.AddRange(quoteElements.Select(qe => new QuoteElement() { 
                Amount = qe.Amount,
                ElementId = qe.ElementId,
                QuoteId = qe.QuoteId
            }));
            _context.SaveChanges();

            if (shouldNotifyByMail)
            {

                var elementAggregateString = quoteElements.Aggregate("<br>Name - Amount - Rate - Total<br>", (acc, qE) => acc + $" <br>{qE.Element.Name} - {qE.Amount} - {qE.Element.Rate}$ - {qE.Amount * qE.Element.Rate}$");
                var body = $"Subtotal: {quoteToAdd.SubTotal}$ | TaxRatePercentage: {quoteToAdd.TaxRatePercentage}% | Date Created: {quoteToAdd.DateCreated.ToLongDateString()}{elementAggregateString}";
                var message = new Message(new List<string>() { customerEmail }, $"Quote for construction work | {quoteToAdd.SubTotal}$", body);
                _emailSender.SendMail(message);
            }

            return addedQuote.Entity;
        }

        public List<Quote> GetAllQuotes()
        {
            var quotes = _context.Quotes
                    .Include(q => q.QuoteElements)
                    .ThenInclude(qe => qe.Element)
                    .OrderByDescending(q => q.DateCreated)
                    .Where(q => !q.IsArchived)
                    .ToList();

            foreach (var quote in quotes)
            {
                foreach (var quoteElement in quote.QuoteElements)
                {
                    quoteElement.Quote = null;
                    quoteElement.Element.QuoteElements = null;
                }
            }

            return quotes;
        }

        public List<Quote> GetArchivedQuotes()
        {
            var quotes = _context.Quotes
                    .Include(q => q.QuoteElements)
                    .ThenInclude(qe => qe.Element)
                    .OrderByDescending(q => q.DateCreated)
                    .Where(q => q.IsArchived)
                    .ToList();

            foreach (var quote in quotes)
            {
                foreach (var quoteElement in quote.QuoteElements)
                {
                    quoteElement.Quote = null;
                    quoteElement.Element.QuoteElements = null;
                }
            }

            return quotes;
        }

        public List<Quote> GetQuotesByDate(DateTime dateCreated)
        {
            var dateIso = Date.ToIsoFormat(dateCreated);

            var quotes = _context.Quotes
                    .Include(q => q.QuoteElements)
                    .OrderByDescending(q => q.DateCreated)
                    .Where(q => Date.ToIsoFormat(q.DateCreated) == dateIso && q.IsArchived == false)
                    .ToList();

            foreach (var quote in quotes)
            {
                foreach (var quoteElement in quote.QuoteElements)
                {
                    quoteElement.Quote = null;
                }
            }

            return quotes;
        }

        public bool ArchiveQuote(int id)
        {
            var quoteToArchive = _context.Quotes.Find(id);

            if (quoteToArchive == null || quoteToArchive.IsArchived)
            {
                return false;
            }

            quoteToArchive.IsArchived = true;

            var changes = _context.SaveChanges();

            return changes > 0;
        }

        public bool UnarchiveQuote(int id)
        {
            var quoteToArchive = _context.Quotes.Find(id);

            if (quoteToArchive == null || !quoteToArchive.IsArchived)
            {
                return false;
            }

            quoteToArchive.IsArchived = false;

            var changes = _context.SaveChanges();

            return changes > 0;
        }

        public bool DeleteQuote(int id)
        {
            var quoteToDelete = _context.Quotes.Find(id);

            if(quoteToDelete == null || !quoteToDelete.IsArchived)
            {
                return false;
            }

            _context.Quotes.Remove(quoteToDelete);
            var changes = _context.SaveChanges();

            return changes > 0;
        }
    }
}
