using Microsoft.EntityFrameworkCore.ChangeTracking;
using NeoMatrix.Data;
using NeoMatrix.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoMatrix.Caches
{
    public class ValidationsCache : IValidationsCache
    {

        private static ConcurrentDictionary<string, ValidationResult> validations;

        private readonly MatrixDbContext db;

        public ValidationsCache(MatrixDbContext db)
        {
            this.db = db;

            if (validations == null)
            {
                validations = new ConcurrentDictionary<string, ValidationResult>(
                    db.ValidationResults.ToDictionary(n => n.Name)
                );
            }
        }

        public async Task<ValidationResult> CreateAsync(ValidationResult validationResult)
        {
            await db.ValidationResults.AddAsync(validationResult);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return validations.AddOrUpdate(validationResult.Url, validationResult, UpdateCache);
            }
            return null;
        }

        private ValidationResult UpdateCache(string url, ValidationResult validationResult)
        {
            if (validations.TryGetValue(url, out ValidationResult old))
                if (validations.TryUpdate(url, validationResult, old)) return validationResult;
            return null;
        }
    }
}
