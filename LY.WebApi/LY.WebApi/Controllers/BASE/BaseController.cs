using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LY.WebApi.Controllers.BASE
{
    [Authorize]
    [ApiController]
    public class BaseController : ControllerBase
    {

    }
}
