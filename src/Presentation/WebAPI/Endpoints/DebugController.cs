using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints;

[Authorize]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }
}