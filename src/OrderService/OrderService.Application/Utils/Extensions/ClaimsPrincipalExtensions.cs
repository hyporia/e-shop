using System.Security.Claims;

namespace OrderService.Application.Utils.Extensions;

/// <summary>
/// Extensions for working with ClaimsPrincipal.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the user ID from the claims principal.
    /// </summary>
    /// <param name="principal">The claims principal.</param>
    /// <returns>The user ID as a Guid, or null if not found or not valid.</returns>
    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            return null;
        }

        // Try to get the user ID from the "sub" claim (standard JWT claim for subject/user ID)
        var subClaim = principal.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(subClaim) && Guid.TryParse(subClaim, out var subId))
        {
            return subId;
        }

        // Fallback to NameIdentifier if the sub claim is not present
        var nameIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(nameIdClaim) && Guid.TryParse(nameIdClaim, out var nameId))
        {
            return nameId;
        }

        return null;
    }

    /// <summary>
    /// Checks if the user can access a resource belonging to a specific user ID.
    /// </summary>
    /// <param name="principal">The claims principal.</param>
    /// <param name="resourceUserId">The user ID the resource belongs to.</param>
    /// <returns>True if the user is authorized to access the resource, false otherwise.</returns>
    public static bool CanAccessUserResource(this ClaimsPrincipal? principal, Guid resourceUserId)
    {
        var userId = principal.GetUserId();
        if (userId == null)
        {
            return false;
        }

        return userId == resourceUserId;
    }
}
