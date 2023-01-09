using Emlak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Emlak.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext();
        // GET: Home
        public ActionResult Index()
        {
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;

            var ilan = db.Ilans.Include(m => m.Mahalle).Include(e => e.Tip);
            return View(ilan.ToList());
        }
        public ActionResult DurumList(int id)
        {
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;
            var ilan=db.Ilans.Where(i=>i.DurumId==id).Include(m => m.Mahalle).Include(e => e.Tip);
            return View(ilan.ToList());
        }
        public ActionResult MenuFiltre(int id)
        {
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;
            var filtre = db.Ilans.Where(i => i.TipId == id).Include(m => m.Mahalle).Include(e => e.Tip).ToList();
            return View(filtre);
        }

        public PartialViewResult PartialFiltre()
        {
            ViewBag.sehirlist = new SelectList(SehirGetir(), "SehirId", "SehirAd");
            ViewBag.durumlist = new SelectList(DurumGetir(), "DurumId", "DurumAd");
            return PartialView();
        }
        public ActionResult Filtre(int min, int max, int sehirid, int mahalleid, int semtid, int durumid,int tipid)
        {
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;

            var filtre = db.Ilans.Where(i => i.Fiyat >= min && i.Fiyat <= max
              && i.DurumId == durumid
              && i.SemtId == semtid
              && i.MahalleId == mahalleid
              && i.SehirId == sehirid
              && i.TipId == tipid).Include(m => m.Mahalle).Include(e => e.Tip).ToList();
            return View(filtre);
        }
        public List<Sehir> SehirGetir()
        {
            List<Sehir> sehirler = db.Sehirs.ToList();
            return sehirler;
        }
        public ActionResult SemtGetir(int SehirId)
        {
            List<Semt> semtlist = db.Semts.Where(x => x.SehirId == SehirId).ToList();
            ViewBag.semtlistesi = new SelectList(semtlist, "SemtId", "SemtAd");
            return PartialView("SemtPartial");
        }
        public ActionResult MahalleGetir(int SemtId)
        {
            List<Mahalle> mahallelist = db.Mahalles.Where(x => x.SemtId == SemtId).ToList();
            ViewBag.mahallelistesi = new SelectList(mahallelist, "MahalleId", "MahalleAd");
            return PartialView("MahallePartial");
        }
        public List<Durum> DurumGetir()
        {
            List<Durum> durumlar = db.Durums.ToList();
            return durumlar;
        }
        public ActionResult TipGetir(int DurumId)
        {
            List<Tip> tiplist = db.Tips.Where(x => x.DurumId == DurumId).ToList();
            ViewBag.tiplistesi = new SelectList(tiplist, "TipId", "TipAd");
            return PartialView("TipPartial");
        }
        public ActionResult Search(string q)
        {
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;
            var ara= db.Ilans.Include(m => m.Mahalle).Include(e => e.Tip);
            if (!string.IsNullOrEmpty(q))
            {
                ara = ara.Where(i => i.Açıklama.Contains(q) || i.Mahalle.MahalleAd.Contains(q) || i.Tip.TipAd.Contains(q));
            }
            return View(ara.ToList());
        }
        public ActionResult Detay(int id)
        {
            var ilan = db.Ilans.Where(i => i.IlanId == id).Include(m => m.Mahalle).Include(e => e.Tip).FirstOrDefault();
            var imgs = db.Resims.Where(i => i.IlanId == id).ToList();
            ViewBag.imgs = imgs;
            return View(ilan);
        }
        public PartialViewResult Slider()
        {
            var ilan = db.Ilans.ToList().Take(5);
            var imgs = db.Resims.ToList();
            ViewBag.imgs = imgs;
            return PartialView(ilan);
        }
    }
}