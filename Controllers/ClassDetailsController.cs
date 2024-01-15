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
using deha_exam_quanlykhoahoc.ViewModels;
using X.PagedList;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;

namespace deha_exam_quanlykhoahoc.Controllers
{
    public class ClassDetailsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IClassDetailService _classDetailService;
        private readonly UserManager<User> _userManager;
        private readonly IClassService _classService;

        public ClassDetailsController(MyDBContext context , IClassDetailService classDetailService, UserManager<User> userManager, IClassService classService)
        {
            _context = context;
            _classDetailService = classDetailService;
            _userManager = userManager;
            _classService = classService;
        }

        // GET: ClassDetails
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.ClassDetail.Include(c => c.Class).Include(c => c.User);
            return View(await myDBContext.ToListAsync());
        }

        // GET: ClassDetails/Details/5
        public async Task<IActionResult> Details(int? id, int? page)
        {
            if (id == null)
            {
                return NotFound();
            }
            var myclass = await _classService.GetById(id);
            if (myclass == null)
                return NotFound();
            var liststudent = await _classDetailService.GetAllByClass(id);
            int pageSize = 3;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<ClassDetail> lst = new PagedList<ClassDetail>(liststudent, pageNumber, pageSize);
            if (liststudent.Count() == 0)
            {
                TempData["Message"] = "Class doesn't have any student";
                ViewBag.classid = id;
                return View(lst);

            }
            //check remove
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(myclass.AuthorID))
                    ViewBag.Master = "OK";
            }
            ViewBag.teachername = myclass.Author.FullName;
            ViewBag.classid = id;
            return View(lst);
        }

        // GET: ClassDetails/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ClassDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassID,UserID")] ClassDetail classDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id", classDetail.ClassID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", classDetail.UserID);
            return View(classDetail);
        }

        // GET: ClassDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassDetail == null)
            {
                return NotFound();
            }

            var classDetail = await _context.ClassDetail.FindAsync(id);
            if (classDetail == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id", classDetail.ClassID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", classDetail.UserID);
            return View(classDetail);
        }

        // POST: ClassDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassID,UserID")] ClassDetail classDetail)
        {
            if (id != classDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassDetailExists(classDetail.Id))
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
            ViewData["ClassID"] = new SelectList(_context.Class, "Id", "Id", classDetail.ClassID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", classDetail.UserID);
            return View(classDetail);
        }

        // GET: ClassDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var result = await _classDetailService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: ClassDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _classDetailService.GetById(id);
            if (User.Identity.IsAuthenticated)
            {
                
                if (result == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (!user.Id.Equals(result.Class.AuthorID))
                {
                    TempData["Message"] = "Can't delete cause you aren't class teacher";
                    return RedirectToAction("Details", "ClassDetails",new { Id = result.Class.Id});
                }
                var final = await _classDetailService.Delete(id);

                if (final.type.Equals("Success"))
                {
                    TempData["Message"] = final.message;
                    return RedirectToAction("Details", "ClassDetails", new { Id = result.Class.Id });
                }
                else
                {
                    TempData["Message"] = final.message;
                    return View(result);
                }
            }
            else
                return RedirectToAction("Details", "ClassDetails", new { Id = result.Class.Id });
        }

        private bool ClassDetailExists(int id)
        {
          return (_context.ClassDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
