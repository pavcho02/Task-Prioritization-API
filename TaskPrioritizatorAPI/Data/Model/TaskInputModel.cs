using Data.Model.Enums;

namespace Data.Model
{
    public class InputTaskModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateOnly DueDate { get; set; }

        public bool IsCritical { get; set; }
    }
}
