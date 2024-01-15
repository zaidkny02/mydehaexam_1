using AutoMapper;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace deha_exam_quanlykhoahoc.Services
{
    public class ClassService : IClassService
    {
        private readonly IValidator<ClassViewModel> _classviewvalidator;
        private readonly IValidator<ClassRequest> _classrequestvalidator;
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public ClassService(MyDBContext context, IMapper mapper, IValidator<ClassRequest> classrequestvalidator , IValidator<ClassViewModel> classviewvalidator)
        {
            _context = context;
            _mapper = mapper;
            _classviewvalidator = classviewvalidator;
            _classrequestvalidator = classrequestvalidator;
        }

        public async Task<Result> Create(ClassRequest request)
        {
            Result result = new Result();
            ValidationResult validationResult = _classrequestvalidator.Validate(request);
            if (validationResult.IsValid)
            {
                try
                {
                    var myclass = _mapper.Map<Class>(request);
                    _context.Add(myclass);
                    await _context.SaveChangesAsync();
                    result.type = "Success";
                    result.message = myclass.Id.ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    //ERROR ex
                    result.type = "Failure";
                    result.message = ex.ToString();
                    return result;
                }
            }
            result.type = "Failure";
            result.message = "Model isn't valid";
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            Result result = new Result();
            var myclass = await _context.Class.FindAsync(id);
            try
            {
                if (myclass != null)
                {
                    _context.Class.Remove(myclass);
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

        public async Task<IEnumerable<ClassViewModel>> GetAll()
        {
            var listclass = await _context.Class.Include(x => x.Author).ToListAsync();
            return _mapper.Map<IEnumerable<ClassViewModel>>(listclass);
        }

        public async Task<IEnumerable<ClassViewModel>> GetAllbyUser(string userid)
        {
            var listclassbyuser = await _context.ClassDetail.Where(x => x.UserID == userid).ToListAsync();
            List<int> listid = new List<int>();
            foreach (var item in listclassbyuser)
                listid.Add(item.ClassID);
            var listclass = await _context.Class.Include(x => x.Author).Where(x => listid.Contains(x.Id)).ToListAsync();
            var addclass = await _context.Class.Include(x => x.Author).Where(x => x.AuthorID == userid).ToListAsync();
            listclass.AddRange(addclass);
            return _mapper.Map<IEnumerable<ClassViewModel>>(listclass);
        }

        public async Task<ClassViewModel> GetById(int? id)
        {
            var myclass = await _context.Class.AsNoTracking()
                 .Include(x => x.Author)
                 .FirstOrDefaultAsync(m => m.Id == id) ;
            return _mapper.Map<ClassViewModel>(myclass);
        }

        public async Task<Result> Update(ClassViewModel request)
        {
            Result result = new Result();
            ValidationResult validationResult = _classviewvalidator.Validate(request);
            if (validationResult.IsValid)
            {
                if (!ClassExists(request.Id))
                {
                    result.type = "NotFound";
                    result.message = "NotFound";
                    return result;
                    // throw new Exception("Course does not exist");
                }
                try
                {
                    _context.Update(_mapper.Map<Class>(request));
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
            result.type = "Failure";
            result.message = "Model isn't Valid";
            return result;
        }


        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.Id == id);
        }
    }
}
