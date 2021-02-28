using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public abstract class ControllerBase
    {
    }
}
