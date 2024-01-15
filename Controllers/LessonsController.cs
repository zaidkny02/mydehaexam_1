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
using System.Net.WebSockets;

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly UserManager<User> _userManager;
        private readonly IClassService _classService;
        private readonly MyDBContext _context;
        private readonly ICommentService _commentService;
        private readonly IFileinLessonService _fileinLessonService;

        public LessonsController(MyDBContext context, ILessonService lessonService, UserManager<User> userManager,IClassService classService, ICommentService commentService, IFileinLessonService fileinLessonService)
        {
            _lessonService = lessonService;
            _userManager = userManager;
            _classService = classService;
            _context = context;
            _commentService = commentService;
            _fileinLessonService = fileinLessonService;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Lesson.Include(l => l.Class);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var mylesson = await _lessonService.GetById(id);
            if (mylesson == null)
                return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                var listcomment = await _commentService.GetAllByLesson(id);
                var listfile = await _fileinLessonService.GetAllByLesson(id);
                ViewBag.listfile = listfile;
                ViewBag.listcomment = listcomment;
                var user = await _userManager.GetUserAsync(User);
                if(user.Id.Equals(mylesson.Class.AuthorID))
                {
                    ViewBag.master = "OK";
                }
                return View(mylesson);
            }
            else
            {
                TempData["Message"] = "Login to see lesson";
                return RedirectToAction("Details", "Classes", new { id= mylesson.ClassID });
            }
        }

            // GET: Lessons/Create
            public async Task<IActionResult> Create(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                    return RedirectToAction("Index", "Classes");
                else
                {
                    var user = await _userManager.GetUserAsync(User);
                    var myclass = await _classService.GetById(id);
                    if (user.Id.Equals(myclass.AuthorID))
                        ViewBag.ClassID = id;
                    else
                    {
                        TempData["Message"] = "Can't create lesson cause you aren't class's teacher";
                        return RedirectToAction("Details", "Classes", new { id = id });
                    }
                }
                // ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id");
                return View();
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Index", "Classes");
            }
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var myclass = await _classService.GetById(request.ClassID);
                if (user.Id.Equals(myclass.AuthorID))
                {
                    var result = await _lessonService.Create(request);
                    if (result.type.Equals("Success"))
                    {
                        TempData["Message"] = "Create lesson successfully";
                        return RedirectToAction("Details", "Classes", new { id = request.ClassID });
                    }
                    else
                    {
                        TempData["Message"] = result.message;
                        ViewBag.ClassID = request.ClassID;
                        return View(request);
                    }
                }
                else
                {
                    TempData["Message"] = "Can't create lesson cause you aren't class's teacher";
                    return RedirectToAction("Details", "Classes", new { id = request.ClassID });
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var lesson = await _lessonService.GetById(id);
                if (lesson == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(lesson.Class.AuthorID))
                {
                    ViewBag.ClassID = lesson.ClassID;
                    var listfile = await _fileinLessonService.GetAllByLesson(id);
                    ViewBag.listfile = listfile;
                    // ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id", lesson.ClassID);
                    return View(lesson);
                }
                else
                {
                    TempData["Message"] = "Can't edit lesson cause you aren't class's teacher";
                    return RedirectToAction("Details", "Lessons", new { id = lesson.Id });
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonViewModel lesson)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id != lesson.Id)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                var myclass = await _classService.GetById(lesson.ClassID);
                if (user.Id.Equals(myclass.AuthorID))
                {
                    var result = await _lessonService.Update(lesson);
                    if (result.type.Equals("Success"))
                    {
                        TempData["Message"] = "Update lesson successfully";
                        return RedirectToAction("Details", "Lessons", new { id = lesson.Id });
                    }

                    else
                    {
                        TempData["Message"] = result.message;
                        ViewBag.ClassID = lesson.ClassID;
                        var listfile = await _fileinLessonService.GetAllByLesson(id);
                        ViewBag.listfile = listfile;
                        return View(lesson);
                    }
                }
                else
                {
                    TempData["Message"] = "Can't edit lesson cause you aren't class's teacher";
                    return RedirectToAction("Details", "Lessons", new { id = lesson.Id });
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var lesson = await _lessonService.GetById(id);
                if (lesson == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(lesson.Class.AuthorID))
                {
                    return View(lesson);
                }
                else
                {
                    TempData["Message"] = "Can't delete lesson cause you aren't class's teacher";
                    return RedirectToAction("Details", "Lessons", new { id = lesson.Id });
                }
            }
            else
                return RedirectToAction("Index", "Classes");

        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var mylesson = await _lessonService.GetById(id);
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(mylesson.Class.AuthorID))
                {
                    var result = await _lessonService.Delete(id);
                    if (result.type.Equals("Success"))
                    {
                        TempData["Message"] = "Delete lesson successfully";
                        return RedirectToAction("Details", "Classes", new { id = mylesson.ClassID });
                    }
                    else
                    {
                        TempData["Message"] = result.message;
                        return RedirectToAction("Delete", new { id = id });
                    }
                }
                else
                {
                    TempData["Message"] = "Can't delete lesson cause you aren't class's teacher";
                    return RedirectToAction("Details", "Lessons", new { id = mylesson.Id });
                }
            }
            else
                return RedirectToAction("Index", "Classes");
        }

        private bool LessonExists(int id)
        {
          return (_context.Lesson?.Any(e => e.Id == id)).GetValueOrDefault();
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
