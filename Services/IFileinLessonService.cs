using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface IFileinLessonService
    {
        Task<FileInLessonModel> GetById(int? id);
        Task<IEnumerable<FileInLessonModel>> GetAllByLesson(int? lessonid);
        Task<Result> Delete(int id);
        Task<Result> Create(FileInLessonModel fileinlesson);
        Task<Result> Update(FileInLessonModel fileinlesson);
    }
}
