using AutoMapper;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.Services;
using deha_exam_quanlykhoahoc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, UserManager<User> userManager, IMapper mapper)
        {
            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
        }

        #region login
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.Login(model);
                    if (result.type.Equals("Success"))
                    {
                        TempData["Message"] = result.message;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Message"] =  result.message;
                        return View(model);
                    }
                }
                else
                   return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }
            #endregion

        #region register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _accountService.CreateAccount(model);
            if(result.type.Equals("Success"))
            {
                TempData["Message"] = result.message;
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["Message"] = "An error occurred while register account: " + result.message;
                return View(model);
            }
        }
        #endregion

        #region logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            int a = await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region listaccount
        public async Task<IActionResult> ListAccount(int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (await CheckUserRole("Admin"))
                {
                    int pageSize = 3;
                    int pageNumber = page == null || page < 0 ? 1 : page.Value;
                    var mylist =  await _accountService.getlistUserwithRole();
                    PagedList<ListAccountViewModel> lst = new PagedList<ListAccountViewModel>(mylist, pageNumber, pageSize);
                    return View(lst);
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        #endregion
        #region changeuserrole
        public async Task<IActionResult> ChangeUserRole(string? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (await CheckUserRole("Admin"))
                {

                    var user = await _accountService.getUserbyID(id);
                    if (user == null)
                    {
                        // Handle the case where the user is not found
                        return NotFound();
                    }
                    var roles = await _accountService.getRolesofUser(user);
                    var role = roles[0];
                    var listrole = await _accountService.getlistRole();
                    listrole.Remove(listrole.SingleOrDefault(x => x.Name == "Admin"));
                    ViewBag.Roles = new SelectList(listrole, nameof(IdentityRole.Name), nameof(IdentityRole.Name), role);
                    return View(user);
                }
                else
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string id, string newRole)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (await CheckUserRole("Admin") && user.Id != id)
                {
                    var result = await _accountService.ChangeUserRole(id, newRole);
                    TempData["Message"] = result.message;
                    return RedirectToAction("ListAccount", "Account");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        #endregion


        #region accountmanager
        public async Task<IActionResult> AccountManager()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                return View(_mapper.Map<AccountManagerModel>(user));
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountManager(AccountManagerModel user)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _accountService.UpdateAccount(user);
                TempData["Message"] = result.message;
                if (result.type.Equals("Success"))
                    return RedirectToAction("Index", "Home");
                else
                    return View(user);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        #endregion



        #region check_currentuser_role
        public async Task<bool> CheckUserRole(string _role)
        {
            //get user roles
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item.Equals(_role))
                    return true;
            }
            return false;
        }
        #endregion


    }
}
