using Dapper;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace rent_A_carr.Controllers
{
    public class KullanıcıController : Controller
    {
        // GET: Kullanıcı
        public ActionResult Anasayfa()
        {
            return View();
        }

        
        public ActionResult Araclar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult arac_detay()
        {
            List<AracBilgi> aracBilgi = new List<AracBilgi>();
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();
                aracBilgi = conn.Query<AracBilgi>(@"SELECT Plaka,Marka,Modeli,Yakit,Yil,Vites,Son_KM FROM arac_bilgi where Visible=1").ToList();

            }

            return Json(aracBilgi, JsonRequestBehavior.AllowGet);
        }


        public ActionResult iletişim()
        {
            return View();
        }
        
        
        public ActionResult login()
        {
            return View();
        }
    }
}