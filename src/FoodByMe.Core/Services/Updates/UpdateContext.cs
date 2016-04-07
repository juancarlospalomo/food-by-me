using FoodByMe.Core.Contracts;
using FoodByMe.Core.Framework;
using SQLite.Net;

namespace FoodByMe.Core.Services.Updates
{
    internal class UpdateContext
    {
        public UpdateContext(ICultureProvider cultureProvider, 
            IRecipePersistenceService recipePersistenceService,
            IReferenceBookService referenceBook)
        {
            CultureProvider = cultureProvider;
            RecipePersistenceService = recipePersistenceService;
            ReferenceBook = referenceBook;
        }

        public ICultureProvider CultureProvider { get; internal set; }

        public SQLiteConnection Connection { get; internal set; }

        public IRecipePersistenceService RecipePersistenceService { get; internal set; }

        public IReferenceBookService ReferenceBook { get; internal set; }
    }
}