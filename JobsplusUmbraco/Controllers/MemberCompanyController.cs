using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace JobsplusUmbraco.Controllers
{
    /// <summary>
    /// Všechny operace v rámci zabezpečené ADMIN sekce firmy na FrontEndu.
    /// </summary>
    public class MemberCompanyController : SurfaceController
    {
        // GET: CompanyAdmin
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}