using School.Models;
using Microsoft.AspNetCore.Mvc;

namespace School.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }
        //GET : Teacher/List
        public IActionResult List()
        {
            List<Teacher> Teachers = _api.ListTeachers();
            return View(Teachers);
        }

        //GET : Teacher/Show/{id}
        public IActionResult Show(int id)
        {
            Teacher SelectedAuthor = _api.FindTeacher(id);
            return View(SelectedAuthor);
        }
    }
}
