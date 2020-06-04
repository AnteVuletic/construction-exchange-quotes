﻿using ConstructionExchangeQuotes.Server.Models;
using ConstructionExchangeQuotes.Server.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionExchangeQuotes.Server.Repositories
{
    public class QuoteRepository
    {
        private readonly QuotesDbContext _context;

        public QuoteRepository(QuotesDbContext context)
        {
            _context = context;
        }

        public Quote AddQuote(double taxRatePercentage, string customerEmail, List<QuoteElement> quoteElements)
        {
            var subTotal = quoteElements.Sum(qe => qe.Element.Rate * qe.Amount);
            var dateCreated = DateTime.Now;

            var quoteToAdd = new Quote
            {
                SubTotal = subTotal,
                TaxRatePercentage = taxRatePercentage,
                DateCreated = dateCreated,
                CustomerEmail = customerEmail,
                QuoteElements = quoteElements,
                IsArchived = false
            };

            var addedQuote = _context.Quotes.Add(quoteToAdd);

            _context.SaveChanges();

            return addedQuote.Entity;
        }

        public List<Quote> GetAllQuotes()
        {
            var quotes = _context.Quotes
                    .Include(q => q.QuoteElements)
                    .OrderByDescending(q => q.DateCreated)
                    .Where(q => q.IsArchived == false)
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

        public List<Quote> GetArchivedQuotes()
        {
            var quotes = _context.Quotes
                    .Include(q => q.QuoteElements)
                    .OrderByDescending(q => q.DateCreated)
                    .Where(q => q.IsArchived == false)
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

        public bool DeleteQuote(int id)
        {
            var quoteToDelete = _context.Quotes.Find(id);

            if(quoteToDelete == null)
            {
                return false;
            }

            _context.Quotes.Remove(quoteToDelete);
            var changes = _context.SaveChanges();

            return changes > 0;
        }
    }
}
