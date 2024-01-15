using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface IClassDetailService
    {
        Task<Result> JoinClass(User user,int classid);
        Task<IEnumerable<ClassDetail>> GetAllByClass(int? classid);

        Task<ClassDetail> GetById(int? id);
        Task<Result> Delete(int id);
    }
}
