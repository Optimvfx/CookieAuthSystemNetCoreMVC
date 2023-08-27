using System.Security.Claims;

namespace Common.Convertors
{
    public static class ClaimConvertor
    {
        public static bool TryGetClaimValue<T>(this IEnumerable<Claim> claims, string type, out T value)
        {
            value = default;
            
            var claimeByType = claims.FirstOrDefault(x => x.Type == type);

            return claimeByType != null &&
                   claimeByType.Value.TryParseToObject(out value);
        }
    }
}
