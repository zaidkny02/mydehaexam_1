namespace deha_exam_quanlykhoahoc.Models
{
    public class FileinLesson
    {
        public int Id { get; set; }
        public string? name { get; set; }

        public string? filePath { get; set; }
        public int LessonID { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
