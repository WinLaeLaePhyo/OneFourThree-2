using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OneFourThree.App_Code;

namespace OneFourThree.Controllers
{
    public class LoverController : Controller
    {
        // GET: Lover
        #region Declaration
        DBBack db = new DBBack();
        ParameterBack p = new ParameterBack();
        public ActionResult Dashboard()
        {
            return View();
        }
        public int GetCurrentUserID()
        {
            return Convert.ToInt32(Session["CurrentUserID"]);
        }
        #endregion

        #region SearchOnMap
        public ActionResult SearchOnMap()
        {
            if (TempData["Gender"] != null && TempData["Age"] != null)
            {
                ViewBag.Gender = TempData["Gender"].ToString();
                ViewBag.Age = TempData["Age"];
            }
            else
            {
                int Age = 21;
                //int Age = db.getIntByQuery("select * form Lover where ID=" + GetCurrentUserID(), "Age");
                ViewBag.Gender = "Male";
                ViewBag.Age = Age;
            }
            return View();
        }
        public ActionResult ProcessSearchOnMap()
        {
            string Gender = Request.Form["Gender"];
            int Age = Convert.ToInt32(Request.Form["Age"]);
            TempData["Gender"] = Gender;
            TempData["Age"] = Age;
            return RedirectToAction("SearchOnMap");
        }
        #endregion

        #region ShowLoverDetail
        public ActionResult ShowLoverDetail() {
            int LoverID = Convert.ToInt32(Request.QueryString["ID"]);
            ViewBag.LoverID = LoverID;
            return View();
        }
        #endregion

        #region Get Lover JSON GET
        public JsonResult GetLover()
        {
            var data = new List<ElementDataList>();
            try
            {
                ElementDataList mg = new ElementDataList();
                double sLatitude = Convert.ToDouble(Request.QueryString["Lat"]);
                double sLongitude = Convert.ToDouble(Request.QueryString["Lng"]);
                string Gender = Request.QueryString["Gender"];
                int Age = Convert.ToInt32(Request.QueryString["Age"]);
                data = mg.GetLover(sLatitude, sLongitude, Gender, Age);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch { return Json(data, JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region AboutMe UpdatePrifile
        public ActionResult AboutMe() {
            return View();
        }
        public ActionResult UpdateProfile() {
            return View();
        }
        public ActionResult ProcessUpdateProfile(HttpPostedFileBase postedFile) {

            string Name = db.getStringByQuery("select * from Lover where ID="+GetCurrentUserID(),"Name");
            string Phone = db.getStringByQuery("select *,convert(nvarchar(max),decryptByPassPhrase('root@',Phone)) as LPhone from Lover where ID=" + GetCurrentUserID(), "LPhone");
            string Address = db.getStringByQuery("select * from Lover where ID=" + GetCurrentUserID(), "Address");
            if (Request.Form["Name"] != "")
            {
                Name = Request.Form["Name"];
            }
            if (Request.Form["Phone"] != "")
            {
                Phone = Request.Form["Phone"];
            }
            if (Request.Form["Address"] != "")
            {
                Address = Request.Form["Address"];
            }
            if (postedFile != null)
            {
                string path = Server.MapPath("~/ProfilePicture/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                p.setParameter(1, postedFile.FileName);
                int CurrentUserID = GetCurrentUserID();
                db.ChangeByQueryParameter("insert into LoverProfilePicture values(" + CurrentUserID + ",@Parameter1)", 1, p);
            }

            p.setParameter(1, Name);
            p.setParameter(2, Phone);
            p.setParameter(3, Address);

            db.ChangeByQueryParameter("update Lover set Name=@Parameter1,Phone=encryptByPassPhrase('root@',@parameter2),Address=@parameter3 where ID="+GetCurrentUserID(),3,p);
            TempData["Messaage"] = "သင်၏ အချက်အလက်များ ပြောင်းလဲပြီးပါပြီ။";
            return RedirectToAction("UpdateProfile");
        }
        #endregion
    }
}
