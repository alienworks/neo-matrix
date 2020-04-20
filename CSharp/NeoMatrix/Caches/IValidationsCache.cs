using NeoMatrix.Data.Models;
using System.Threading.Tasks;

namespace NeoMatrix.Caches
{
    public interface IValidationsCache
    {
        Task<ValidationResult> CreateAsync(ValidationResult validationResult);
    }
}