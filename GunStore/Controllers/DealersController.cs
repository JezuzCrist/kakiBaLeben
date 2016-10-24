﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using GunStore.Models;

namespace GunStore.Controllers
{
    [Authorize]
    public class DealersController : Controller
    {
        private ComicsContextDb db = new ComicsContextDb();

        // GET: Dealers
        public ActionResult Index()
        {
            return View(db.Dealers.ToList());
        }

        // GET: Dealers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: Dealers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dealers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,City,Street,Reliability")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,City,Street,Reliability")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }

        // GET: Dealers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dealer dealer = db.Dealers.Find(id);
            db.Dealers.Remove(dealer);
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

        [AllowAnonymous]
        public ActionResult Search(string firstName, string lastName, string city)
        {
            var dealersFiltered = db.Dealers.Include(x => x.Guns);

            if (!string.IsNullOrEmpty(firstName))
            {
                dealersFiltered = dealersFiltered.Where(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                dealersFiltered = dealersFiltered.Where(x => x.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(city))
            {
                dealersFiltered = dealersFiltered.Where(x => x.City.Contains(city));
            }
            return View(dealersFiltered.ToList());
        }

        public string GroupByReliability()
        {
            var dealers = db.Dealers.GroupBy(dealer => dealer.Reliability,
                               dealer => dealer.FirstName + " " + dealer.LastName,
                               (key, g) => new {
                                   Reliability = key,
                                   Dealers = g.ToList()
                               }
                              ).ToList();

            var resultDealers = new List<GroupByObject>();
            dealers.ForEach(d => resultDealers.Add(new GroupByObject(d.Reliability, Concat(d.Dealers))));

            return JsonConvert.SerializeObject(resultDealers);
        }

        private static string Concat(IEnumerable<string> source)
        {
            var sb = new StringBuilder();
            foreach (var s in source)
            {
                sb.Append(s + ", ");
            }
            if (sb.Length > 0)
            {
                var commaLength = ", ".Length;
                sb.Remove(sb.Length - commaLength, commaLength);
            }
            return sb.ToString();
        }

        [JsonObject(MemberSerialization.OptIn)]
        private class GroupByObject
        {
            [JsonProperty]
            private int Reliability { get; set; }
            [JsonProperty]
            private string Dealers { get; set; }

            public GroupByObject(int reliability, string dealers)
            {
                Reliability = reliability;
                Dealers = dealers;
            }
        }
    }

}
