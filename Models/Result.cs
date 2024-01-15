namespace deha_exam_quanlykhoahoc.Models
{
    public class Result
    {
        public string type;
        public string message;

        public Result(string type, string message)
        {
            this.type = type;
            this.message = message;
        }
        public Result() { }
    }
}
