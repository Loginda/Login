using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Login_Asyn.Models;
using System.Threading.Tasks;

namespace Login_Asyn.Models
{
    public class Login
    {
        public string user { set; get; }
        public string pass { set; get; }

        static SessionEntities db = new SessionEntities();

        public async Task<bool> Check()
        {
            await Task.Delay(10);
            var list = db.ACCOUNTs.Where(x => x.UserName == user && x.Password == pass).ToList();
            if (list.Count == 0)
                return false;
            else return true;
        }
    }
}