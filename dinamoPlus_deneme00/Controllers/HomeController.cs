using Microsoft.AspNetCore.Mvc;
using dinamoPlus_deneme00.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dinamoPlus_deneme00.Controllers
{
    public class HomeController : Controller
    {
        // loglar için liste oluşturuyoruz
        private static List<LogEntry> loglar =new List<LogEntry>();
        // aktif islem için değişken
        private static LogEntry aktifLog = null;
        // Index sayfasına gittiğinde tüm logları göster
        private static int kayitSayaci = 1;
        public IActionResult Index()
        {
            OtomatikKontrol(); 
            return View(loglar);
            
        }
        [HttpPost]
        private void OtomatikKontrol()
        {

            var now = DateTime.Now;
            int currentHour = now.Hour;
            int currentMinute = now.Minute;

            // aktif işlemi mola saatinde durdur
            if (aktifLog != null)
            {
                if ((currentHour == 10 && currentMinute == 0) ||
                    (currentHour == 12 && currentMinute == 0) ||
                    (currentHour == 15 && currentMinute == 0) ||
                    (currentHour == 0 && currentMinute == 30))
                {
                    aktifLog.DurusZamani = DateTime.Now;

                    if (currentHour == 10)
                        aktifLog.DurusNedeni = "Çay Molası";
                    else if (currentHour == 12)
                        aktifLog.DurusNedeni = "Yemek Molası";
                    else if (currentHour == 15)
                        aktifLog.DurusNedeni = "2. Çay Molası";
                    else if (currentHour == 0 && currentMinute == 30)
                        aktifLog.DurusNedeni = "Gece Çayı Molası";

                    aktifLog.KayitNo = kayitSayaci++;
                    loglar.Add(aktifLog);

                    aktifLog = null; // aktif logu boşaltıyoruzki bir sonraki işleme hazır hale gelsiin
                }
            }
            // Eğer sistem duruyorsa ve mola süresi bittiyse otomatik başlat
            else if (loglar.Count > 0)
            {
                var sonLog = loglar.LastOrDefault();
                if (sonLog != null && sonLog.DurusZamani.HasValue && !string.IsNullOrEmpty(sonLog.DurusNedeni))
                {
                    var gecenSure = DateTime.Now - sonLog.DurusZamani.Value;

                    if ((sonLog.DurusNedeni == "Çay Molası" && gecenSure.TotalMinutes >= 15) ||
                        (sonLog.DurusNedeni == "2. Çay Molası" && gecenSure.TotalMinutes >= 15) ||
                        (sonLog.DurusNedeni == "Yemek Molası" && gecenSure.TotalMinutes >= 30)||
                        (sonLog.DurusNedeni == "Gece Çayı Molası" && gecenSure.TotalMinutes >= 1))
                    {
                        aktifLog = new LogEntry()
                        {
                            BaslangicZamani = DateTime.Now
                        };
                    }
                }
            }
        }

        public IActionResult Basla()
        {
            if (aktifLog == null)
            {
                aktifLog = new LogEntry()
                {
                    BaslangicZamani = DateTime.Now
                };
                
            }
            return RedirectToAction("Index");
        }   
        [HttpPost]
        public IActionResult Dur(string durusNedeni)
        {
            if (aktifLog != null)
            {
                aktifLog.DurusZamani = DateTime.Now;
                var cay = new[] { 10};
                var yemek = new[] { 12};
                var cay2 = new[] { 15 };
                    
                if (cay.Contains(aktifLog.DurusZamani.Value.Hour))
                {
                    aktifLog.DurusNedeni = "Çay Molası";
                }
                else if (yemek.Contains(aktifLog.DurusZamani.Value.Hour))
                {
                    aktifLog.DurusNedeni = "Yemek Molası";
                }
                else if (cay2.Contains(aktifLog.DurusZamani.Value.Hour))
                {
                    aktifLog.DurusNedeni = "2. Çay Molası";
                }
                else
                {
                    aktifLog.DurusNedeni = string.IsNullOrWhiteSpace(durusNedeni) ? "" : durusNedeni;
                }


                aktifLog.KayitNo = kayitSayaci++;
                loglar.Add(aktifLog);

                aktifLog = null;

                Basla();
            }
            return RedirectToAction("Index");
        }
    }   
}
