using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace deha_exam_quanlykhoahoc.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IValidator<RegisterViewModel> _validator;
        private readonly IValidator<AccountManagerModel> _accountmanagervalidator;
        private readonly string UserRoleName = "Member";
        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IValidator<RegisterViewModel> validator, IValidator<AccountManagerModel> accountmanagervalidator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
            _accountmanagervalidator = accountmanagervalidator;
        }
        #region createaccount
        public async Task<Result> CreateAccount(RegisterViewModel model)
        {
            Result result = new Result();
            ValidationResult validationResult = _validator.Validate(model);

            if (validationResult.IsValid)
            {
                var userNameExists = await _userManager.FindByNameAsync(model.UserName) != null;
                if (userNameExists)
                {
                    result.type = "Failure";
                    result.message = "Username is already taken. Please choose a different username.";
                    return result;
                }

                var user = new User { UserName = model.UserName, Email = model.Email, Dob = model.Dob, FullName = model.FullName };

                //identity result when create
                var createresult = await _userManager.CreateAsync(user, model.Password);

                if (createresult.Succeeded)
                {
                    //Add auto role = member
                    await _userManager.AddToRoleAsync(user, UserRoleName);
                    // Automatically sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    result.type = "Success";
                    result.message = "Register Success.";
                    return result;
                }

                result.type = "Failure";
                foreach (var error in createresult.Errors)
                {
                    result.message = result.message + error.Description + "\n";
                }
                return result;

            }

            result.type = "Failure";
            result.message = "Model isn't valid";
            return result;
        }
        #endregion
        #region login
        public async Task<Result> Login(LoginViewModel model)
        {
            Result result = new Result();
            var loginresult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (loginresult.Succeeded)
            {
                result.type = "Success";
                result.message = "Login Success";
                return result;
            }
            else
            {
                result.type = "Failure";
                if (loginresult.IsNotAllowed || loginresult.RequiresTwoFactor || loginresult.IsNotAllowed)
                    result.message = "Invalid login attempt.";
                else
                    result.message = "Invalid username or password.";
                return result;
            }
        }
        #endregion
        #region logout
        public async Task<int> Logout()
        {
            await _signInManager.SignOutAsync();
            return 1;
        }
        #endregion
        #region changeuserrole
        public async Task<Result> ChangeUserRole(string id, string newRole)
        {
            Result result = new Result();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Handle the case where the user is not found
                result.type = "NotFound";
                result.message = "User not found.";
                return result;
            }

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            // Find the role by name
            var roleExists = await _roleManager.RoleExistsAsync(newRole);

            if (!roleExists)
            {
                // Handle the case where the role is not found
                result.type = "NotFound";
                result.message = "Role not found.";
                return result;
            }

            await _userManager.AddToRoleAsync(user, newRole);

            // Redirect to a success page or return a success message
            result.type = "Success";
            result.message = "Change user role successfully.";
            return result;
        }
        #endregion
        #region getuserbyid
        public async Task<User> getUserbyID(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        #endregion
        #region getroleofuser
        public async Task<IList<string>> getRolesofUser(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        #endregion
        #region getlistrole
        public async Task<List<IdentityRole>> getlistRole()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        #endregion
        #region getlistUserwithRole
        public async Task<List<ListAccountViewModel>> getlistUserwithRole()
        {
            // Get all user identities
            var users = await _userManager.Users.ToListAsync();
            // Create a list to store user information along with roles
            var usersWithRoles = new List<ListAccountViewModel>();

            foreach (var user in users)
            {
                // Get roles for the current user
                var roles = await _userManager.GetRolesAsync(user);

                // Create a view model to store user information along with roles
                var userWithRoles = new ListAccountViewModel(user.Id, user.UserName, user.FullName, user.Dob, user.Email, roles);

                // Add the view model to the list
                usersWithRoles.Add(userWithRoles);
            }
            return usersWithRoles;
        }
        #endregion
        #region getUserInRole
        public async Task<List<ListAccountViewModel>> getUserInRole(string rolename)
        {
            var mylist = new List<ListAccountViewModel>();
            try
            {
                mylist = await this.getlistUserwithRole();
                mylist.RemoveAll(x => x.Roles[0].Equals(rolename) == false);
                
                return mylist;
            }
            catch (Exception ex) { return mylist; }
        }

        public async Task<Result> UpdateAccount(AccountManagerModel model)
        {
            Result result = new Result();
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                result.type = "NotFound";
                result.message = "NotFound";
            }
            else
            {
                ValidationResult validationResult = _accountmanagervalidator.Validate(model);
                if (validationResult.IsValid)
                {
                    try
                    {
                        user.Dob = model.Dob;
                        user.FullName = model.FullName;
                        user.Email = model.Email;
                        var updatemessage = await _userManager.UpdateAsync(user);
                        if (updatemessage.Succeeded)
                        {
                            result.type = "Success";
                            result.message = "Update Account Successfully";
                        }
                        else
                        {
                            // You can inspect the result.Errors property to get details about the errors
                            result.type = "Failure";
                            foreach (var err in updatemessage.Errors)
                                result.message = result.message + err + "\n";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.type = "Failure";
                        result.message = ex.Message;
                    }
                }
                else
                {
                    result.type = "Failure";
                    result.message = "Model isn't valid";
                }
            }
            return result;
        }
        #endregion

    }
}
