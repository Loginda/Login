using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Login_Asyn.Models;

namespace Login_Asyn.Controllers
{
    public class HomeController : Controller
    {
        SessionEntities db = new SessionEntities();
        public async Task<JsonResult> Index(string user, string pass)
        {
            Login lg = new Login();
            lg.user = user;
            lg.pass = pass;
            string res = "";
            ACCOUNT entity;
            Tree x = new Tree();
            var delMin = x.DeleteMin();
            var check1 = lg.Check();
            await Task.WhenAll(delMin, check1);
            bool check = await check1;
            if (check == true)
            {
                entity = db.ACCOUNTs.Where(y => y.UserName == lg.user && y.Password == lg.pass).ToList()[0];
                if (x.FindNode(entity) == null)
                {
                    DateTime dt = DateTime.Now;
                    dt = dt.AddMinutes(Session.Timeout);
                    x.Add(dt, entity);
                    entity.TimeOut = dt;
                    entity.Session = Session.SessionID;
                    Session.Add(entity.UserName, entity);
                    db.SaveChanges();
                    res = "Dang nhap thanh cong";
                }
                else
                    res = "Dang nhap hok thanh cong";
            }
            else if (lg.user != null || lg.pass != null)
            {
                res = "Sai ten dang nhap hoac mat khau";
            }

            return Json(res,JsonRequestBehavior.AllowGet);
        }
    }
}