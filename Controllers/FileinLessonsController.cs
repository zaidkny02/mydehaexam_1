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
using Microsoft.AspNetCore.StaticFiles;
using deha_exam_quanlykhoahoc.ViewModels;

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class FileinLessonsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IFileinLessonService _fileinLessonService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileinLessonsController(MyDBContext context, IFileinLessonService fileinLessonService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _fileinLessonService = fileinLessonService;
            _hostingEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Download(int fileId)
        {
            var myfile = await _fileinLessonService.GetById(fileId);
            if (myfile == null)
                return NotFound();
            else
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
          //      var filePath = Path.Combine(webRootPath, myfile.filePath);
                var filePath = Path.GetFullPath(Path.Combine(webRootPath, myfile.filePath.TrimStart('/')));
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
                // Infer content type based on file extension
                var contentTypeProvider = new FileExtensionContentTypeProvider();
                if (!contentTypeProvider.TryGetContentType(myfile.name, out var contentType))
                {
                    contentType = "application/octet-stream"; // Default content type if not found
                }
                return PhysicalFile(filePath, contentType, myfile.name);
            }
        }
        // GET: FileinLessons
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.FileinLesson.Include(f => f.Lesson);
            return View(await myDBContext.ToListAsync());
        }

        // GET: FileinLessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileinLesson == null)
            {
                return NotFound();
            }

            var fileinLesson = await _context.FileinLesson
                .Include(f => f.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileinLesson == null)
            {
                return NotFound();
            }

            return View(fileinLesson);
        }

        // GET: FileinLessons/Create
        public IActionResult Create()
        {
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id");
            return View();
        }

        // POST: FileinLessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,filePath,LessonID")] FileinLesson fileinLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileinLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id", fileinLesson.LessonID);
            return View(fileinLesson);
        }

        // GET: FileinLessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var fileinLesson = await _fileinLessonService.GetById(id);
            if (fileinLesson == null)
            {
                return NotFound();
            }
           // ViewData["LessonID"] = new SelectList(_context.Lesson, "Id", "Id", fileinLesson.LessonID);
            return View(fileinLesson);
        }

        // POST: FileinLessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FileInLessonModel fileInLessonModel)
        {
            if (id != fileInLessonModel.Id)
            {
                return NotFound();
            }
            var fileinLesson = await _fileinLessonService.GetById(id);
            var result = await _fileinLessonService.Update(fileInLessonModel);
            TempData["Message"] = result.message;
            return RedirectToAction("Edit", "Lessons", new { id = fileinLesson.LessonID });
        }

        // GET: FileinLessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var fileinLesson = await _fileinLessonService.GetById(id);
            if (fileinLesson == null)
            {
                return NotFound();
            }

            return View(fileinLesson);
        }

        // POST: FileinLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileinLesson = await _fileinLessonService.GetById(id);
            var result = await _fileinLessonService.Delete(id);
            TempData["Message"] = result.message;
            return RedirectToAction("Edit", "Lessons", new { id = fileinLesson.LessonID });
        }

        private bool FileinLessonExists(int id)
        {
          return (_context.FileinLesson?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
