using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CreateCommandController<TCreateCommand> : ApiControllerBase
        where TCreateCommand : ICreateCommand
    {
        [HttpPost]
        public async Task<ActionResult<object?>> Create(TCreateCommand command, CancellationToken token)
        {
            return await Mediator.Send(command, token);
        }
    }
}
