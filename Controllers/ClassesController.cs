using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using deha_exam_quanlykhoahoc;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.Services;
using Microsoft.AspNetCore.Identity;
using deha_exam_quanlykhoahoc.ViewModels;
using X.PagedList;
using System.Collections;

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassService _classService;
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IClassDetailService _classdetailService;
        private readonly ILessonService _lessonService;

        public ClassesController(IClassService classService,UserManager<User> userManager, IAccountService accountService, IClassDetailService classdetailService, ILessonService lessonService)
        {
            _classService = classService;
            _userManager = userManager;
            _accountService = accountService;
            _classdetailService = classdetailService;
            _lessonService = lessonService;
        }

        // GET: Classes
        public async Task<IActionResult> Index(int? page,string? myclass)
        {
            IEnumerable<ClassViewModel> listclass = await _classService.GetAll();
            int pageSize = 3;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.Userid = user.Id;
                var type = myclass != null ? myclass : "allclass";
                if(type.Equals("myclass"))
                {
                    listclass = await _classService.GetAllbyUser(user.Id);
                }
                ViewBag.myclass = type;
            }
            PagedList<ClassViewModel> lst = new PagedList<ClassViewModel>(listclass, pageNumber, pageSize);
            return View(lst);
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myclass = await _classService.GetById(id);
            if (myclass == null)
            {
                return RedirectToAction("Index","Classes");
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(myclass.AuthorID))
                {
                    ViewBag.master = "OK";
                }

                var liststudent = await _classdetailService.GetAllByClass(id);
                foreach(var item in liststudent)
                {
                    if(item.UserID.Equals(user.Id))
                    {
                        ViewBag.student = "OK";
                        break;
                    }
                }

            }
            var listlesson = await _lessonService.GetAllByClass(id);
            int pageSize = 3;
            int pageNumber = 1;
            PagedList<LessonViewModel> lst = new PagedList<LessonViewModel>(listlesson, pageNumber, pageSize);
            ViewBag.listlesson = lst;

            

            return View(myclass);
        }

        public async Task<IActionResult> ListLesson(int? page,int ClassID)
        {
            int pageSize = 3;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var mylist = await _lessonService.GetAllByClass(ClassID);
            PagedList<LessonViewModel> lst = new PagedList<LessonViewModel>(mylist, pageNumber, pageSize);
            // return View(lst);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Nếu đây là một yêu cầu AJAX, trả về một PartialView
                //   return PartialView("ListNCC", lst);
                return PartialView("MyViewForListLesson", lst);
            }

            return View(lst);
        }

        // GET: Classes/Create
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
               /* if (await CheckUserRole("Admin"))
                {
                    ViewData["AuthorID"] = new SelectList(await _accountService.getUserInRole("Teacher"), "Id", "FullName");
                    return View();
                }
                if(await CheckUserRole("Teacher"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    ViewBag.myid = user.Id;
                    return View();
                }
                TempData["Message"] = "Don't have right to create";
                return RedirectToAction("Index", "Classes");  */
                var user = await _userManager.GetUserAsync(User);
                ViewBag.myid = user.Id;
                return View();
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Index", "Classes");
            }
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassRequest myclass)
        {
            if (User.Identity.IsAuthenticated)
            {
               /* if (await CheckUserRole("Admin") || await CheckUserRole("Teacher"))
                {
                    var result = await _classService.Create(myclass);
                    if (result.type.Equals("Success"))
                    {
                        TempData["Message"] = result.message;
                        return RedirectToAction("Index", "Classes");
                    }
                    else
                    {
                        TempData["Message"] =  result.message;
                        return View(myclass);
                    }
                }
                else
                    return RedirectToAction("Index", "Classes");   */

                var result = await _classService.Create(myclass);
                if (result.type.Equals("Success"))
                {
                    TempData["Message"] = "Create Class Success";
                    return RedirectToAction("Details", "Classes", new {id = result.message});
                }
                else
                {
                    TempData["Message"] = result.message;
                    return View(myclass);
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var result = await _classService.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (!user.Id.Equals(result.AuthorID))
                {
                    TempData["Message"] = "Can't edit this class cause you aren't class teacher";
                    return RedirectToAction("Details", "Classes", new { id = result.Id });
                }
                //   ViewData["AuthorID"] = new SelectList(await _accountService.getUserInRole("Teacher"), "Id", "FullName");
                ViewData["AuthorID"] = result.AuthorID;
                return View(result);
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Index", "Classes");
            }
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassViewModel myclass)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id != myclass.Id)
                {
                    return NotFound();
                }
                var result = await _classService.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (!user.Id.Equals(result.AuthorID))
                {
                    TempData["Message"] = "Can't edit this class cause you aren't class teacher";
                    return RedirectToAction("Details", "Classes",new {id = myclass.Id});
                }
                var final = await _classService.Update(myclass);
                //message
                TempData["Message"] = final.message;
                return RedirectToAction("Details", "Classes", new {id = myclass.Id});
            }
            else
                return RedirectToAction("Index", "Classes");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinClass(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _classdetailService.JoinClass(user, Id);
                if (result.type.Equals("Success"))
                {
                    TempData["Message"] = result.message;
                    return RedirectToAction("Details", "Classes", new {id = Id});
                }
                if (result.type.Equals("Failure"))
                {
                    TempData["Message"] = result.message;
                    return RedirectToAction("Details", "Classes", new { id = Id });
                }
                else
                {
                    TempData["Message"] = result.message;
                    return RedirectToAction("Index", "Classes");
                }
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Details", "Classes", new { id = Id });
            }
        }


        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _classService.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (!user.Id.Equals(result.AuthorID))
                {
                    TempData["Message"] = "Can't delete this class cause you aren't class teacher";
                    return RedirectToAction("Details", "Classes", new { id = id });
                }
                return View(result);
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Index", "Classes");
            }
           
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _classService.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (!user.Id.Equals(result.AuthorID))
                {
                    TempData["Message"] = "Can't delete this class cause you aren't class teacher";
                    return RedirectToAction("Index", "Classes");
                }
                var final = await _classService.Delete(id);

                if(final.type.Equals("Success"))
                {
                    TempData["Message"] = final.message;
                    return RedirectToAction("Index", "Classes");
                }
                else
                {
                    TempData["Message"] = final.message;
                    return View(result);
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

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
