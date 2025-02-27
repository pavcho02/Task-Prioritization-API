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
        private Mock<DbSet<Data.Model.Task>> dbSet;

        [SetUp]
        public void Setup()
        {
            context = new Mock<TaskDbContext> ();
            dbSet = new Mock<DbSet<Data.Model.Task>>();

            taskBusiness = new TaskBusiness(context.Object);
        }

        [Test]
        public async Task DeleteAsyncRemovesTask()
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

            dbSet.Setup(x => x.FindAsync(1)).ReturnsAsync(task);
            dbSet.Setup(x => x.Remove(task));

            context.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await taskBusiness.DeleteAsync(1);

            
        }
    }
}
