using deha_exam_quanlykhoahoc.Models;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class FileInLessonModel
    {
        public int Id { get; set; }
        public string? name { get; set; }

        public IFormFile? file { get; set; }
        public string? filePath { get; set; }
        public int LessonID { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
