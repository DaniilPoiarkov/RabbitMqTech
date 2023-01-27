using System.Security.Claims;
using System.Security.Principal;

namespace RabbitMq.WebAPI.Extensions;

public static class IdentityExtension
{
    public static int GetUserId(this IIdentity identity)
    {
        if(identity is null)
            throw new ArgumentNullException(nameof(identity));

        var id = (identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        _ = int.TryParse(id, out var userId);
        return userId;
    }
}
