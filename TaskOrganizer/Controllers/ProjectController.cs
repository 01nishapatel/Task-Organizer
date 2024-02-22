using Microsoft.AspNetCore.Mvc;
using Database;
/*using System.Data.Entity;*/
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskOrganizer.Controllers
{
    public class ProjectController : Controller
    {
        private readonly TaskOrganizerDBContext _context;

        public ProjectController(TaskOrganizerDBContext context)
        {
            if (HttpContext == null || HttpContext.Session.GetInt32("UserId") == null)
            {
                GoToLogout();
            }
            _context = context;
        }

        [Route("Project/Board/{id:int}")]
        public async Task<IActionResult> Board(int? id, string search)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["headertitle"] = "Board";
            /*var tasktitle = from m in _context.Task select m;*/
            var vibe = HttpContext.Session.GetInt32("UserId");
            var u = _context.User.FirstOrDefault(z => z.UserID == vibe);
            bool isAd = u.IsAdmin;

            foreach (var obj1 in _context.Task.Where(s => s.ProjectID == id))
            {

                var dur = obj1.EndDate - DateTime.Now;

                if (dur.GetValueOrDefault().Days <= 0)
                {
                    obj1.Status = Database.Status.Reassigned;
                }
                _context.Task.Update(obj1);
            }


            await _context.SaveChangesAsync();

            if (isAd)
            {
                var tasktitle = from m in _context.Task.Where(x => x.ProjectID == id && x.UserId == HttpContext.Session.GetInt32("uid")) select m;

                if (!String.IsNullOrEmpty(search))
                {
                    tasktitle = tasktitle.Where(s => s.TaskTitle!.ToLower().Contains(search.ToLower()) || s.Description!.ToLower().Contains(search.ToLower()));
                }


                return View(await tasktitle.ToListAsync());
            }
            else
            {
                var tasktitle1 = from m in _context.Task.Where(x => x.ProjectID == id && x.UserId == HttpContext.Session.GetInt32("UserId")) select m;

                if (!String.IsNullOrEmpty(search))
                {
                    tasktitle1 = tasktitle1.Where(s => s.TaskTitle!.ToLower().Contains(search.ToLower()) || s.Description!.ToLower().Contains(search.ToLower()));
                }


                return View(await tasktitle1.ToListAsync());
            }

        }

        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProject([Bind(Prefix = "Item1")] Project project)
        {
            if (!ModelState.IsValid)
            {
                return View(Tuple.Create<Project, IEnumerable<Project>, IEnumerable<Database.Task>>(project, _context.Project.ToList(), _context.Task.ToList()));
            }
            int iid = 0;
            foreach (var i in _context.User)
            {
                if (i.UserName == HttpContext.Session.GetString("UserName"))
                {
                    iid = i.UserID;
                }
            }
            project.UserID = @iid;
            project.Duration = (project.EndDate - project.StartDate).GetValueOrDefault().Days;
            _context.Project.Add(project);
            _context.SaveChanges();
            return RedirectToAction("AllProject", Tuple.Create<Project, IEnumerable<Project>, IEnumerable<Database.Task>>(project, _context.Project.ToList(), _context.Task.ToList()));
        }

        public async Task<ActionResult> GoToLogout()
        {
            return RedirectToAction("Login", "Account");
        }




        public async Task<IActionResult> AllProject(ProjectAssign projectAssign, IEnumerable<Project> mi, IEnumerable<Result> mii, string Search)
        {
            ViewData["headertitle"] = "All Project";
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var vibe = HttpContext.Session.GetInt32("UserId");


            //User Login 
            mi = (from PJ in _context.Project.ToList()
                  join PJA in _context.ProjectAssign.Where(x => x.UserID == vibe).ToList() on PJ.ProjectID equals PJA.ProjectID
                  select new Project()
                  {
                      ProjectID = PJ.ProjectID,
                      StartDate = PJ.StartDate,
                      Description = PJ.Description,
                      Duration = PJ.Duration,
                      Key = PJ.Key,
                      EndDate = PJ.EndDate,
                      ProjectTitle = PJ.ProjectTitle,
                      UserID = PJ.UserID,
                  });

            // Admin
            mii = _context.Project.ToList().Where(x => x.UserID == vibe).Select(x => new Result
            {
                TotalMember = _context.ProjectAssign.Where(y => y.ProjectID == x.ProjectID).ToList().Count(),
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ProjectTitle = x.ProjectTitle,
                Key = x.Key,
                ProjectID = x.ProjectID,
                TotalTask = _context.Task.Where(y => y.ProjectID == x.ProjectID).ToList().Count()

            });


            var userdetail = _context.Project.FirstOrDefault(x => x.UserID == vibe);
            var userdetail2 = _context.ProjectAssign.FirstOrDefault(x => x.UserID == vibe);


            var str = "";
            var abc = _context.User.FirstOrDefault(x => x.UserID == vibe);
            bool isadmin = abc.IsAdmin;


            if (isadmin)
            {
                HttpContext.Session.SetInt32("projectid", userdetail.ProjectID);

                if (!String.IsNullOrEmpty(Search))
                {
                    mii = mii.Where(s => s.ProjectTitle!.ToLower().Contains(Search.ToLower()) || s.Description!.ToLower().Contains(Search.ToLower()) || s.Key!.ToLower().Contains(Search.ToLower()));
                    return View(Tuple.Create<Project, IEnumerable<Result>, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>>(new Project(), mii, _context.Project.ToList(), _context.Task.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList()));
                }
                else
                {

                    return View(Tuple.Create<Project, IEnumerable<Result>, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>>(new Project(), mii, _context.Project.ToList(), _context.Task.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList()));
                }
            }
            else
            {
                HttpContext.Session.SetInt32("projectid", userdetail2.ProjectID);



                if (!String.IsNullOrEmpty(Search))
                {
                    mi = mi.Where(s => s.ProjectTitle!.ToLower().Contains(Search.ToLower()) || s.Description!.ToLower().Contains(Search.ToLower()) || s.Key!.ToLower().Contains(Search.ToLower()));
                    return View(Tuple.Create<Project, IEnumerable<Result>, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>>(new Project(), mii, mi.ToList(), _context.Task.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList()));


                }
                else
                {

                    return View(Tuple.Create<Project, IEnumerable<Result>, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>>(new Project(), mii, mi.ToList(), _context.Task.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList()));
                }
            }

        }


        public class Result : Project
        {
            public int TotalMember { get; set; }
            public int TotalTask { get; set; }
        }

        [HttpGet]
        public IActionResult Details()
        {
            Project project = new Project();
            return View("Details", project);
        }

        public bool IsAsign(int UserId, int ProjectId)
        {
            return _context.Task.Any(d => d.UserId == UserId && d.ProjectID == ProjectId);
        }

        [Route("Project/UserDashBoard")]
        public IActionResult UserDashBoard(int uid, int id)
        {
            ViewData["headertitle"] = "DashBoard";
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var iid = HttpContext.Session.GetInt32("UserId");
            var PJA = _context.ProjectAssign.Where(m => m.UserID == uid && m.ProjectID == id).FirstOrDefault();


            var project = _context.Project.Where(s => s.ProjectID == id).FirstOrDefault();
            var user = _context.User.FirstOrDefault(n => n.UserID == iid);
            var istask = _context.Task.Where(c => c.UserId == uid && c.ProjectID == id);
            bool istaskassign = istask.Any();
            bool isadmin = user.IsAdmin;

            HttpContext.Session.SetString("ProjName", project.ProjectTitle ?? "");
            HttpContext.Session.SetString("SDate", project.StartDate.ToString());
            HttpContext.Session.SetInt32("uid", PJA.UserID);

            if (!isadmin)
            {
                HttpContext.Session.SetInt32("UserType", (int)PJA.UserType);
            }
            if (isadmin)
            {
                {
                    HttpContext.Session.SetInt32("ProjId", project.ProjectID);
                    return View(Tuple.Create<Project, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<ProjectAssign>, ProjectAssign>(project, _context.Project.ToList(), istask.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList(), PJA));
                }
            }
            else
            {
                HttpContext.Session.SetInt32("ProjId", PJA.ProjectID);
                return View(Tuple.Create<Project, IEnumerable<Project>, IEnumerable<Database.Task>, IEnumerable<Database.User>, IEnumerable<ProjectAssign>, ProjectAssign>(project, _context.Project.ToList(), istask.ToList(), _context.User.ToList(), _context.ProjectAssign.ToList(), PJA));
            }
        }


        [Route("Project/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return View(new Project());

            }
            else
            {
                return View(_context.Project.Find(id));


            }
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            _context.Project.Update(project);
            _context.SaveChanges();
            return RedirectToAction("AllProject", "Project");
        }

        [Route("Project/Calender/{id:int}")]
        public IActionResult Calender(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["headertitle"] = "Calendar";
            var project = _context.Project.Where(s => s.ProjectID == id).SingleOrDefault();
            return View(Tuple.Create<Project, IEnumerable<Project>, IEnumerable<Database.Task>>(project, _context.Project.ToList(), _context.Task.ToList()));
        }




        public JsonResult Calendar2()
        {
            ViewData["headertitle"] = "Calendar";
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var eventlist = from e in _context.Task.Where(j => j.UserId == HttpContext.Session.GetInt32("UserId") && j.ProjectID == HttpContext.Session.GetInt32("ProjId"))
                                select new
                                {
                                    title = e.TaskTitle,
                                    start = e.StartDate,
                                    end = e.EndDate
                                };
                var rows = eventlist.ToArray();
                return Json(rows);
            }
            else
            {

                return Json(new { redirectUrl = @Url.Action("logout", "Account") });

            }


        }

        public JsonResult AdminCalender2()
        {
            ViewData["headertitle"] = "Calendar";
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var eventlist = from e in _context.Project
                                select new
                                {
                                    title = e.ProjectTitle,
                                    start = e.StartDate,
                                    end = e.EndDate
                                };

                var rows = eventlist.ToArray();
                return Json(rows);
            }
            else
            {
                return Json(new { redirectUrl = @Url.Action("logout", "Account") });

            }
        }
        public IActionResult AdminCalender()
        {
            ViewData["headertitle"] = "Calendar";
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
