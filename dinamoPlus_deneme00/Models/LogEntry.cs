﻿using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace dinamoPlus_deneme00.Models
{
    //public class LogEntry : Controller
    public class LogEntry
    {
        public int KayitNo { get; set; }
        public DateTime?  BaslangicZamani { get; set; }
        public DateTime? DurusZamani { get; set; }
        public DateTime? AnlikZaman { get; set; }
        public string DurusNedeni { get; set; }

        public string ToplamSure
        {
            get
            {
                if (BaslangicZamani.HasValue && DurusZamani.HasValue)
                {
                    return (DurusZamani.Value - BaslangicZamani.Value).ToString(@"hh\:mm");
                }
                return "-"; // eğer zamanlar boşsa  "-" döndürür
            }
        }

        public string Statu
        {
            get
            {
                return string.IsNullOrEmpty(DurusNedeni) ? "ÜRETİM" : "DURUŞ";
            }
        }
    }
}
