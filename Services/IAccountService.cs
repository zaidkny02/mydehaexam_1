using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace deha_exam_quanlykhoahoc.Services
{
    public interface IAccountService
    {
        Task<Result> UpdateAccount(AccountManagerModel model);
        Task<Result> CreateAccount(RegisterViewModel model);
        Task<Result> Login(LoginViewModel model);
        Task<int> Logout();
        Task<Result> ChangeUserRole(string id, string newRole);
        Task<User> getUserbyID(string id);
        Task<IList<string>> getRolesofUser(User user);
        Task<List<IdentityRole>> getlistRole();
        Task<List<ListAccountViewModel>> getlistUserwithRole();
        Task<List<ListAccountViewModel>> getUserInRole(string rolename);
    }
}
