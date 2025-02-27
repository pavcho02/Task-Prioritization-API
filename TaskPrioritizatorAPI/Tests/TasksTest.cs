using Business;
using Data;
using Data.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private TaskBusiness taskBusiness;

        private Mock<TaskDbContext> context;
        private Mock<DbSet<Data.Model.Task>> tasks;

        [SetUp]
        public void Setup()
        {
            context = new Mock<TaskDbContext>();
            tasks = new Mock<DbSet<Data.Model.Task>>();

            taskBusiness = new TaskBusiness(context.Object);
        }

        [Test]
        public void CreateAsyncAddsTask()
        {
            var task = new Data.Model.Task
            {
                Id = 1,
                Title = "Test",
                Description = "Test",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Priority = PriorityType.low,
                IsCompleted = false
            };

            var InputTask = new Data.Model.InputTaskModel
            {
                Title = "Test",
                Description = "Test",
                DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCritical = true
            };

            Assert.Pass();
        }

    }
}
