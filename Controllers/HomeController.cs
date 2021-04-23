using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UniversityCommittee.Models;

namespace UniversityCommittee.Controllers
{
    public class HomeController : Controller
    {
        UniversityContext db = new UniversityContext();

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Courses()
        {
            return View();
        }
        public ActionResult Admission()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminPanelUsers()
        {
            ViewBag.Users = db.Users.Include(p => p.Role);
            

            return View("AdminPanel_Users");
        }
        [HttpPost]
        public ActionResult AdminPanelMessages()
        {
            ViewBag.Messages = db.Contacts;

            return View("AdminPanel_Messages");
        }
        [HttpPost]
        public ActionResult AdminPanelGroups()
        {
            ViewBag.Groups = db.Groups.Include(t => t.Users);

            return View("AdminPanel_Groups");
        }

        [HttpPost]
        public ActionResult AdminPanelGroupsDetails(int id)
        {
            ViewBag.Group = db.Groups.Where(p => p.Id == id).SingleOrDefault();

            ViewBag.Student = db.Users.Where(p => p.GroupId == id && p.RoleId == 1).FirstOrDefault();

            ViewBag.Subjects = db.Subjects;

            ViewBag.GroupsStudents = db.Users.Where(g=>g.GroupId==id);

            return View("AdminPanel_GroupsDetails");
        }

        [HttpPost]
        public ActionResult AddGroup(string Name)
        {
            var group = db.Groups.Where(g => g.GroupName == Name).SingleOrDefault();
            if (group == null)
            {
                group = new Group();
                group.GroupName = Name;
                db.Groups.Add(group);
                db.SaveChanges();
            }
            ViewBag.Groups = db.Groups.Include(t => t.Users);

            return View("AdminPanel_Groups");
        }

        [HttpPost]
        public ActionResult EditGroup(int group_id, string GroupName)
        {

            var Group = db.Groups.Where(p => p.Id == group_id).SingleOrDefault();

            if (Group != null)
            {
                Group.GroupName = GroupName;
                db.Entry(Group).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message1 = 1;
            }
            else
            {
                ViewBag.Message1 = 0;
            }

            ViewBag.Group = db.Groups.Where(p => p.Id == group_id).SingleOrDefault();

            ViewBag.Student = db.Users.Where(p => p.GroupId == group_id && p.RoleId == 1).FirstOrDefault();

            ViewBag.Subjects = db.Subjects;
            ViewBag.GroupsStudents = db.Users.Where(g => g.GroupId == group_id);

            return View("AdminPanel_GroupsDetails");
        }
        [HttpPost]
        public ActionResult DeleteMessage(int m_id)
        {
            Contact m = db.Contacts.Find(m_id);
            if (m != null)
            {
                db.Contacts.Remove(m);
                db.SaveChanges();
            }
            ViewBag.Messages = db.Contacts;
            return View("AdminPanel_Messages");
        }

        [HttpPost]
        public ActionResult DeleteGroup(int group_id)
        {
            Group g = db.Groups.Find(group_id);
            if (g != null)
            {
                db.Groups.Remove(g);
                db.SaveChanges();
            }
            ViewBag.Groups = db.Groups.Include(t => t.Users);

            return View("AdminPanel_Groups");
        }

        [HttpPost]
        public ActionResult AssignSubjects(int subject_id, int group_id)
        {
            var Students = db.Users.Where(p => p.GroupId == group_id && p.RoleId == 1);

            Subject subj = db.Subjects.Where(sub => sub.Id == subject_id).SingleOrDefault();

            foreach (User stud in Students)
            {
                Assessment assessment = new Assessment();
                assessment.UserId = stud.Id;
                assessment.SubjectId = subject_id;
                db.Assessments.Add(assessment);
            }

            ((List<User>)subj.Users).AddRange(Students);


            db.Entry(subj).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Group = db.Groups.Where(p => p.Id == group_id).SingleOrDefault();

            ViewBag.Student = db.Users.Where(p => p.GroupId == group_id && p.RoleId == 1).FirstOrDefault();

            ViewBag.Subjects = db.Subjects;
            ViewBag.GroupsStudents = db.Users.Where(g => g.GroupId == group_id);

            return View("AdminPanel_GroupsDetails");
        }
        [HttpPost]
        public ActionResult RejectSubjects(int subject_id, int group_id)
        {
            var Students = db.Users.Where(p => p.GroupId == group_id && p.RoleId == 1);

            Subject subj = db.Subjects.Where(sub => sub.Id == subject_id).SingleOrDefault();



            foreach (User stud in Students)
            {
                Assessment assess = db.Assessments.Where(a => a.UserId == stud.Id && a.SubjectId == subject_id).SingleOrDefault();
                db.Assessments.Remove(assess);
            }


            /* ((List<User>)subj.Users).AddRange(Students);

             foreach (User st in Students)
             {
                 subj.Users.Remove(st);
             }*/

            subj.Users = ((List<User>)subj.Users).Except(Students).ToList();

            db.Entry(subj).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Group = db.Groups.Where(p => p.Id == group_id).SingleOrDefault();

            ViewBag.Student = db.Users.Where(p => p.GroupId == group_id && p.RoleId == 1).FirstOrDefault();

            ViewBag.Subjects = db.Subjects;
            ViewBag.GroupsStudents = db.Users.Where(g => g.GroupId == group_id);

            return View("AdminPanel_GroupsDetails");
        }


