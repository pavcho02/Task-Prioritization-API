using Business;
using Data;
using Data.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Legacy;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private TaskBusiness taskBusiness;
        private TaskDbContext context;

        private async Task SeedTestData()
        {
            await context.Tasks.AddAsync(new Data.Model.Task
            {
                Id = 1,
                Title = "Test Task 1",
                Description = "Test description 1",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Priority = PriorityType.high,
                IsCompleted = false
            });
            await context.Tasks.AddAsync(new Data.Model.Task
            {
                Id = 2,
                Title = "Test Task 2",
                Description = "Test description 2",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                Priority = PriorityType.medium,
                IsCompleted = false
            });
            await context.Tasks.AddAsync(new Data.Model.Task
            {
                Id = 3,
                Title = "Test Task 3",
                Description = "Test description 3",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
                Priority = PriorityType.low,
                IsCompleted = false
            });
            await context.Tasks.AddAsync(new Data.Model.Task
            {
                Id = 4,
                Title = "Test Task 4",
                Description = "Test description 4",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1),
                Priority = PriorityType.low,
                IsCompleted = true
            });
            await context.SaveChangesAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            context = new TaskDbContext(options);

            await SeedTestData();

            taskBusiness = new TaskBusiness(context);
        }

        [Test]
        public async Task CreateAsyncAddsTask()
        {
            var task = new Data.Model.InputTaskModel
            {
                Title = "Test",
                Description = "Test",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCritical = true
            };

            var resultTask = await taskBusiness.CreateAsync(task);

            Assert.That(context.Tasks.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task CreateAsyncCalculatesCorrectForHighPriority()
        {
            // Task with high priority 
            var task = new Data.Model.InputTaskModel
            {
                Title = "Test1",
                Description = "Test1",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCritical = true
            };
            
            var resultTask = await taskBusiness.CreateAsync(task);

            Assert.That(task.Title, Is.EqualTo(resultTask?.Title));
            Assert.That(task.Description, Is.EqualTo(resultTask?.Description));
            Assert.That(task.DueDate, Is.EqualTo(resultTask?.DueDate));
            Assert.That(PriorityType.high, Is.EqualTo(resultTask?.Priority));
            Assert.That(false, Is.EqualTo(resultTask?.IsCompleted));
        }

        [Test]
        public async Task CreateAsyncCalculatesCorrectForMediumPriority()
        {
            // Task with medium priority 
            var task = new Data.Model.InputTaskModel
            {
                Title = "Test2",
                Description = "Test2",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2),
                IsCritical = false
            };

            var resultTask = await taskBusiness.CreateAsync(task);

            Assert.That(task.Title, Is.EqualTo(resultTask?.Title));
            Assert.That(task.Description, Is.EqualTo(resultTask?.Description));
            Assert.That(task.DueDate, Is.EqualTo(resultTask?.DueDate));
            Assert.That(PriorityType.medium, Is.EqualTo(resultTask?.Priority));
            Assert.That(false, Is.EqualTo(resultTask?.IsCompleted));
        }

        [Test]
        public async Task CreateAsyncCalculatesCorrectForLowPriority()
        {            
            // Task with low priority 
            var task = new Data.Model.InputTaskModel
            {
                Title = "Test3",
                Description = "Test3",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
                IsCritical = false
            };

            var resultTask = await taskBusiness.CreateAsync(task);

            Assert.That(task.Title, Is.EqualTo(resultTask?.Title));
            Assert.That(task.Description, Is.EqualTo(resultTask?.Description));
            Assert.That(task.DueDate, Is.EqualTo(resultTask?.DueDate));
            Assert.That(PriorityType.low, Is.EqualTo(resultTask?.Priority));
            Assert.That(false, Is.EqualTo(resultTask?.IsCompleted));
        }

        [Test]
        public async Task GetSortedByPriorityLevelAsyncReturnsTasksSortedByPriority()
        {
            var result = await taskBusiness.GetSortedByPriorityLevelAsync();
            
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result.ElementAt(0).Priority, Is.EqualTo(PriorityType.high));
            Assert.That(result.ElementAt(1).Priority, Is.EqualTo(PriorityType.medium));
            Assert.That(result.ElementAt(2).Priority, Is.EqualTo(PriorityType.low));
            Assert.That(result.ElementAt(3).Priority, Is.EqualTo(PriorityType.low));
        }

        [Test]
        public async Task GetSortedByDueDateAsyncReturnsTasksSortedByDueDate()
        {
            var result = await taskBusiness.GetSortedByDueDateAsync();

            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(4));
            Assert.That(result.ElementAt(2).Id, Is.EqualTo(2));
            Assert.That(result.ElementAt(3).Id, Is.EqualTo(3));
        }

        [Test]
        public async Task GetFilteredByPriorityLevelAsyncReturnsTasksFilteredByPriority()
        {
            var result = await taskBusiness.GetFilteredByPriorityLevelAsync(PriorityType.high);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0).Priority, Is.EqualTo(PriorityType.high));
        }

        [Test]
        public async Task GetFilteredByCompletionStatusAsyncReturnsTasksFilteredByCompletionStatus()
        {
            var result = await taskBusiness.GetFilteredByCompletionStatusAsync(true);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0).IsCompleted, Is.EqualTo(true));
        }

        [Test]
        public async Task GetAsyncReturnsCorrectTaskById()
        {
            var task = new Data.Model.Task
            {
                Id = 1,
                Title = "Test Task 1",
                Description = "Test description 1",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Priority = PriorityType.high,
                IsCompleted = false
            };

            var result = await taskBusiness.GetAsync(1);

            Assert.That(task.Id, Is.EqualTo(result?.Id));
            Assert.That(task.Title, Is.EqualTo(result?.Title));
            Assert.That(task.Description, Is.EqualTo(result?.Description));
            Assert.That(task.DueDate, Is.EqualTo(result?.DueDate));
            Assert.That(task.Priority, Is.EqualTo(result?.Priority));
            Assert.That(task.IsCompleted, Is.EqualTo(result?.IsCompleted));
        }

        [Test]
        public async Task GetAsyncCannotReturnTaskByWithInvalidId()
        {
            var result = await taskBusiness.GetAsync(10);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAllAsyncReturnsAllTasks()
        {
            var result = await taskBusiness.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(4));

            // Assert that all returned tasks are default sorted(by priority)
            Assert.That(result.ElementAt(0).Priority, Is.EqualTo(PriorityType.high));
            Assert.That(result.ElementAt(1).Priority, Is.EqualTo(PriorityType.medium));
            Assert.That(result.ElementAt(2).Priority, Is.EqualTo(PriorityType.low));
            Assert.That(result.ElementAt(3).Priority, Is.EqualTo(PriorityType.low));
        }

        [Test]
        public async Task UpdateAsyncCalculatesCorrectPriorityWhenDueDateIsUpdated()
        {
            var task = new Data.Model.TaskUpdateModel
            {
                Id = 4,
                Title = "Updated",
                Description = "Updated description",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2),
                IsCompleted = false,
                IsCritical = false
            };

            await taskBusiness.UpdateAsync(task);

            var result = await taskBusiness.GetAsync(4);

            Assert.That(DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2), Is.EqualTo(result?.DueDate));
            Assert.That(result?.Priority, Is.EqualTo(PriorityType.medium));            
        }

        [Test]
        public async Task UpdateAsyncCalculatesCorrectPriorityWhenIsCompletedIsUpdated()
        {
            var task = new Data.Model.TaskUpdateModel
            {
                Id = 1,
                Title = "Updated",
                Description = "Updated description",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCompleted = true,
                IsCritical = false
            };

            await taskBusiness.UpdateAsync(task);

            var result = await taskBusiness.GetAsync(1);

            Assert.That(PriorityType.low, Is.EqualTo(result?.Priority));
            Assert.That(task.IsCompleted, Is.EqualTo(result?.IsCompleted));
        }

        [Test]
        public async Task UpdateAsyncUpdatesTaskWithNoChangeOfProirity()
        {
            var task = new Data.Model.TaskUpdateModel
            {
                Id = 1,
                Title = "Updated",
                Description = "Updated description",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCompleted = false,
                IsCritical = false
            };

            await taskBusiness.UpdateAsync(task);

            var result = await taskBusiness.GetAsync(1);

            Assert.That(task.Id, Is.EqualTo(result?.Id));
            Assert.That(task.Title, Is.EqualTo(result?.Title));
            Assert.That(task.Description, Is.EqualTo(result?.Description));
            Assert.That(task.DueDate, Is.EqualTo(result?.DueDate));
            Assert.That(task.IsCompleted, Is.EqualTo(result?.IsCompleted));
        }

        [Test]
        public async Task UpdateAsyncCannotUpdateTaskWithInvalidId()
        {
            var task = new Data.Model.TaskUpdateModel
            {
                Id = 10,
                Title = "Test Task 10",
                Description = "Test description 10",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCompleted = false,
                IsCritical = false
            };

            await taskBusiness.UpdateAsync(task);

            var result = await taskBusiness.GetAsync(10);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsyncRemovesTask()
        {
            await taskBusiness.DeleteAsync(1);

            Assert.That(context.Tasks.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteAsyncCannotRemoveTaskWithInvalidId()
        {
            await taskBusiness.DeleteAsync(10);

            Assert.That(context.Tasks.Count(), Is.EqualTo(4));
        }

        // TearDown method to dispose of the context after each test
        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();  // Ensure the in-memory database is deleted after each test
            context.Dispose();  // Properly dispose of the context to release resources
        }
    }
}
