using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreRC2Demo.ViewComponents
{
    public class DemoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}