        [HttpPost]
        public ActionResult AdminPanelSubjects()
        {
            ViewBag.Subjects = db.Subjects;

            return View("AdminPanel_Subjects");
        }

        [HttpPost]
        public ActionResult AdminPanelSubjectsDetails(int id)
        {
            ViewBag.Subject = db.Subjects.Where(p => p.Id == id).SingleOrDefault();
            ViewBag.Teachers = db.Users.Where(t => t.RoleId == 2);

            return View("AdminPanel_SubjectsDetails");
        }
        [HttpPost]
        public ActionResult EditSubject(int subject_id, string Name, int CreditAmount)
        {

            var Subject = db.Subjects.Where(p => p.Id == subject_id).SingleOrDefault();

            if (Subject != null)
            {
                Subject.Name = Name;
                Subject.CreditAmount = CreditAmount;
                db.Entry(Subject).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message1 = 1;
            }
            else
            {
                ViewBag.Message1 = 0;
            }

            ViewBag.Subject = db.Subjects.Where(p => p.Id == subject_id).SingleOrDefault();
            ViewBag.Teachers = db.Users.Where(t => t.RoleId == 2);

            return View("AdminPanel_SubjectsDetails");
        }
        [HttpPost]
        public ActionResult AddSubject(string Name, int CreditAmount)
        {
            Subject Subj = db.Subjects.Where(p => p.Name == Name).SingleOrDefault();
            if (Subj == null)
            {
                Subj = new Subject();
                Subj.Name = Name;
                Subj.CreditAmount = CreditAmount;
                db.Subjects.Add(Subj);
                db.SaveChanges();
            }
            ViewBag.Subjects = db.Subjects;

            return View("AdminPanel_Subjects");
        }
        [HttpPost]
        public ActionResult DeleteSubject(int subject_id)
        {
            Subject s = db.Subjects.Find(subject_id);
            if (s != null)
            {
                db.Subjects.Remove(s);
                db.SaveChanges();
            }
            ViewBag.Subjects = db.Subjects;

            return View("AdminPanel_Subjects");
        }

        [HttpPost]
        public ActionResult AssignTeachers(int subject_id, int teacher_id)
        {
            User teacher = db.Users.Where(at => at.Id == teacher_id).SingleOrDefault();
            Subject subj = db.Subjects.Where(sub => sub.Id == subject_id).SingleOrDefault();
            subj.Users.Add(teacher);


            db.Entry(subj).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Subject = db.Subjects.Where(p => p.Id == subject_id).SingleOrDefault();
            ViewBag.Teachers = db.Users.Where(t => t.RoleId == 2);

            return View("AdminPanel_SubjectsDetails");
        }
        [HttpPost]
        public ActionResult RejectTeachers(int subject_id, int teacher_id)
        {
            User teacher = db.Users.Where(at => at.Id == teacher_id).SingleOrDefault();
            Subject subj = db.Subjects.Where(sub => sub.Id == subject_id).SingleOrDefault();
            subj.Users.Remove(teacher);

            db.Entry(subj).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Subject = db.Subjects.Where(p => p.Id == subject_id).SingleOrDefault();
            ViewBag.Teachers = db.Users.Where(t => t.RoleId == 2);

            return View("AdminPanel_SubjectsDetails");
        }


