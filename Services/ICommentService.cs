using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface ICommentService
    {
        Task<CommentViewModel> GetById(int? id);
        Task<Result> Create(CommentRequest request);
        Task<Result> Delete(int id);
        Task<IEnumerable<CommentViewModel>> GetAllByLesson(int? lessonID);
        Task<Result> Update(CommentViewModel commentviewmodel);
    }
}
