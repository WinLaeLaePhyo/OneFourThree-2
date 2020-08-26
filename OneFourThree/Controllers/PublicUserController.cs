using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OneFourThree.App_Code;
namespace OneFourThree.Controllers
{
    public class PublicUserController : Controller
    {
        // GET: PublicUser
        DBBack db = new DBBack();
        ParameterBack p = new ParameterBack();
        public ActionResult Index()
        {
            return View();
        }

        #region Login Logout
        public ActionResult Logout()
        {
            Session.RemoveAll();
            if (Request.Cookies["userInfo"] != null)
            {
                HttpCookie myCookie = new HttpCookie("userInfo");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("Index");
        }
        public ActionResult LoginForm()
        {
            return View();
        }
        public ActionResult ProcessLoginForm()
        {
            int CurrentUserID, AccessLevel;

            Authentication a = new Authentication();
            string Phone = Request.Form["Phone"];
            string Pin = Request.Form["Pin"];

            HttpCookie userInfo = new HttpCookie("userInfo");
            userInfo["Phone"] = Phone;
            userInfo["Pin"] = Pin;
            userInfo.Expires = DateTime.Now.AddDays(5);
            Response.Cookies.Add(userInfo);

            if (a.checkUser(Phone, Pin))
            {
                CurrentUserID = a.getUserID();
                AccessLevel = a.getAccessLevel();

                //assign to session
                Session["CurrentUserID"] = CurrentUserID;
                Session["AccessLevel"] = AccessLevel;

                if (AccessLevel == 1)
                {
                    return RedirectToAction("Dashboard", "Company");
                }
                else if (AccessLevel == 2)
                {
                    return RedirectToAction("Dashboard", "Lover");
                }
            }
            else
            {
                TempData["Message"] = "Invalid Credentials";
                return RedirectToAction("LoginForm");
            }
            return View();
        }
#endregion

        #region LoverRegisterOne
        public ActionResult LoverRegisterOne() {
            return View();
        }
        public ActionResult ProcessLoverRegisterOne()
        {
            string Email = "";
            try
            {
                string Name = Request.Form["Name"];
                string Phone = Request.Form["Phone"];
                if (Request.Form["Email"] != "")
                {
                    Email = Request.Form["Email"];
                }

                p.setParameter(1,Phone);
                if (db.CheckByQueryParameter("select * from Lover where convert(nvarchar(MAX),decryptByPassPhrase('root@',Phone)) = @parameter1", 1, p))
                {
                    TempData["Message"] = "ယခု ဖုန်းနံပါတ်ဖြင့် အကောင့်ဖွင့်ပြီးပါပြီ။";
                    return RedirectToAction("LoverRegisterOne");
                }
                string Address = Request.Form["Address"];
                TempData["Name"] = Name;
                TempData["Phone"] = Phone;
                TempData["Email"] = Email;
                TempData["Address"] = Address;
                return RedirectToAction("LoverRegisterTwo");
            }
            catch {
                return RedirectToAction("UnderMaintainence","PublicUser");
            }
        }
        #endregion

        #region LoverRegisterTwo
        public ActionResult LoverRegisterTwo() {
            return View();
        }
        public ActionResult ProcessLoverRegisterTwo() 
        {
            string Email = "";
            try
            {
                string Name = Request.QueryString["Name"];
                string Phone = Request.QueryString["Phone"];
                if (Request.QueryString["Email"] != "")
                {
                    Email = Request.QueryString["Email"];
                }
                string Address = Request.QueryString["Address"];
                string Height = Request.Form["Height"];
                string LoverType = Request.Form["LoverType"];
                string TownID = Request.Form["TownID"];
                TempData["Name"] = Name;
                TempData["Phone"] = Phone;
                TempData["Email"] = Email;
                TempData["Address"] = Address;
                TempData["Height"] = Height;
                TempData["LoverType"] = LoverType;
                TempData["TownID"] = TownID;
                return RedirectToAction("LoverRegisterThree");
            }
            catch
            {
                return RedirectToAction("UnderMaintainence", "PublicUser");
            }
        }
        #endregion

        #region LoverRegisterThree
        public ActionResult LoverRegisterThree() {
            return View();
        }
        public ActionResult ProcessLoverRegisterThree() {

            string Email = "";
            try
            {
                string Name = Request.QueryString["Name"];
                string Phone = Request.QueryString["Phone"];
                if (Request.QueryString["Email"] != "")
                {
                    Email = Request.QueryString["Email"];
                }
                string Address = Request.QueryString["Address"];
                string Height = Request.QueryString["Height"];
                string Type = Request.QueryString["LoverType"];
                string TownID = Request.QueryString["TownID"];
                string Password = Request.Form["Password"];
                string Age = Request.Form["Age"];
                string Gender = Request.Form["Gender"];
                string Location = Request.Form["Location"];
                string Lat = Request.Form["Lat"];
                string Lng = Request.Form["Lng"];

                p.setParameter(1, Name);
                p.setParameter(2, Phone);
                p.setParameter(3, Email);
                p.setParameter(4, Address);
                p.setParameter(5, Height);
                p.setParameter(6, Age);
                p.setParameter(7, Type);
                p.setParameter(8, TownID);
                p.setParameter(9, Location);
                p.setParameter(10, Lat);
                p.setParameter(11, Lng);
                p.setParameter(12, Gender);
                db.ChangeByQueryParameter("insert into Lover values(@Parameter1,encryptByPassPhrase('root@',@Parameter2),encryptByPassPhrase('root@',@Parameter3),@Parameter4,@Parameter5,@Parameter6,@Parameter7,@Parameter8,@Parameter9,@Parameter10,@Parameter11,'"+DateTime.Now+"',@Parameter12)", 12,p);

                p.setParameter(1, Phone);
                p.setParameter(2, Password);

                int AllID = db.getIntByQueryParameter("select * from Lover where convert(nvarchar(max),decryptByPassPhrase('root@',Phone))=@parameter1", "ID", 1, p);
                db.ChangeByQueryParameter("insert into Login values(encryptByPassPhrase('root@',@Parameter1),encryptByPassPhrase('root@',@Parameter2),2,"+AllID+",'true','"+DateTime.Now+"')", 2,p);

                TempData["Message"] = "အကောင့်ဖွင့်ခြင်းအောင်မြင်ပါသည်။";
                return RedirectToAction("LoginForm");
            }
            catch
            {
                return RedirectToAction("UnderMaintainence", "PublicUser");
            }
        }
        #endregion
    }
}