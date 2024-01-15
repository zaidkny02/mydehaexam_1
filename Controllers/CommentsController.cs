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

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly ILessonService _lessonService;
        private readonly IClassDetailService _classDetailService;

        public CommentsController(MyDBContext context, ILessonService lessonService, UserManager<User> userManager, ICommentService commentService, IClassDetailService classDetailService)
        {
            _context = context;
            _lessonService = lessonService;
            _commentService = commentService;
            _userManager = userManager;
            _classDetailService = classDetailService;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Comment.Include(c => c.Lesson).Include(c => c.User);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Lesson)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> AddComment(string NewCommentContent, int LessonID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var lesson = await _lessonService.GetById(LessonID);
                bool check = false;
                var ListStudentInClass = await _classDetailService.GetAllByClass(lesson.ClassID);
                foreach(var item in ListStudentInClass)
                {
                    if (user.Id.Equals(item.UserID))
                    {
                        check = true;
                        break;
                    }
                }
                if(user.Id.Equals(lesson.Class.AuthorID))
                    check = true;
                if (check)
                {
                    var newComment = new CommentRequest
                    {
                        UserID = user.Id, // You may get this from the logged-in user
                        Content = NewCommentContent,
                        CommentDate = DateTime.Now,
                        LessonID = LessonID
                    };
                    var result = await _commentService.Create(newComment);
                    TempData["Message"] = result.message;
                    return RedirectToAction("Details", "Lessons", new { id = LessonID });
                }
                else
                {
                    TempData["Message"] = "Only student and teacher in this class can comment";
                    return RedirectToAction("Details", "Lessons", new { id = LessonID });
                }
            }
            else
            {
                TempData["Message"] = "Please login first";
                return RedirectToAction("Details", "Lessons", new {id = LessonID});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,CommentDate,LessonID,UserID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id", comment.LessonID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id", comment.LessonID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", comment.UserID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,CommentDate,LessonID,UserID")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id", comment.LessonID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Lesson)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'MyDBContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
