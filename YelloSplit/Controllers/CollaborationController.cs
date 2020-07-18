using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YelloSplit.Controllers
{
    public class CollaborationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserCollaborations()
        {
            return View();
        }

        public IActionResult UserPendingCollabos()
        {
            return View();
        }
    }
}
