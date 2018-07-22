using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Red_Black_Tree;
using System.Threading.Tasks;
using Login_Asyn.Models;

namespace Login_Asyn.Controllers
{
    public class Tree
    {
        SessionEntities db = new SessionEntities();

        private static Red_Black rb = null;

        public Tree()
        {
            if (rb == null)
                rb = new Red_Black();
        }

        public bool isEmpty()
        {
            return rb.isEmpty();
        }

        public void Add(DateTime x, ACCOUNT y)
        {
            rb.Add(x, y);
        }

        public Node GetMinNode()
        {
            if (rb.isEmpty())
                return null;
            return rb.GetMinNode();
        }

        public void Delete(Node z)
        {
            rb.Delete(z);
        }

        public async Task<Node> DeleteMin()
        {
            await Task.Delay(10);
            Node min = rb.GetMinNode();
            if (min != null && DateTime.Compare(min.key, DateTime.Now) < 0)
            {
                rb.Delete(min);
                return min;
            }
            return null;
        }

        public Node FindNode(ACCOUNT x)
        {
            var dt = db.ACCOUNTs.Where(k => k.UserName == x.UserName).ToList()[0];
            
            if (dt.TimeOut == null)
                return null;
            else
            {
                return rb.FindNode(dt);
            }
        }
    }
}