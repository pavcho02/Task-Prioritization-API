using Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PriorityType Priority { get; set; }

        public DateOnly DueDate { get; set; }

        public bool IsComplete { get; set; }
    }
}
