using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropellerMonkeys.WebApp.Sockets;
using PropellerMonkeys.WebApp.Sockets.Server.ConsoleClient;

namespace PropellerMonkeys.WebApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClientsController : ControllerBase
  {
    // GET: api/Clients
    [HttpGet]
    public IEnumerable<ConsoleClientWrapper> Get() => WebSocketsMiddleware.ConsoleClientServer.CurrentClients;

    // GET: api/Clients/5
    [HttpGet("{id}", Name = "Get")]
    public string Get(int id)
    {
      return "value";
    }

    // POST: api/Clients
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT: api/Clients/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
