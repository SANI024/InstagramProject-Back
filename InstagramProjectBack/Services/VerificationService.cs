using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

namespace InstagramProjectBack.Services
{
    public class VerificationService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        public VerificationService(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
        }

        public string GenerateVerifyToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        public void StoreVerifyToken(string token, string email)
        {
            _cache.Set(token, email, _cacheOptions);
        }

        public bool CheckVerifyToken(string token, out string email)
        {
          return _cache.TryGetValue(token, out email);
        }

        public void RemoveVerifyToken(string token)
        {
            _cache.Remove(token);
        }       
    }
}