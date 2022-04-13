using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Web.Mvc.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Pager()
        {
            return View();
        }
    }
}
