using AutoMapper;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace deha_exam_quanlykhoahoc.Services
{
    public class LessonService : ILessonService
    {
        private readonly MyDBContext _context;
        private readonly IClassService _classService;
        private readonly IMapper _mapper;
        private readonly IFileinLessonService _fileinLessonService;
        private readonly IValidator<LessonRequest> _lessonrequestvalidator;
        private readonly IValidator<LessonViewModel> _lessonviewmodelvalidator;
        private readonly IEmailService _emailService;
        private readonly IClassDetailService _classDetailService;
        public LessonService(MyDBContext context, IClassService classService, IMapper mapper, IValidator<LessonViewModel> lessonviewmodelvalidator, IValidator<LessonRequest> lessonrequestvalidator, IFileinLessonService fileinLessonService, IEmailService emailService, IClassDetailService classDetailService)
        {
            _context = context;
            _classService = classService;
            _mapper = mapper;
            _lessonviewmodelvalidator = lessonviewmodelvalidator;
            _lessonrequestvalidator = lessonrequestvalidator;
            _fileinLessonService = fileinLessonService;
            _emailService = emailService;
            _classDetailService = classDetailService;
        }

        public async Task<Result> Create(LessonRequest lessonrequest)
        {
            Result result = new Result();
            var myclass = await _classService.GetById(lessonrequest.ClassID);
            if (myclass == null)
            {
                result.type = "NotFound";
                result.message = "Class not found!!";
            }
            else
            {
                ValidationResult validationResult = _lessonrequestvalidator.Validate(lessonrequest);
                if (validationResult.IsValid)
                {
                    try
                    {
                        var lesson = _mapper.Map<Lesson>(lessonrequest);
                        _context.Add(lesson);
                        await _context.SaveChangesAsync();
                        int lessonid = lesson.Id;
                        if (lessonrequest.lsfile != null)
                        {
                            foreach (var item in lessonrequest.lsfile)
                            {
                                var filemodel = new FileInLessonModel
                                {
                                    name = item.FileName,
                                    file = item,
                                    LessonID = lessonid
                                };
                                result = await _fileinLessonService.Create(filemodel);
                            }
                        }
                        result.type = "Success";
                        result.message = "Add lesson success!";
                        //send email
                        var liststudent = await _classDetailService.GetAllByClass(lesson.ClassID);
                        if(liststudent.Count() > 0)
                        {
                            foreach(var item in liststudent)
                                await _emailService.SendEmailAsync(item.User.Email, "Teacher just add lesson "+ lesson.Title + " in " + myclass.Title +" class", "lesson created at "+ lesson.DateCreated);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.type = "Failure";
                        result.message = ex.ToString();
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

        public async Task<Result> Delete(int id)
        {
            Result result = new Result();
            var lesson = await _context.Lesson.FindAsync(id);
            try
            {
                if (lesson != null)
                {
                    _context.Lesson.Remove(lesson);
                    await _context.SaveChangesAsync();
                    result.type = "Success";
                    result.message = "Success";
                    return result;
                }
                result.type = "NotFound";
                result.message = "NotFound";
                return result;
            }
            catch (Exception ex)
            {
                //   throw new Exception("Can't remove course because : " + ex);
                result.type = "Failure";
                result.message = ex.ToString();
                return result;
            }
        }


        public async Task<IEnumerable<LessonViewModel>> GetAllByClass(int? classid)
        {

            var lesson = await _context.Lesson.Where(x => x.ClassID == classid).
                ToListAsync();
            return _mapper.Map<IEnumerable<LessonViewModel>>(lesson);
        }

        public async Task<LessonViewModel> GetById(int? id)
        {
            var lesson = await _context.Lesson
                .Include(x => x.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<LessonViewModel>(lesson);
        }

        public async Task<Result> Update(LessonViewModel lessonviewmodel)
        {
            Result result = new Result();
            if (!LessonExists(lessonviewmodel.Id))
            {
                // throw new Exception("Lesson does not exist");
                result.type = "NotFound";
                result.message = "NotFound";
                return result;
            }
            ValidationResult validationResult = _lessonviewmodelvalidator.Validate(lessonviewmodel);
            if (validationResult.IsValid)
            {
                try
                {
                    /*  if (lesson.Image != null)
                      {
                          if (!string.IsNullOrEmpty(lesson.ImagePath))
                              await _storageService.DeleteFileAsync(lesson.ImagePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                          lesson.ImagePath = await SaveFile(lesson.Image);
                      }    */
                    int lessonid = lessonviewmodel.Id;
                    if (lessonviewmodel.lsfile != null)
                    {
                        foreach (var item in lessonviewmodel.lsfile)
                        {
                            var filemodel = new FileInLessonModel
                            {
                                name = item.FileName,
                                file = item,
                                LessonID = lessonid
                            };
                            result = await _fileinLessonService.Create(filemodel);
                        }
                    }
                    _context.Update(_mapper.Map<Lesson>(lessonviewmodel));
                    await _context.SaveChangesAsync();
                    result.type = "Success";
                    result.message = "Success";
                    return result;
                }
                catch (Exception ex)
                {
                    result.type = "Failure";
                    result.message = ex.ToString();
                    return result;
                }
            }
            else
            {
                result.type = "Failure";
                /* foreach(var err in validationResult.Errors)
                 {
                     result.message = result.message + err.ErrorMessage + "\n";
                 } */
                result.message = "Model isn't valid";
                return result;
            }
        }
        private bool LessonExists(int id)
        {
            return (_context.Lesson?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
