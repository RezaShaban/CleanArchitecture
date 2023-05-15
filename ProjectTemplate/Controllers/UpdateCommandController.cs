using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class UpdateCommandController<TUpdateCommand> : ApiControllerBase
    {
        [HttpPut]
        public async Task<ActionResult> Update(TUpdateCommand command, CancellationToken token)
        {
            var result = await Mediator.Send(command, token);
            return Ok(result);
        }
    }
}
