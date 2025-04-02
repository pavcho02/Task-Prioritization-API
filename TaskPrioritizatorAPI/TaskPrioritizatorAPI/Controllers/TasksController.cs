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
            var task = await taskBusiness.CreateAsync(inputTask);
            return Created($"/tasks/{task.Id}", task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? sort, [FromQuery] string? filter, [FromQuery] string? value)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                IEnumerable<Data.Model.Task> tasks = new List<Data.Model.Task>();
                if (sort.Equals("priority"))
                {
                    tasks = await taskBusiness.GetSortedByPriorityLevelAsync();
                }
                else if (sort.Equals("dueDate"))
                {
                    tasks = await taskBusiness.GetSortedByDueDateAsync();
                }
                else //default sort is sorting by priority
                {
                    tasks = await taskBusiness.GetSortedByPriorityLevelAsync();
                }

                if (tasks.Any())
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound("No tasks founded with sorting parameters");
                }
            }
            else if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(value))
            {
                IEnumerable<Data.Model.Task>? tasks = new List<Data.Model.Task>();
                if (filter.Equals("priority"))
                {
                    if (value.Equals("high"))
                    {
                        tasks = await taskBusiness.GetFilteredByPriorityLevelAsync(PriorityType.high);
                    }
                    else if (value.Equals("medium"))
                    {
                        tasks = await taskBusiness.GetFilteredByPriorityLevelAsync(PriorityType.medium);
                    }
                    else if (value.Equals("low"))
                    {
                        tasks = await taskBusiness.GetFilteredByPriorityLevelAsync(PriorityType.low);
                    }
                }
                else if (filter.Equals("isCompleted"))
                {
                    if (value.Equals("true"))
                    {
                        tasks = await taskBusiness.GetFilteredByCompletionStatusAsync(true);
                    }
                    else
                    {
                        tasks = await taskBusiness.GetFilteredByCompletionStatusAsync(false);
                    }
                }

                if (tasks.Any())
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound("No tasks founded with filter parameters");
                }
            }
            else
            {
                var tasks = await taskBusiness.GetAllAsync();
                if (tasks.Any())
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound("No tasks found");                    
                }
            }
        }

        [HttpGet("/tasks/{id}")]
        public async Task<IActionResult> GetByTaskId(int id)
        {
            var task = await taskBusiness.GetAsync(id);
            if(task != null)
            {
                return Ok(task);
            }
            else
            {
                return NotFound("Invalid task ID");
            }
        }

        [HttpPut("/tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Data.Model.TaskUpdateModel task)
        {
            if ((await taskBusiness.GetAsync(id)) == null)
            {
                return NotFound("Invalid task ID");
            }
            else
            {
                await taskBusiness.UpdateAsync(task);
                return Ok(await taskBusiness.GetAsync(id));
            }
        }

        [HttpDelete("/tasks/{id}")]
        public async Task<IActionResult> DeleteTaskById(int id)
        {
            if ((await taskBusiness.GetAsync(id)) == null)
            {
                return NotFound("Invalid task ID");
            }
            else
            {
                await taskBusiness.DeleteAsync(id);
                return Ok();
            }
        }
    }
}
