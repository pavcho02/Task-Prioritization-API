using Business;
using Data.Model.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskPrioritizatorAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskBusiness.GetAllAsync();
            if (tasks == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(tasks);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSorted([FromQuery]string sort)
        {
            IEnumerable<Data.Model.Task>? tasks = null;
            if (sort.Equals("priority"))
            {
                tasks = await taskBusiness.GetSortedByPriorityLevelAsync();
            }
            else if (sort.Equals("dueDate"))
            {
                tasks = await taskBusiness.GetSortedByDueDateAsync();
            }

            if(tasks != null)
            {
                return Ok(tasks);
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllFiltered(string filter, string value)
        {
            var tasks = new List<Data.Model.Task>();
            if (filter.Equals("priority"))
            {
                tasks = await taskBusiness.GetFilteredByPriorityLevelAsync(value);
            }
            else if (filter.Equals("isCompleted"))
            {
                tasks = await taskBusiness.GetFilteredByCompletionStatusAsync(value);
            }

            if(tasks != null)
            {
                return Ok(tasks);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/tasks/:{id}")]
        public async Task<IActionResult> GetByTaskId(int id)
        {
            var task = await taskBusiness.GetAsync(id);
            if(task != null)
            {
                return Ok(task);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("/tasks/:{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Data.Model.Task task)
        {
            if ((await taskBusiness.GetAsync(id)) == null)
            {
                return BadRequest();
            }
            else
            {
                await taskBusiness.UpdateAsync(task);
                return Ok(task);
            }
        }

        [HttpDelete("/tasks/:{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = taskBusiness.GetAsync(id);
            if(task == null)
            {
                return BadRequest();
            }
            else
            {
                await taskBusiness.DeleteAsync(id);
                return Ok();
            }
        }
    }
}
