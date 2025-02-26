using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskPrioritizatorAPI.Controllers
{
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskBusiness taskBusiness;
        public TasksController(ITaskBusiness taskBusiness)
        {
            this.taskBusiness = taskBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Data.Model.InputTaskModel inputTask)
        {
            await taskBusiness.CreateAsync(inputTask);
            return Ok();
        }

        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskBusiness.GetAllAsync();
            return Ok(tasks);
        }

        [HttpPut("api/tasks/:{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Data.Model.Task task)
        {
            await taskBusiness.UpdateAsync(task);
            return Ok();
        }

        [HttpDelete("api/tasks/:{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await taskBusiness.DeleteAsync(id);
            return Ok();
        }
    }
}
