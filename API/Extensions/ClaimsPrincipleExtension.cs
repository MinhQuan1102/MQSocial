using System;
using System.Security.Claims;
using API.DTO.User;

namespace API.Extensions;

public static class ClaimsPrincipleExtension
{
    public static string GetUsername(this ClaimsPrincipal user) {
        return user.FindFirst(ClaimTypes.Name)?.Value ?? "";
    }

    public static int GetUserId(this ClaimsPrincipal user){
        return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
    }
}
