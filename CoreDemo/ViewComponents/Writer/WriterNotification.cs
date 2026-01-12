using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterNotification:ViewComponent
    {
        NotificationManager nm = new NotificationManager(new EfNotificationRepository());
        public IViewComponentResult Invoke() //Invoke metoduyla notification bildirimlerini açmak,ilgili verileri getirmek
        {
            var values = nm.GetList();
            return View(values);
        }
    }
}