        [HttpPost]
        public ActionResult CreateUser(User user, String Password, String Repassword)
        {
            //User userValid = db.Users.Find(user.Email);

            var users = db.Users.Where(p => p.Email == user.Email).SingleOrDefault();
            if (users == null)
            {
                if (Password.Equals(Repassword) && ModelState.IsValid)
                {
                    user.PictureURL = "https://99181-282044-raikfcquaxqncofqfm.stackpathdns.com/wp-content/uploads/2016/05/icon-user-default.png";
                    user.RoleId = 1;
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Message = 1;
                    return View("Register");
                }

            }
            ViewBag.Message = 0;
            return View("Register");
        }

        [HttpPost]
        public ActionResult CreateContact(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
            return RedirectPermanent("/Home/Contact");
        }

        public async Task<ActionResult> UserList()
        {
            IEnumerable<User> users = await db.Users.ToListAsync();
            ViewBag.Users = users;
            return View("Index");
        }

        public ActionResult Login()
        {
            try
            {
                if (Request.Cookies["email"].Value != null)
                {
                    String Email = HttpContext.Request.Cookies["email"].Value;
                    var user = (User)db.Users.Where(x => x.Email == Email).SingleOrDefault();
                    ViewBag.User = user;
                    if (user.RoleId == 3)
                    {
                        ViewBag.Users = db.Users.Include(p => p.Role); ;
                        return View("AdminPanel_Users");
                    }
                    return View("ProfilePage");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ToLogin(String req)
        {
            if (req != null)
            {
                Response.Cookies["email"].Expires = DateTime.Now.AddDays(-1d);
            }
            return View("Login");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProfilePage(String Email, String Password)
        {
            try
            {
                if (HttpContext.Request.Cookies["email"].Value != null)
                {
                    Email = HttpContext.Request.Cookies["email"].Value;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            var user = (User)db.Users.Where(x => x.Email == Email).SingleOrDefault();
            if (user != null)
            {
                try
                {
                    if (user.Password == Password || HttpContext.Request.Cookies["email"].Value != null)
                    {
                        if (user.Password == Password)
                        {
                            HttpContext.Response.Cookies["email"].Value = user.Email;
                            Response.Cookies["email"].Expires = DateTime.Now.AddDays(+1d);
                        }
                        ViewBag.User = user;
                        if (user.RoleId == 3)
                        {
                            ViewBag.Users = db.Users.Include(p => p.Role); ;
                            return View("AdminPanel_Users");
                        }
                        return View("ProfilePage");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return RedirectPermanent("/Home/Login");
        }

        [HttpPost]
        public ActionResult EditProfilePage(int UserID)
        {

            var user = (User)db.Users.Where(x => x.Id == UserID).SingleOrDefault();
            if (user != null)
            {
                ViewBag.User = user;
                return View("EditProfilePage");
            }
            return View("ProfilePage");
        }

        [HttpPost]
        public ActionResult EditProfileData1(int id, String email, String name, String surname, Boolean byadmin)
        {

            var user = (User)db.Users.Where(x => x.Id == id).SingleOrDefault();

            if (user != null)
            {
                user.Email = email;
                user.Name = name;
                user.Surname = surname;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message1 = 1;
                ViewBag.User = user;
                if (byadmin)
                {
                    ViewBag.User = db.Users.Where(u => u.Id == id).SingleOrDefault();
                    ViewBag.Groups = db.Groups;
                    ViewBag.Roles = db.Roles;
                    return View("AdminPanel_UsersDetails");
                }
                return View("EditProfilePage");
            }
            ViewBag.User = user;
            if (byadmin)
            {
                ViewBag.User = db.Users.Where(u => u.Id == id).SingleOrDefault();
                ViewBag.Groups = db.Groups;
                ViewBag.Roles = db.Roles;
                return View("AdminPanel_UsersDetails");
            }
            return View("ProfilePage");
        }

        [HttpPost]
        public ActionResult EditProfileData2(int id, String url_img)
        {

            var user = (User)db.Users.Where(x => x.Id == id).SingleOrDefault();

            if (user != null)
            {
                user.PictureURL = url_img;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message2 = 1;
                ViewBag.User = user;
                return View("EditProfilePage");
            }
            ViewBag.Message2 = 0;
            ViewBag.User = user;
            return View("ProfilePage");
        }

        [HttpPost]
        public ActionResult EditProfileData3(int id, String password,
            String new_password, String re_new_password)
        {

            var user = (User)db.Users.Where(x => x.Id == id).SingleOrDefault();

            if (user != null)
            {
                if (user.Password == password)
                {
                    if (new_password == re_new_password)
                    {
                        user.Password = new_password;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message3 = 1;
                        ViewBag.User = user;
                        return View("EditProfilePage");
                    }
                }
            }
            ViewBag.Message3 = 0;
            ViewBag.User = user;
            return View("EditProfilePage");
        }
        [HttpPost]
        public ActionResult EditProfilePasswordByAdmin(int user_id, String new_password)
        {

            var user = (User)db.Users.Where(x => x.Id == user_id).SingleOrDefault();

            if (user != null)
            {
                user.Password = new_password;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message3 = 1;
                ViewBag.User = db.Users.Where(u => u.Id == user_id).SingleOrDefault();
                ViewBag.Groups = db.Groups;
                ViewBag.Roles = db.Roles;
                return View("AdminPanel_UsersDetails");
            }
            ViewBag.Message3 = 0;
            ViewBag.User = db.Users.Where(u => u.Id == user_id).SingleOrDefault();
            ViewBag.Groups = db.Groups;
            ViewBag.Roles = db.Roles;
            return View("AdminPanel_UsersDetails");
        }
        [HttpPost]
        public ActionResult AdminPanelUsersDetails(int id)
        {
            ViewBag.User = db.Users.Where(u => u.Id == id).SingleOrDefault();
            ViewBag.Groups = db.Groups;
            ViewBag.Roles = db.Roles;

            return View("AdminPanel_UsersDetails");
        }
        [HttpPost]
        public ActionResult EditUserGroup(int user_id, int groupId)
        {

            var User = db.Users.Where(p => p.Id == user_id).SingleOrDefault();
            

            if (User != null && groupId != -1)
            {
                var UserFromGroup = db.Users.Where(p => p.GroupId == groupId && p.RoleId == 1).FirstOrDefault();

                User.GroupId = groupId;

                User.Subjects = new List<Subject>();

                if (UserFromGroup != null)
                {
                    foreach (Subject subj in User.Subjects)
                    {
                        Assessment assess = db.Assessments.Where(a => a.UserId == user_id && a.SubjectId == subj.Id).SingleOrDefault();
                        db.Assessments.Remove(assess);
                        db.SaveChanges();
                    }

                    User.Subjects = UserFromGroup.Subjects;

                    foreach (Subject subj in UserFromGroup.Subjects)
                    {
                        Assessment assessment = new Assessment();
                        assessment.UserId = user_id;
                        assessment.SubjectId = subj.Id;
                        db.Assessments.Add(assessment);
                    }
                }
                

                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message4 = 1;
            }
            if (groupId == -1)
            {
                User.GroupId = null;
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message4 = 1;
            }
            ViewBag.User = db.Users.Where(u => u.Id == user_id).SingleOrDefault();
            ViewBag.Groups = db.Groups;
            ViewBag.Roles = db.Roles;

            return View("AdminPanel_UsersDetails");
        }
        [HttpPost]
        public ActionResult EditUserRole(int user_id, int role_id)
        {

            var User = db.Users.Where(p => p.Id == user_id).SingleOrDefault();

            if (User != null)
            {

                User.GroupId = null;
                User.Subjects = new List<Subject>();//Удаление прошлых данных при переходе между ролями

                User.RoleId = role_id;
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message2 = 1;
            }
            ViewBag.User = db.Users.Where(u => u.Id == user_id).SingleOrDefault();
            ViewBag.Groups = db.Groups;
            ViewBag.Roles = db.Roles;
            return View("AdminPanel_UsersDetails");
        }

        [HttpPost]
        public ActionResult TeacherPanelSubjects(int teacher_id)
        {
            var Teacher = db.Users.Where(t => t.Id == teacher_id && t.RoleId==2).SingleOrDefault();
            ViewBag.Subjects = Teacher.Subjects;
            
            ViewBag.User = Teacher;

            return View("TeacherPanel_Subjects");
        }
        [HttpPost]
        public ActionResult TeacherSubjectDetails(int teacher_id,int subject_id)
        {
            var Teacher = db.Users.Where(t => t.Id == teacher_id).SingleOrDefault();
            
            var Subject = db.Subjects.Where(sub => sub.Id == subject_id).SingleOrDefault();

            var Students = db.Users.Where(s => s.RoleId == 1).Include(p => p.Group).Include(p=>p.Subjects);

            List<Group> Groups = new List<Group>();
            foreach (User st in Students)
            {
                if (st.Subjects.Contains(Subject))
                {
                    if (!Groups.Contains(st.Group))
                    {
                        Groups.Add(st.Group);
                    }
                }
            }
            ViewBag.Groups = Groups;
            ViewBag.User = Teacher;
            ViewBag.Subjects = db.Subjects.Where(t => t.Users.Contains(Teacher));
            ViewBag.Subject = Subject;

            return View("TeacherPanel_Groups");
        }

        [HttpPost]
        public ActionResult TeacherGroupDetails(int teacher_id, int subject_id,int group_id)
        {
            var Teacher = db.Users.Where(t => t.Id == teacher_id).SingleOrDefault();

            var Group = db.Groups.Where(t => t.Id == group_id).SingleOrDefault();
            var Subject = db.Subjects.Where(t => t.Id == subject_id).SingleOrDefault();

            var Students = db.Users.Where(s => s.RoleId == 1 && s.GroupId==group_id).Include(p => p.Group).Include(p => p.Subjects).ToList();

            List<Assessment> assessments = new List<Assessment>();

            var AssessmentsSource = db.Assessments.ToList();


            foreach (var st in Students)
            {
                var assess = AssessmentsSource.Where(a => a.UserId == st.Id && a.SubjectId == subject_id).SingleOrDefault();
                assessments.Add(assess);
            }
            ViewBag.Assessments = assessments;
            ViewBag.User = Teacher;
            ViewBag.Group = Group;
            ViewBag.Students = Students;
            ViewBag.Subject = Subject;

            return View("TeacherPanel_Students");
        }

        [HttpPost]
        public ActionResult TeacherEditScore(int teacher_id, int subject_id, int group_id,int stud_id,Assessment NewScore)
        {
            var Teacher = db.Users.Where(t => t.Id == teacher_id).SingleOrDefault();

            var Group = db.Groups.Where(t => t.Id == group_id).SingleOrDefault();

            var Subject = db.Subjects.Where(t => t.Id == subject_id).SingleOrDefault();

            var Students = db.Users.Where(s => s.RoleId == 1 && s.GroupId == group_id).Include(p => p.Group).Include(p => p.Subjects).ToList();

            var Stud= db.Users.Where(t => t.Id == stud_id).SingleOrDefault();

            var StudScore = db.Assessments.Where(d => d.UserId == stud_id && d.SubjectId == subject_id).SingleOrDefault();

            StudScore.Midterm = NewScore.Midterm;
            StudScore.Endterm = NewScore.Endterm;
            StudScore.FinalExam = NewScore.FinalExam;
            StudScore.FinalScore = 0.6 * (0.5 * (NewScore.Midterm + NewScore.Endterm)) + 0.4 * NewScore.FinalExam;

            db.Entry(StudScore).State = EntityState.Modified;
            db.SaveChanges();


            List<Assessment> assessments = new List<Assessment>();

            var AssessmentsSource = db.Assessments.ToList();

            foreach (var st in Students)
            {
                var assess = AssessmentsSource.Where(a => a.UserId == st.Id && a.SubjectId == subject_id).SingleOrDefault();
                assessments.Add(assess);
            }
            ViewBag.Assessments = assessments;
            ViewBag.User = Teacher;
            ViewBag.Group = Group;
            ViewBag.Subject = Subject;
            ViewBag.Students = Students;

            return View("TeacherPanel_Students");
        }

        [HttpPost]
        public ActionResult Grades(int student_id)
        {
            var Student = db.Users.Where(s => s.Id == student_id).SingleOrDefault();

            var Subjects = Student.Subjects;

            List<Assessment> assessments = new List<Assessment>();

            var AssessmentsSource = db.Assessments.ToList();


            foreach (var subj in Subjects)
            {
                var assess = AssessmentsSource.Where(a => a.UserId == student_id && a.SubjectId == subj.Id).SingleOrDefault();
                assessments.Add(assess);
            }



            
            ViewBag.Assessments = assessments;
            ViewBag.User = Student;
            ViewBag.Subjects = Subjects;

            return View("StudentPanel_Grades");
        }


        [HttpPost]
        public ActionResult ToSchedule(int student_id)
        {
            var Student = db.Users.Where(s => s.Id == student_id).SingleOrDefault();

           
            ViewBag.User = Student;
            return View("ScheduleView");
        }

    }
}