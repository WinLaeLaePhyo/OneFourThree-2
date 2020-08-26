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
    [LoverFilter]
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

        #region GetNotification - JSON GET
        public JsonResult GetNotification()
        {
            var data = db.getCountByQuery("select * from Notification where Seen = 'False' and Receiver = " + GetCurrentUserID());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ChattingRoom SaveMessage
        public ActionResult ChattingRoom() {
            int Receiver = Convert.ToInt32(Request.QueryString["Requester"]);
            ViewBag.Receiver = Receiver;
            return View();
        }
        public ActionResult SaveMessage() {
            string Receiver = Request.Form["Receiver"];
            string Message = Request.Form["Message"];
            p.setParameter(1, Receiver);
            p.setParameter(2, Message);
            db.ChangeByQueryParameter("insert into ChatMessages values("+GetCurrentUserID()+",@Parameter1,@Parameter2,'True','"+DateTime.Now+"')",2,p);
            return RedirectToAction("ChattingRoom", new { Requester = Receiver });
        }
        #endregion

        #region ShowLoverDetail ConnectWithLover LoverNotification ShowLoverProfile AcceptRequester 
        public ActionResult ShowLoverDetail() {
            int LoverID = Convert.ToInt32(Request.QueryString["ID"]);
            ViewBag.LoverID = LoverID;
            return View();
        }
        public ActionResult ShowLoverProfile()
        {
            int Requester = Convert.ToInt32(Request.QueryString["Requester"]);
            ViewBag.Requester = Requester;
            return View();
        }
        public ActionResult AcceptRequester() { 
            int Requester = Convert.ToInt32(Request.QueryString["Requester"]);
            string RequesterName = db.getStringByQuery("select * from Lover where ID="+Requester,"Name");
            int Receiver = GetCurrentUserID();
            db.ChangeByQuery("insert into RequesterReceiverConnection values("+Requester+","+Receiver+",'"+DateTime.Now+"')");

            string Message = "သင်နှင့် ရင်းနှီးခွင့် လက်ခံလိုက်ပါသည်။";
            p.setParameter(1, Requester.ToString());
            p.setParameter(2, Message);
            db.ChangeByQueryParameter("insert into Notification values('" + GetCurrentUserID() + "',@parameter1,@parameter2,'" + DateTime.Now + "','False')", 2, p);
            TempData["Message"] = RequesterName+" နှင့်ချိတ်ဆက်ပြီးပါပြီ။";
            return RedirectToAction("FriendList"); ;
        }
        public ActionResult ConnectWithLover() {
            string LoverID = Request.QueryString["LoverID"];
            string Message = "သင်နှင့် ရင်းနှီးခွင့် တောင်းဆိုထားပါသည်။";
            p.setParameter(1, LoverID);
            p.setParameter(2, Message);
            db.ChangeByQueryParameter("insert into Notification values('" + GetCurrentUserID() + "',@parameter1,@parameter2,'"+DateTime.Now+"','False')", 2,p);
            return RedirectToAction("Dashboard");
        }
        public ActionResult LoverNotification() {
            db.ChangeByQuery("update Notification set Seen='True' where Receiver="+GetCurrentUserID());
            return View();
        }
        #endregion

        #region FriendList Remove Lover
        public ActionResult FriendList() {
            return View();
        }
        public ActionResult RemoveLover()
        {
            int Requester = Convert.ToInt32(Request.QueryString["Requester"]);
            int Receiver = Convert.ToInt32(Request.QueryString["Receiver"]);
            db.ChangeByQuery("delete from RequesterReceiverConnection where Requester="+Requester+" and Receiver="+Receiver);
            return RedirectToAction("FriendList");
        }
        #endregion

        #region ConnectedLover
        public ActionResult ConnectedLover() {
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
