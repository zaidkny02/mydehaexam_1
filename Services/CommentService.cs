using AutoMapper;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace deha_exam_quanlykhoahoc.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly MyDBContext _context;
        private readonly ILessonService _lessionService;
        private readonly IValidator<CommentRequest> _Commentrequestvalidator;
        private readonly IValidator<CommentViewModel> _Commentviewmodelvalidator;
        public CommentService(IMapper mapper, MyDBContext context, ILessonService lessionService, IValidator<CommentRequest> commentrequestvalidator, IValidator<CommentViewModel> commentviewmodelvalidator)
        {
            _mapper = mapper;
            _context = context;
            _lessionService = lessionService;
            _Commentrequestvalidator = commentrequestvalidator;
            _Commentviewmodelvalidator = commentviewmodelvalidator;
        }

        public async Task<Result> Create(CommentRequest request)
        {
            Result result = new Result();
            var lesson = await _lessionService.GetById(request.LessonID);
            if (lesson == null)
            {
                result.type = "Failure";
                result.type = "Can't find the lesson";
            }
            else
            {
                ValidationResult validationResult = _Commentrequestvalidator.Validate(request);
                if (validationResult.IsValid)
                {
                    try
                    {
                        var comment = _mapper.Map<Comment>(request);
                        _context.Add(comment);
                        await _context.SaveChangesAsync();
                        result.type = "Success";
                        result.message = "Add comment success!";
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
            var comment = await _context.Comment.FindAsync(id);
            try
            {
                if (comment != null)
                {
                    _context.Comment.Remove(comment);
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

        public async Task<IEnumerable<CommentViewModel>> GetAllByLesson(int? lessonID)
        {
            var comment = await _context.Comment
                .Include(x => x.Lesson)
                .Include(x => x.User)
                .Where(x => x.LessonID == lessonID).
               ToListAsync();
            return _mapper.Map<IEnumerable<CommentViewModel>>(comment);
        }

        public async Task<CommentViewModel> GetById(int? id)
        {
            var comment = await _context.Comment
                .Include(x => x.Lesson)
                .Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<CommentViewModel>(comment);
        }

        public async Task<Result> Update(CommentViewModel commentviewmodel)
        {
            Result result = new Result();
            if (!CommentExists(commentviewmodel.Id))
            {
                result.type = "NotFound";
                result.message = "NotFound";
                return result;
            }
            ValidationResult validationResult = _Commentviewmodelvalidator.Validate(commentviewmodel);
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
                    _context.Update(_mapper.Map<Comment>(commentviewmodel));
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
                result.message = "Model isn't valid";
                return result;
            }
        }

        private bool CommentExists(int id)
        {
            return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
