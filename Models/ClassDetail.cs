namespace deha_exam_quanlykhoahoc.Models
{
    public class ClassDetail
    {
        public int Id { get; set; }
        public int ClassID { get; set; }
        public string UserID { get; set; }
        public Class? Class { get; set; }
        public User? User { get; set; }
    }
}
