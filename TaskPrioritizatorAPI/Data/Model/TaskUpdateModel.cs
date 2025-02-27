﻿using Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TaskUpdateModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateOnly DueDate { get; set; }

        public bool IsCritical { get; set; }

        public bool IsCompleted { get; set; }
    }
}
