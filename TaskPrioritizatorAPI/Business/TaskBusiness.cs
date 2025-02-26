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
            }
            else if(task.DueDate.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))) < 0)
            {
                task.Priority = PriorityType.medium;
            }
            else
            {
                task.Priority = PriorityType.low;
            }

            return task;
        }

        private void UpdatePriority(Data.Model.Task task)
        {
            if (task.IsCompleted)
            {
                task.Priority = PriorityType.low;
            }
        }

        public TaskBusiness(TaskDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Data.Model.InputTaskModel inputTask)
        {
            await context.Tasks.AddAsync(GetTaskWithCalculatedPriority(inputTask));
            await context.SaveChangesAsync();
        }

        public async Task<Data.Model.Task?> GetAsync(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            if(task != null)
            {
                return task;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Data.Model.Task>?> GetSortedByPriorityLevelAsync() 
        {
            var tasks = await context.Tasks.OrderBy(Task => Task.Priority).ToListAsync();
            if (tasks != null)
            {
                return tasks;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Data.Model.Task>?> GetSortedByDueDateAsync() 
        {
            var tasks = await context.Tasks.OrderBy(Task => Task.DueDate).ToListAsync();
            if (tasks != null)
            {
                return tasks;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Data.Model.Task>?> GetFilteredByCompletionStatusAsync(bool isCompleted) 
        {
            var tasks = await context.Tasks.Where(t => t.IsCompleted == isCompleted).ToListAsync();
            if (tasks != null)
            {
                return tasks;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Data.Model.Task>?> GetFilteredByPriorityLevelAsync(PriorityType priorityType)
        {
            var tasks = await context.Tasks.Where(t => t.Priority ==  priorityType).ToListAsync();
            if (tasks != null)
            {
                return tasks;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Data.Model.Task>?> GetAllAsync()
        {
            return await GetSortedByPriorityLevelAsync();
        }

        public async Task UpdateAsync(Data.Model.Task task)
        {
            var existingTask = await context.Tasks.FindAsync(task.Id);
            if(existingTask != null)
            {
                context.Entry(existingTask).CurrentValues.SetValues(task);
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
