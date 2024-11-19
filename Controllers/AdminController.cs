using EntityLayer;
using EntityLayer.SqlQuery;
using rent_A_carr.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using EntityLayer.Repository;
using System.Reflection;
using System.Security.Policy;
using System.Collections;

namespace rent_A_carr.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult AnaMenü()
        {

            return View();
        }

        public ActionResult Musteri_bilgi()
        {
            //using (QueryWareHouse dc = new QueryWareHouse())
            //{
            //    var v = from a in dc.musteri_bilgi select a;
            //    //sorting
            //    if(!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //    {
            //        v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    }

            //}

            GetMusteriBilgi("");
            return View();
        }

        //Müşteri silme işlemi
        [HttpPost]
        public JsonResult musteri_sil(string tc)
        {
            result res = new result();
            try
            {
                using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
                {
                    conn.Open();
                    int sonuc = conn.Execute(@"delete musteri_bilgi where TC=@TC", new { TC = tc });
                    if (sonuc > 0)
                    {
                        res.state = true;
                        res.message = "İşlem başarılı";
                    }
                }
            }
            catch (Exception ex)
            {
                res.state = false;
                res.message = ex.Message;
                res.error = ex.StackTrace;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        //MÜŞTERİ EKLE/GÜNCELLE
        [HttpPost]
        public ActionResult Musteri_Ekle(MusteriBilgi model)
        {
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();
                var kontrol = conn.Query<MusteriBilgi>(@"select * from musteri_bilgi where TC=@tc", new { tc = model.TC }).FirstOrDefault();
                if (kontrol == null)
                {
                    conn.Execute(@"insert into musteri_bilgi (TC,Ad,Soyad,Telefon,Email,Adres) values (@TC,@Ad,@Soyad,@Telefon,@Email,@Adres)", model);
                }
                else
                {
                    conn.Execute(@"update musteri_bilgi set  Ad=@Ad, Soyad=@Soyad, Telefon=@Telefon, Email=@Email ,Adres=@Adres where TC=@TC", model);
                }
            }

            return View();
        }


        //MÜŞTERİ DÜZENLE
        [HttpPost]
        public JsonResult Musterikontrol(string tc)
        {
            MusteriBilgi musteri = new MusteriBilgi();
            bool state = false;
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();
                musteri = conn.Query<MusteriBilgi>(@"select * from musteri_bilgi where TC=@Tc", new { Tc = tc }).FirstOrDefault();
                if (musteri != null) state = true;
            }
            return Json(new { state = state, musteri = musteri }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Musteri_bilgi(MusteriBilgi model)
        {
            MvcDbHelper.Repository.Insert(QueryWareHouse.MusteriBilgi.Insert, model);
            return View();


        }



        //MÜŞTERİ LİSTELE
        [HttpPost]
        public JsonResult listele(datatableModel data, string TC)

        {
            int totalRecords = 0;

            string fquery = "";

            var datableorder = data.order[0].column;

            if (!string.IsNullOrEmpty(data.search.value))
            {
                fquery += @"where (TC like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%' or Ad like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%' or Soyad like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%')";

            }

            string orderName = data.columns[datableorder].name;

            fquery += " order by " + orderName + " " + data.order[0].dir;

            fquery += " OFFSET " + data.start + " ROWS FETCH NEXT " + data.length + " ROWS ONLY;";

            List<MusteriBilgi> musteribilgi = GetMusteriBilgi(fquery);
            //totalRecords = v.Count();

            return Json(new { draw = data.draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = musteribilgi }, JsonRequestBehavior.AllowGet);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult Arac_bilgi()
        {
            GetAracBilgi(" where Visible=1");
            return View();
        }

        //ARAÇ DÜZENLE
        [HttpPost]
        public ActionResult arac_ekle(AracBilgi model)
        {
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();
                var kontrol = conn.Query<AracBilgi>(@"select * from arac_bilgi where Plaka=@plaka", new { plaka = model.Plaka }).FirstOrDefault();
                if (kontrol == null)
                {
                    conn.Execute(@"insert into arac_bilgi (Plaka,Marka,Modeli,Yakit,Yil,Vites,Son_KM) values (@Plaka,@Marka,@Modeli,@Yakit,@Yil,@Vites,@Son_KM)", model);
                }
                else
                {
                    conn.Execute(@"update arac_bilgi set Marka=@Marka, Modeli=@Modeli, Yakit=@Yakit ,Yil=@Yil, Vites=@Vites, Son_KM=@Son_KM where Plaka=@Plaka", model);
                }
            }

            return View();
        }


        //ARAÇ DÜZENLE
        [HttpPost]
        public JsonResult arackontrol(string Plaka)
        {
            AracBilgi arac = new AracBilgi();
            bool state = false;
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();
                arac = conn.Query<AracBilgi>(@"select * from arac_bilgi where Plaka=@Plaka", new { plaka = Plaka }).FirstOrDefault();
                if (arac != null) state = true;
            }
            return Json(new { state = state, arac = arac }, JsonRequestBehavior.AllowGet);
        }


        //ARAÇ SİLME İŞLEMİ
        [HttpPost]
        public JsonResult arac_sil(string Plaka)
        {
            result res = new result();
            try
            {
                using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
                {
                    conn.Open();
                    int sonuc = conn.Execute(@"update arac_bilgi set Visible =0 where Plaka=@Plaka", new { PLAKA = Plaka });
                    if (sonuc > 0)
                    {
                        res.state = true;
                        res.message = "İşlem başarılı";
                    }
                }
            }
            catch (Exception ex)
            {
                res.state = false;
                res.message = ex.Message;
                res.error = ex.StackTrace;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AracBilgi(AracBilgi model)
        {
            MvcDbHelper.Repository.Insert(QueryWareHouse.AracBilgi.Insert, model);
            return View();
        }

        //ARAÇ LİSTELE
        [HttpPost]
        public JsonResult listeleA(datatableModel data, string Plaka)

        {
            int totalRecords = 0;

            string fquery = "where Visible = 1 ";

            var datableorder = data.order[0].column;

            if (!string.IsNullOrEmpty(data.search.value))
            {
                fquery += @" and Plaka like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%' or Marka like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%' or Model like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%') ";

            }

            string orderName = data.columns[datableorder].name;

            fquery += " order by " + orderName + " " + data.order[0].dir;

            fquery += " OFFSET " + data.start + " ROWS FETCH NEXT " + data.length + " ROWS ONLY;";

            List<AracBilgi> AracBilgi = GetAracBilgi(fquery);


            return Json(new { draw = data.draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = AracBilgi }, JsonRequestBehavior.AllowGet);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult Kira_bilgileri()
        {

            return View();
        }

        //Kİra DÜZENLE-MÜŞTERİ SELECT2 
        [HttpPost]
        public JsonResult musteri_sec()
        {
            List<KeyValue> res = new List<KeyValue>();
            string connectionString = "Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;";
            string query = @"select TC as [Key] ,(Ad+' '+Soyad) as [Value] from musteri_bilgi";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = conn.Query(query);
                conn.Close();
                foreach (var kv in result)
                {
                    KeyValue nkey = new KeyValue();
                    nkey.Value = kv.Key;
                    nkey.Key = kv.Value;
                    res.Add(nkey);
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }


        //KİRA DÜZENLE-ARAÇ SELECT2 
        [HttpPost]
        public JsonResult arac_sec()
        {
            List<KeyValue> res = new List<KeyValue>();
            string connectionString = "Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;";
            string query = @"select Plaka as [Key] ,(Marka+' '+Modeli) as [Value] from arac_bilgi";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = conn.Query(query);
                conn.Close();
                foreach (var kv in result)
                {
                    KeyValue nkey = new KeyValue();
                    nkey.Value = kv.Key;
                    nkey.Key = kv.Value;
                    res.Add(nkey);
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }


        ////Kira DÜZENLE
        //[HttpPost]
        //public JsonResult kirakontrol(string tc)
        //{
        //    KiraBilgi kira = new KiraBilgi();
        //    bool state = false;
        //    using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
        //    {
        //        conn.Open();
        //        kira = conn.Query<KiraBilgi>(@"select * from kira_bilgileri where kira_bilgileri_ID=@kira_bilgileri_ID", new { kirabilgiID = kira }).FirstOrDefault();
        //        if (kira != null) state = true;
        //    }
        //    return Json(new { state = state, kira = kira }, JsonRequestBehavior.AllowGet);
        //}

        //KİRA EKLE
        [HttpPost]
        public ActionResult kira_ekle(KiraBilgi model, string TC, string Plaka)
        {
            using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
            {
                conn.Open();

                model.Musteri_ID = conn.QueryFirstOrDefault<string>("SELECT Musteri_ID FROM musteri_bilgi WHERE TC = @TC", new { TC });
                model.arac_ID = conn.QueryFirstOrDefault<string>("select arac_ID from arac_bilgi where Plaka = @Plaka", new { Plaka });


                conn.Execute(@"INSERT INTO [kira_bilgileri]
               (
                 [Alis_Tarihi]
                ,[Teslim_Tarihi]
                ,[Toplam_Tutar]
                ,[Musteri_ID]
                ,[arac_ID]
                )  VALUES 
                (
                 @Alis_Tarihi
                ,@Teslim_Tarihi
                ,@Toplam_Tutar
                ,@Musteri_ID
                ,@arac_ID
                )", model);
            }

            return View();
        }

        //KİRA SİL
        [HttpPost]
        public JsonResult KiraSil(string tc)
        {
            result res = new result();
            try
            {
                using (var conn = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
                {
                    conn.Open();
                    var kiraBilgileriIDs = conn.Query<int>(
                     @"SELECT kb.kira_bilgileri_ID
                      FROM musteri_bilgi m
                      JOIN kira_bilgileri kb ON m.Musteri_ID = kb.Musteri_ID
                      WHERE m.TC = @TC",
                    new { TC = tc });          

                    int sonuc = conn.Execute(@"delete kira_bilgileri where kira_bilgileri_ID = @kirabilgiID", new { kirabilgiID = kiraBilgileriIDs });
                    if (sonuc > 0)
                    {
                        res.state = true;
                        res.message = "İşlem başarılı";
                    }
                }
            }
            catch (Exception ex)
            {
                res.state = false;
                res.message = ex.Message;
                res.error = ex.StackTrace;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        //KİRA LİSTELE
        [HttpPost]
        public JsonResult Klistele(datatableModel data, string Plaka, string TC)

        {
            int totalRecords = 0;

            string fquery = "";

            var datableorder = data.order[0].column;

            if (!string.IsNullOrEmpty(data.search.value))
            {
                fquery += @"where (TC like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%' or Plaka like '%'+ cast('" + data.search.value + "' as varchar(10)) +'%')";

            }

            string orderName = data.columns[datableorder].name;

            fquery += " order by " + orderName + " " + data.order[0].dir;

            fquery += " OFFSET " + data.start + " ROWS FETCH NEXT " + data.length + " ROWS ONLY;";


            List<KiraBilgi> KiraBilgi = GetKiraBilgi(fquery);


            return Json(new { draw = data.draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = KiraBilgi }, JsonRequestBehavior.AllowGet);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      


        public ActionResult Rezervasyon_bilgi()
        {
            return View();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        [HttpPost]
        public ActionResult Kira_bilgileri(KiraBilgi model)
        {
            MvcDbHelper.Repository.Insert(QueryWareHouse.KiraBilgi.Insert, model);
            return View();
        }


        public List<MusteriBilgi> GetMusteriBilgi(string fque)
        {
            var musteribilgiResult = MvcDbHelper.Repository.GetAll<MusteriBilgi>(QueryWareHouse.MusteriBilgi.GetAll + fque).ToList();
            return musteribilgiResult;
        }

        public List<KiraBilgi> GetKiraBilgi(string fque)
        {
            var kirabilgiResult = MvcDbHelper.Repository.GetAll<KiraBilgi>(QueryWareHouse.KiraBilgi.GetAll + fque).ToList();
            return kirabilgiResult;
        }

        public List<AracBilgi> GetAracBilgi(string fque)
        {
            var aracbilgiResult = MvcDbHelper.Repository.GetAll<AracBilgi>(QueryWareHouse.AracBilgi.GetAll + fque).ToList();
            return aracbilgiResult;
        }
        public class datatableModel
        {
            public int draw { get; set; }
            public int start { get; set; }
            public int length { get; set; }
            public List<datatableOrder> order { get; set; }
            public List<datatableColumns> columns { get; set; }
            public datatableSearch search { get; set; }

        }
        public class datatableOrder
        {
            public int column { get; set; }
            public string dir { get; set; }

        }
        public class datatableColumns
        {
            public string data { get; set; }
            public string name { get; set; }
            public Boolean searchable { get; set; }
            public Boolean orderable { get; set; }
        }
        public class datatableSearch
        {
            public string value { get; set; }
            public bool regex { get; set; }
        }
        //silme işlemi//
        public class result
        {
            public bool state { get; set; }
            public string message { get; set; }
            public string data { get; set; }
            public string error { get; set; }
        }

    }
}/*  var sql = "SELECT * FROM musteri_bilgi where TC = @TC;";

                using (var connection = new SqlConnection("Data Source = DESKTOP-CV1CC8N\\SQLEXPRESS01; Initial Catalog = rent_a_car; Integrated Security=true;"))
                {
                    var rowsAffected = connection.Execute(sql, new { TC =TC});
                }
*/