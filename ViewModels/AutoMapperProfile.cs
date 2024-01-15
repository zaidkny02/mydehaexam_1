using AutoMapper;
using deha_exam_quanlykhoahoc.Models;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Class, ClassViewModel>();
            CreateMap<ClassViewModel, Class>();
            CreateMap<ClassRequest, Class>();
            CreateMap<Lesson, LessonViewModel>();
            CreateMap<LessonViewModel, Lesson>();
            CreateMap<LessonRequest, Lesson>();
            CreateMap<FileinLesson, FileInLessonModel>();
            CreateMap<FileInLessonModel, FileinLesson>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<CommentRequest, Comment>();
            CreateMap<User, AccountManagerModel>();
        }
    }
}
