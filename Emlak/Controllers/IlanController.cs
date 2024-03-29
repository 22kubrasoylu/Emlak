﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Emlak.Models;
using System.IO;

namespace Emlak.Controllers
{
    public class IlanController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Ilan
        public ActionResult Index()
        {
            var username = User.Identity.Name;

            var ilans = db.Ilans.Where(i=>i.UserName==username).Include(i => i.Mahalle).Include(i => i.Tip);
            return View(ilans.ToList());
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
        public ActionResult Images(int id)
        {
            var ilan = db.Ilans.Where(i => i.IlanId == id).ToList();
            var rsml = db.Resims.Where(i => i.IlanId == id).ToList();
            ViewBag.rsml = rsml;
            ViewBag.ilan = ilan;
            return View();
        }
        [HttpPost]
        public ActionResult Images(int id,HttpPostedFileBase file)
        {
            string path = Path.Combine("/Content/images/" + file.FileName);
            file.SaveAs(Server.MapPath(path));
            Resim rsm = new Resim();
            rsm.ResimAd = file.FileName.ToString();
            rsm.IlanId = id;
            db.Resims.Add(rsm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Ilan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ilan ilan = db.Ilans.Find(id);
            if (ilan == null)
            {
                return HttpNotFound();
            }
            return View(ilan);
        }

        // GET: Ilan/Create
        public ActionResult Create()
        {
            ViewBag.sehirlist = new SelectList(SehirGetir(), "SehirId", "SehirAd");
            ViewBag.durumlist = new SelectList(DurumGetir(), "DurumId", "DurumAd");
            return View();
        }

        // POST: Ilan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IlanId,Açıklama,Fiyat,OdaSayisi,BanyoSayisi,Kredi,Alan,Kat,Telefon,Adres,UserName,SehirId,SemtId,DurumId,MahalleId,TipId")] Ilan ilan)
        {
            if (ModelState.IsValid)
            {
                ilan.UserName = User.Identity.Name;
                db.Ilans.Add(ilan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sehirlist = new SelectList(SehirGetir(), "SehirId", "SehirAd");
            ViewBag.durumlist = new SelectList(DurumGetir(), "DurumId", "DurumAd");
            return View(ilan);
        }

        // GET: Ilan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ilan ilan = db.Ilans.Find(id);
            if (ilan == null)
            {
                return HttpNotFound();
            }
            ViewBag.sehirlist = new SelectList(SehirGetir(), "SehirId", "SehirAd");
            ViewBag.durumlist = new SelectList(DurumGetir(), "DurumId", "DurumAd");
            ViewBag.SemtId = new SelectList(db.Semts, "SemtId", "SemtAd", ilan.SemtId);
            ViewBag.MahalleId = new SelectList(db.Mahalles, "MahalleId", "MahalleAd", ilan.MahalleId);
            ViewBag.TipId = new SelectList(db.Tips, "TipId", "TipAd", ilan.TipId);
            return View(ilan);
        }

        // POST: Ilan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IlanId,Açıklama,Fiyat,OdaSayisi,BanyoSayisi,Kredi,Alan,Kat,Telefon,Adres,UserName,SehirId,SemtId,DurumId,MahalleId,TipId")] Ilan ilan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ilan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sehirlist = new SelectList(SehirGetir(), "SehirId", "SehirAd");
            ViewBag.durumlist = new SelectList(DurumGetir(), "DurumId", "DurumAd");
            return View(ilan);
        }

        // GET: Ilan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ilan ilan = db.Ilans.Find(id);
            if (ilan == null)
            {
                return HttpNotFound();
            }
            return View(ilan);
        }

        // POST: Ilan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ilan ilan = db.Ilans.Find(id);
            db.Ilans.Remove(ilan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
