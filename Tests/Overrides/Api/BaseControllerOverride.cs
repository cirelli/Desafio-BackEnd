using Api.Controllers;
using Domain.ServiceResults;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Overrides.Api
{
    internal class BaseControllerOverride : BaseController
    {
        new public ActionResult HandleServiceResult(IServiceResult result)
        {
            return base.HandleServiceResult(result);
        }
    }
}
