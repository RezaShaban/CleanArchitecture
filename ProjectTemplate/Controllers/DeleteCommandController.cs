using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class DeleteCommandController<TDeleteCommand> : ApiControllerBase
    {
        //TODO: should be implement in QueryController
        //[HttpGet]
        //public async Task<ActionResult<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
        //{
        //    return await Mediator.Send(query);
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id, CancellationToken token)
        {
            var command = Activator.CreateInstance(typeof(TDeleteCommand));
            command?.GetType().GetProperty("Id")?.SetValue(command, id);
            var result = await Mediator.Send(command, token);

            return Ok(result);
        }
    }
}
