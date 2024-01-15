using AutoMapper;
using deha_exam_quanlykhoahoc.Models;
using deha_exam_quanlykhoahoc.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace deha_exam_quanlykhoahoc.Services
{
    public class FileinLessonService : IFileinLessonService
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public FileinLessonService(MyDBContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<Result> Create(FileInLessonModel fileinlesson)
        {
            Result result = new Result();
            var file = _mapper.Map<FileinLesson>(fileinlesson);
            if (fileinlesson.file != null)
            {
                file.filePath = await SaveFile(fileinlesson.file);
            }
            try
            {
                _context.Add(file);
                await _context.SaveChangesAsync();
                result.type = "Success";
                result.message = "Save File Success";
            }
            catch (Exception ex)
            {
                result.type = "Failure";
                result.message = ex.ToString();
            }
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            Result result = new Result();
            var file = await _context.FileinLesson.FindAsync(id);
            try
            {
                if (file != null)
                {
                    if (!string.IsNullOrEmpty(file.filePath))
                        await _storageService.DeleteFileAsync(file.filePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                    _context.FileinLesson.Remove(file);
                    await _context.SaveChangesAsync();
                    result.type = "Success";
                    result.message = "Delete File Success";
                    return result;
                }
                result.type = "NotFound";
                result.message = "NotFound";
                return result;
            }
            catch (Exception ex)
            {
                result.type = "Failure";
                result.message = ex.ToString();
                return result;
            }
        }

        public async Task<IEnumerable<FileInLessonModel>> GetAllByLesson(int? lessonid)
        {
            var listfile = await _context.FileinLesson.Where(x => x.LessonID == lessonid).
                ToListAsync();
            return _mapper.Map<IEnumerable<FileInLessonModel>>(listfile);
        }

        public async Task<FileInLessonModel> GetById(int? id)
        {
            var file = await _context.FileinLesson.AsNoTracking()
                .Include(l => l.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<FileInLessonModel>(file);
        }

        public async Task<Result> Update(FileInLessonModel fileinlesson)
        {
            Result result = new Result();
            if (!FileExists(fileinlesson.Id))
            {
                // throw new Exception("Lesson does not exist");
                result.type = "NotFound";
                result.message = "NotFound";
                return result;
            }
            try
            {
                if (fileinlesson.file != null)
                {
                    if (!string.IsNullOrEmpty(fileinlesson.filePath))
                        await _storageService.DeleteFileAsync(fileinlesson.filePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                    fileinlesson.filePath = await SaveFile(fileinlesson.file);
                    //change name 
                    fileinlesson.name = fileinlesson.file.FileName;
                }
               
                _context.Update(_mapper.Map<FileinLesson>(fileinlesson));
                await _context.SaveChangesAsync();
                result.type = "Success";
                result.message = "Update File Success";
                return result;
            }
            catch (Exception ex)
            {
                result.type = "Failure";
                result.message = ex.ToString();
                return result;
            }
        }

        private bool FileExists(int id)
        {
            return (_context.FileinLesson?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
