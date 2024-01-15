using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace deha_exam_quanlykhoahoc.Services
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly IClassService _classService;
        private readonly MyDBContext _context;
        public ClassDetailService(IClassService classService, MyDBContext context)
        {
            _classService = classService;
            _context = context;
        }

        public async Task<IEnumerable<ClassDetail>> GetAllByClass(int? classid)
        {
            var liststudent = await _context.ClassDetail.Include(x => x.User).Where(x => x.ClassID == classid)
                                                  .ToListAsync();
            return liststudent;
        }

        public async Task<ClassDetail> GetById(int? id)
        {
            var mystudentinclass = await _context.ClassDetail.AsNoTracking()
                 .Include(x => x.User)
                 .Include(x => x.Class)
                 .FirstOrDefaultAsync(m => m.Id == id);
            return mystudentinclass;
        }

        public async Task<Result> JoinClass(User user, int classid)
        {
            Result result = new Result();
            var myclass = await _classService.GetById(classid);
            if(myclass == null)
            {
                result.type = "NotFound";
                result.message = "Class not found!!";
            }
            else
            {
                // can setting condition to join class here
                if (ClassDetailExists(user.Id, myclass.Id))
                {
                    result.type = "Failure";
                    result.message = "You already join this class!!";
                    return result;
                }
                if(user.Id.Equals(myclass.AuthorID))
                {
                    result.type = "Failure";
                    result.message = "Can't join this class because you are class teacher!!";
                }
                else
                {
                    var newdetail = new ClassDetail
                    {
                        ClassID = myclass.Id,
                        UserID = user.Id
                    };
                    try
                    {
                        _context.Add(newdetail);
                        await _context.SaveChangesAsync();
                        result.type = "Success";
                        result.message = "Join class successful";
                    }
                    catch(Exception ex)
                    {
                        result.type = "Failure";
                        result.message = ex.ToString();
                    }
                }
            }
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            Result result = new Result();
            var myclassdetail = await _context.ClassDetail.FindAsync(id);
            try
            {
                if (myclassdetail != null)
                {
                    _context.ClassDetail.Remove(myclassdetail);
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

        private bool ClassDetailExists(string userid, int classid)
        {
            return _context.ClassDetail.Any(e => e.UserID.Equals(userid) && e.ClassID == classid);
        }
    }
}
