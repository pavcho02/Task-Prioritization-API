using Data;
using Data.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class TaskBusiness : ITaskBusiness
    {
        private readonly TaskDbContext context;

        private Data.Model.Task GetTaskWithCalculatedPriority(Data.Model.InputTaskModel inputTask)
        {
            Data.Model.Task task = new Data.Model.Task();
            task.Title = inputTask.Title;
            task.Description = inputTask.Description;
            task.DueDate = inputTask.DueDate;
            if(inputTask.IsCritical)
            {
                task.Priority = PriorityType.high;
                return task;
            }            
            else if(task.DueDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)))
            {
                task.Priority = PriorityType.medium;
                return task;
            }
            else
            {
                task.Priority = PriorityType.low;
                return task;
            }            
        }

        private PriorityType CalculateTaskPriority(Data.Model.TaskUpdateModel task)
        {
            if(task.IsCompleted)
            {
                return PriorityType.low;
            }
            else if (task.IsCritical)
            {
                return PriorityType.high;
            }
            else if (task.DueDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)))
            {
                return PriorityType.medium;
            }
            else
            {
                return PriorityType.low;
            }
        }

        public TaskBusiness(TaskDbContext context)
        {
            this.context = context;
        }

        public async Task<Data.Model.Task> CreateAsync(Data.Model.InputTaskModel inputTask)
        {
            var task = GetTaskWithCalculatedPriority(inputTask);

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task<Data.Model.Task?> GetAsync(int id)
        {
            return await context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<Data.Model.Task>> GetSortedByPriorityLevelAsync() 
        {
            return await context.Tasks.OrderBy(Task => (int)Task.Priority).ToListAsync();
        }

        public async Task<IEnumerable<Data.Model.Task>> GetSortedByDueDateAsync() 
        {
            return await context.Tasks.OrderBy(Task => Task.DueDate).ToListAsync();
        }

        public async Task<IEnumerable<Data.Model.Task>> GetFilteredByCompletionStatusAsync(bool isCompleted) 
        {
            return await context.Tasks.Where(t => t.IsCompleted == isCompleted).ToListAsync();
        }

        public async Task<IEnumerable<Data.Model.Task>> GetFilteredByPriorityLevelAsync(PriorityType priorityType)
        {
            return await context.Tasks.Where(t => t.Priority.Equals(priorityType)).ToListAsync();
        }

        public async Task<IEnumerable<Data.Model.Task>> GetAllAsync()
        {
            //the default sorting is by priority level
            return await GetSortedByPriorityLevelAsync();
        }

        public async Task UpdateAsync(Data.Model.TaskUpdateModel task)
        {
            var existingTask = await context.Tasks.FindAsync(task.Id);
            if(existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.Priority = CalculateTaskPriority(task);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task != null)
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();
            }
        }
    }
}
