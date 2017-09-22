﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DotAPicker.Models;

namespace DotAPicker.Controllers
{
    public class HeroController : DotAController
    {
        // GET: Hero
        public ActionResult Index()
        {
            return View("Heroes", db.Heroes);
        }

        // GET: Hero/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hero/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Hero/Create
        [HttpPost]
        public ActionResult Create(Hero model)
        {
            if (db.Heroes.Any(h => h.Name == model.Name))
            {
                throw new Exception("This name is already in use");
            }

            db.Heroes.Add(model);
            db.Save();
            return Index();
        }

        // GET: Hero/Edit/5
        public ActionResult Edit(string id)
        {
            var hero = db.Heroes.FirstOrDefault(h => h.Name == id);
            if (hero == null)
            {
                throw new Exception("Hero not found.");
            }

            return View("Edit", hero);
        }

        // POST: Hero/Edit/5
        [HttpPost]
        public ActionResult Edit(Hero model)
        {
            var hero = db.Heroes.FirstOrDefault(h => h.Name == model.Name);
            if (hero == null)
            {
                throw new Exception("Hero not found.");
            }

            //Repace the hero with the edited hero
            db.Heroes.Remove(hero);
            db.Heroes.Add(model);
            
            db.Save();
            db = DotADB.Load();
            return Index();
        }

        // GET: Hero/Delete/5
        public ActionResult Delete(string id)
        {
            var hero = db.Heroes.FirstOrDefault(h => h.Name == id);
            if (hero == null)
            {
                throw new Exception("Hero not found.");
            }

            db.Heroes.Remove(hero);

            db.Save();
            db = DotADB.Load();
            return Index();
        }
    }
}
