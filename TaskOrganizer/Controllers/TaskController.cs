using Microsoft.AspNetCore.Mvc;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Controllers
{
    public class TaskController : Controller
    {
        private TaskOrganizerDBContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public TaskController(TaskOrganizerDBContext db, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }
        public IActionResult AddTask()
        {

            var UserTypeId = HttpContext.Session.GetInt32("UserType");
            var UserID = HttpContext.Session.GetInt32("UserId");

            TaskOrganizer.ViewModel.TaskUserViewModel obj = new TaskOrganizer.ViewModel.TaskUserViewModel();

            var userProjectDetail = _db.ProjectAssign.Where(x => x.UserID == UserID && (int)x.UserType == UserTypeId ).Select(x => x.ProjectID).ToList(); 

            obj.ProjectList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_db.Project.Where(x => userProjectDetail.Contains(x.ProjectID)), "ProjectID", "ProjectTitle");
/*            obj.UserList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_db.User, "UserID", "UserName");*/

            return View(obj);
        }

        public class UserDetailModel
        {
            public int UserId { get; set; }
            public string UserName { get; set; } = string.Empty;
        }

        [HttpGet]
        public List<UserDetailModel> GetUserbyProductId(int ProjectId)
        {
            if (ProjectId < 0)
                return new List<UserDetailModel>();
            return _db.ProjectAssign.Where(x => x.ProjectID == ProjectId).ToList().Join(_db.User, x => x.UserID, y => y.UserID, (x, y) => new UserDetailModel { UserId = y.UserID, UserName = y.UserName ?? "" }).ToList();

        }

        [HttpPost]
        public async Task<string> UpdateTaskDetails(Status status, int id)
        {
            var old = await _db.Task.FirstOrDefaultAsync(t => t.TaskID == id);
            if (old != null)
            {
                old.Status = status;
            }
            _db.SaveChanges();
            return "";
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskUserViewModel taskUserViewModel)
        {
            try
            {
                if (taskUserViewModel.Attachments != null)
                {
                    string filename = "AllFiles/";
                    filename += Guid.NewGuid().ToString() + taskUserViewModel.Attachments.FileName;
                    string folder = Path.Combine(_env.WebRootPath, filename);
                    await taskUserViewModel.Attachments.CopyToAsync(new FileStream(folder, FileMode.Create));
                    taskUserViewModel.attach.Attachments = taskUserViewModel.Attachments.FileName;
                    taskUserViewModel.attach.FilePath = filename;


                    var fileExt = Path.GetExtension(taskUserViewModel.Attachments.FileName);

                    if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".png" || fileExt == ".heic" || fileExt == ".bmp")
                    {
                        taskUserViewModel.attach.AttachmentType = AttachmentType.Photo;
                    }

                    if (fileExt == ".mp4" || fileExt == ".mov")
                    {
                        taskUserViewModel.attach.AttachmentType = AttachmentType.Video;
                    }
                    else if (fileExt == ".doc" || fileExt == ".txt" || fileExt == ".docx" || fileExt == ".xlsx" || fileExt == ".pdf" || fileExt == ".rtf")
                    {
                        taskUserViewModel.attach.AttachmentType = AttachmentType.Document;
                    }
                    else
                    {
                        taskUserViewModel.attach.AttachmentType = AttachmentType.Other;
                    }
                    var Att = taskUserViewModel.attach.AttachmentType;
                    var data = new Attachment() { Attachments = taskUserViewModel.Attachments.FileName, AttachmentType = Att, FilePath = filename };
                    _db.Attachment.Add(data);
                    await _db.SaveChangesAsync();
                    taskUserViewModel.task.AttachmentID = data.AttachmentID;

                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error while Uploading File.";
            }

            _db.Task.Add(taskUserViewModel.task);

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = taskUserViewModel.task.TaskTitle + " named Task Added Successfully";

            return RedirectToAction("AllProject", "Project");
        }


        public async Task<IActionResult> TaskDetail(int? id)
        {
            TaskUserViewModel obj1 = new TaskUserViewModel();

            if (id == null)
            {
                return NotFound();
            }
            var edit = await _db.Task.FindAsync(id);
            var at = await _db.Attachment.FindAsync(edit.AttachmentID);
            var AId = _db.User.FirstOrDefault(s => s.UserID == edit.AssigneeID);
            obj1.UserName = AId.UserName;
            HttpContext.Session.SetInt32("taskid", edit.TaskID);



            if (edit == null)
            {
                return NotFound();
            }

            obj1.task = edit;
            if (at != null)
            {
                obj1.attach = at;
            }
            return View(obj1);

        }

        [HttpPost]
        public async Task<IActionResult> TaskDetail(IFormCollection form, int id)
        {
            TaskUserViewModel obj2 = new TaskUserViewModel();

            var ToUpdate = await _db.Task.FindAsync(id);

            await _db.SaveChangesAsync();

            if (ToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Database.Task>(ToUpdate, "task", s => s.TaskTitle, s => s.UserId, s => s.Description, s => s.TaskType, s => s.Priority, s => s.Status, s => s.StartDate, s => s.EndDate))
            {
                await _db.SaveChangesAsync();

                return RedirectToAction("Board", "Project", new { id = ToUpdate.ProjectID });
            }

            return View();


        }


        public IActionResult DeleteTask(int? id)
        {
            var obj = _db.Task.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Remove(obj);
            var pid = 0;
            foreach (var p in _db.Task)
            {
                if (id == p.TaskID)
                {
                    pid = p.ProjectID;
                }
            }
            _db.SaveChanges();

            return RedirectToAction("Board", "Project", new { id = @pid });
        }
    }
}
