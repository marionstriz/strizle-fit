using System.ComponentModel;
using System.Security.Claims;
using System.Text;

namespace Base.Extensions;

public static class IdentityExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) => GetUserId<Guid>(user);
    public static TKeyType GetUserId<TKeyType>(this ClaimsPrincipal user)
    {
        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
        {
            throw new NullReferenceException("NameIdentifier claim not found");
        }

        return (TKeyType) TypeDescriptor
            .GetConverter(typeof(TKeyType))
            .ConvertFromInvariantString(idClaim.Value)!;
    }
}