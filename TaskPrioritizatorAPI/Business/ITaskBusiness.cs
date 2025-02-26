using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model.Enums;

namespace Business
{
    public interface ITaskBusiness
    {
        public Task CreateAsync(Data.Model.InputTaskModel inputTask);

        public Task<IEnumerable<Data.Model.Task>?> GetAllAsync();

        public Task<IEnumerable<Data.Model.Task>?> GetSortedByPriorityLevelAsync();

        public Task<IEnumerable<Data.Model.Task>?> GetSortedByDueDateAsync();

        public Task<IEnumerable<Data.Model.Task>?> GetFilteredByCompletionStatusAsync(bool isCompleted);

        public Task<IEnumerable<Data.Model.Task>?> GetFilteredByPriorityLevelAsync(PriorityType priorityType);

        public Task<Data.Model.Task?> GetAsync(int id);

        public Task UpdateAsync(Data.Model.Task task);

        public Task DeleteAsync(int id);
    }
}
