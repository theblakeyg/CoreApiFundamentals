using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
  [Route("api/[controller]")]
  public class CampsController : ControllerBase
  {
    public object Get()
    {
      return new { Moniker = "ATL2020", Name = "Atlanta Code Camp" };
    }
  }
}
