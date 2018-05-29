using DMLTriggerGenerator.Utils;
using System.Web.Mvc;

namespace DMLTriggerGenerator.Attributes
{
    public class ConnectedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var sessionWrapper = new HttpContextSessionWrapper();
           
            if(sessionWrapper.ConnectionString == null)
            {
                filterContext.Result = new RedirectResult("~/Connection");
            }
        }
    }
}