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
            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            if (HttpContext.Request.Query.ContainsKey("sort"))
            {
                string sort = HttpContext.Request.Query["sort"];
                IEnumerable<Data.Model.Task>? tasks = null;
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

                if (tasks != null)
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound();
                }
            }
            else if (HttpContext.Request.Query.ContainsKey("sort"))
            {
                string filter = HttpContext.Request.Query["filter"];
                string value = HttpContext.Request.Query["value"];
                IEnumerable<Data.Model.Task>? tasks = null;
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

                if (tasks != null)
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound();
                }
            }
            else
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
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Data.Model.TaskUpdateModel task)
        {
            if ((await taskBusiness.GetAsync(id)) == null && task.Id != id)
            {
                return BadRequest();
            }
            else
            {
                await taskBusiness.UpdateAsync(task);
                return Ok(await taskBusiness.GetAsync(id));
            }
        }

        [HttpDelete("/tasks/:{id}")]
        public async Task<IActionResult> DeleteTaskById(int id)
        {
            var task = await taskBusiness.GetAsync(id);
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
