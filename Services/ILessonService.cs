using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface ILessonService
    {
        Task<LessonViewModel> GetById(int? id);
        Task<IEnumerable<LessonViewModel>> GetAllByClass(int? classid);
        Task<Result> Delete(int id);
        Task<Result> Create(LessonRequest lessonrequest);
        Task<Result> Update(LessonViewModel lessonviewmodel);
    }
}
