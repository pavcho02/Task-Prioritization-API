using Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TaskUpdateModel
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title must be less than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description must be less than 200 characters")]

        public string Description { get; set; }

        [Required(ErrorMessage = "DueDate is required")]
        public DateOnly DueDate { get; set; }

        [Required(ErrorMessage = "IsCritical is required")]
        public bool IsCritical { get; set; }

        [Required(ErrorMessage = "IsCompleted is required")]
        public bool IsCompleted { get; set; }
    }
}
