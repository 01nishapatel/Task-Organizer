using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace TaskOrganizer.Controllers
{
    public class AccountController : Controller
    {
        private TaskOrganizerDBContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(TaskOrganizerDBContext context, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Register()
        {

            return View();

        }

        [HttpPost]
        public IActionResult Register(User user)
        {

            int i = user.IsAdmin ? 1 : 0;
            user.IsAdmin = Convert.ToBoolean(i);


            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToAction("AllProject", "Project");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {



            using (_context)
            {
                var userdetail = _context.User.FirstOrDefault(x => x.EmailID.ToLower() == user.EmailID.ToLower());


                if (userdetail == null)
                {
                    ViewBag.ErrorMessage = "User doesn't exist in Database.";
                    return View();
                }
                else
                {

                    if (user.Password.Equals(userdetail.Password))
                    {

                        HttpContext.Session.SetString("UserEmail", userdetail.EmailID ?? "");
                        HttpContext.Session.SetString("UserName", userdetail.UserName ?? "");
                        HttpContext.Session.SetInt32("UserId", userdetail.UserID);
                        HttpContext.Session.SetString("isAd", userdetail.IsAdmin.ToString());
                        HttpContext.Session.SetString("FullName", userdetail.FullName ?? "");
                        bool isAd = userdetail.IsAdmin;

                        var name = HttpContext.Session.GetString("FullName");
                        var sub1 = name.Split(" ");
                        var a = sub1[0].Substring(0, 1);
                        var b = sub1[1].Substring(0, 1);
                        var c = a + b;
                        HttpContext.Session.SetString("labelname", c);

                        return RedirectToAction("AllProject", "Project");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Please Enter correct Password.";
                        return View();
                    }
                }


            }
        }



        public async Task<IActionResult> AllUser(string Search)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["headertitle"] = "All User";

            var allUser = from m in _context.User select m;

            if (!String.IsNullOrEmpty(Search))
            {
                allUser = allUser.Where(s => s.FullName!.ToLower().Contains(Search.ToLower()) || s.UserName!.ToLower().Contains(Search.ToLower()) || s.Address!.ToLower().Contains(Search.ToLower()) || s.EmailID!.ToLower().Contains(Search.ToLower()));

            }


            return View(await allUser.ToListAsync());

        }

        [Route("Account/AllMember/{id:int}")]
        public async Task<IActionResult> AllMember(int id, string Search)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["headertitle"] = "All Member";

            var allMember = from m in _context.User select m;

            var a = _context.ProjectAssign.Where(p => p.ProjectID == id);
            var d = _context.ProjectAssign.FirstOrDefault(p => p.ProjectID == id);

            var j = _context.Project.FirstOrDefault(l => l.ProjectID == id);


            if (!String.IsNullOrEmpty(Search))
            {
                allMember = allMember.Where(s => s.FullName!.ToLower().Contains(Search.ToLower()) || s.UserName!.ToLower().Contains(Search.ToLower()) || s.Address!.ToLower().Contains(Search.ToLower()) || s.EmailID!.ToLower().Contains(Search.ToLower()));

                return View(Tuple.Create<IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>, IEnumerable<Database.Task>>(await allMember.ToListAsync(), a, _context.Task.ToList()));
            }
            else
            {
                if (d != null)
                {
                    HttpContext.Session.SetInt32("projjid", d.ProjectID);
                    return View(Tuple.Create<IEnumerable<Database.User>, IEnumerable<Database.ProjectAssign>, IEnumerable<Database.Task>>(_context.User.ToList(), a, _context.Task.ToList()));
                }
                else
                {
                    HttpContext.Session.SetInt32("projjid", j.ProjectID);
                    return RedirectToAction("AddMember", "Account", new { id = j.ProjectID });
                }
            }

        }


        [Route("Account/UserProfile/{id:int}")]
        public IActionResult UserProfile(int? id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["headertitle"] = "User Profile";
            var detail = _context.User.Where(x => x.UserID == id).FirstOrDefault();

            HttpContext.Session.SetString("uname", detail.UserName);
            HttpContext.Session.SetString("uemail", detail.EmailID);
            HttpContext.Session.SetString("labeluname", detail.FullName);
            var nm = HttpContext.Session.GetString("labeluname");
            var a = nm.Split(" ");
            var b = a[0].Substring(0, 1);
            var c = a[1].Substring(0, 1);
            var d = b + c;
            HttpContext.Session.SetString("lbl", d);
            if (id == 0)
            {
                return View(new User());

            }
            else
            {
                return View(_context.User.Find(id));


            }
        }
        [HttpPost]
        public IActionResult UserProfile(User user)
        {

            _context.User.Update(user);
            _context.SaveChanges();
            return RedirectToAction("AllProject", "Project");
        }

        [Route("Account/AddMember/{id:int}")]
        public IActionResult AddMember(int? id)
        {
            TaskOrganizer.ViewModel.combinedModel obj = new TaskOrganizer.ViewModel.combinedModel();
            obj.ProjectAssignModel = new ViewModel.ProjectAssignModel();
            obj.ProjectAssignModel.emaillist = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.User, "UserID", "EmailID");
            var a = _context.ProjectAssign.Where(v => v.ProjectID == id).FirstOrDefault();
            return View(obj);
        }


        [HttpPost]
        [Route("Account/AddMember/{id:int}")]
        public IActionResult AddMember(int id, ProjectAssign projectAssign)
        {

            var m = _context.Project.FirstOrDefault(n => n.ProjectID == id);
            int projectID = m.ProjectID;
            projectAssign.ProjectID = projectID;
            _context.ProjectAssign.Add(projectAssign);
            _context.SaveChanges();
            return RedirectToAction("AllProject", "Project", projectAssign);
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]

        public IActionResult ForgotPassword(User user, IFormCollection form)
        {
            using (_context)
            {
                var detail = _context.User.Where(x => x.EmailID.ToLower() == user.EmailID.ToLower()).FirstOrDefault();

                if (detail == null)
                {
                    ViewBag.msg = "User not found";
                    return View();
                }

                HttpContext.Session.SetString("UserEmailId", user.EmailID ?? "");
                string EmailId = "tanvir.codexlancers@outlook.com";
                /* string EmailId = "tanvir.codexlancers@outlook.com";*/
                string Password = "Welcome@007";
                string Port = "587";
                /* string Host = "smtp-mail.outlook.com";*/
                string Host = "smtp.office365.com";

                string EnableSsl = "true";
                string UseDefaultCredentials = "false";
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(EmailId);
                msg.To.Add(user.EmailID.ToLower());
                msg.Subject = "Reset Passsword";

                Random rnd = new Random();
                int otp = rnd.Next(1000, 9999);



                HttpContext.Session.SetInt32("OTP", otp);

                string msg1 = "Reset Password Code : " + otp;

                msg.IsBodyHtml = false;
                msg.Body = msg1;


                using (SmtpClient client = new SmtpClient())
                {
                    client.UseDefaultCredentials = Convert.ToBoolean(UseDefaultCredentials);
                    client.Credentials = new System.Net.NetworkCredential(EmailId, Password);
                    client.Port = Convert.ToInt32(Port);
                    client.Host = Host;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = Convert.ToBoolean(EnableSsl);

                    try
                    {
                        client.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        var str = ex.ToString();
                    }
                }
                return RedirectToAction("Reset", "Account");
            }

        }
        public IActionResult Reset()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Reset(IFormCollection form, User user)
        {

            var s = form["resetcode"].ToString();

            var a = HttpContext.Session.GetInt32("OTP").ToString();
            var e = HttpContext.Session.GetString("UserEmailId");

            using (_context)
            {

                var detail = _context.User.Where(x => x.EmailID.ToLower() == e.ToLower()).FirstOrDefault();

                if (detail != null && detail?.EmailID != null)
                {
                    if (s == a)
                    {
                        detail.Password = user.Password;
                        _context.User.Update(detail);
                        _context.SaveChanges();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.message = "Wrong otp";
                        return View();
                    }
                }
                return RedirectToAction("Login", "Account");

            }

        }
        public IActionResult logout(int n)
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }


    }
}
