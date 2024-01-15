using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface IClassService
    {
        Task<IEnumerable<ClassViewModel>> GetAll();
        Task<IEnumerable<ClassViewModel>> GetAllbyUser(string userid);
        Task<ClassViewModel> GetById(int? id);
        Task<Result> Create(ClassRequest request);
        Task<Result> Update(ClassViewModel request);
        Task<Result> Delete(int id);
    }
}
