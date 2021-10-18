using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zadanie_Rekrutacyjne.Models;

namespace Zadanie_Rekrutacyjne.Controllers
{
    public class TreeViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TreeView
        public ActionResult Index()
        {
            return View(db.TreeViews.ToList());
        }

        public ActionResult Treeview(int? parentId, int? sortType)
        {

            if (parentId == null)
            {
                parentId = 0;
            }


            ViewBag.parentId = parentId;
            var lista = db.TreeViews.ToList();

            if (sortType == 1)
            {
                var listaAZ = db.TreeViews.OrderBy(x => x.Name).ToList();
                return PartialView(listaAZ);
            }
            else if (sortType == 2)
            {
                var listaZA = db.TreeViews.OrderByDescending(x => x.Name).ToList();
                return PartialView(listaZA);
            }

            return PartialView(lista);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult CreateChild(int id)
        {
  
            TreeView treeView = db.TreeViews.Find(id);
            if (treeView == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Rodzic = treeView.Name;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateChild(int id, TreeView treeView)
        {
            treeView.ParentId = id;

            if (ModelState.IsValid)
            {
                db.TreeViews.Add(treeView);
                db.SaveChanges();
                return RedirectToAction("Index", "TreeView");
            }

            return View();
        }


        // GET: TreeView/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id == 1)
            {
                return RedirectToAction("Index", "TreeView");
            }

            TreeView treeView = db.TreeViews.Find(id);
            if (treeView == null)
            {
                return HttpNotFound();
            }

            TreeView parent = db.TreeViews.Find(treeView.ParentId);
            ViewBag.Rodzic = parent.Name;
            return View(treeView);
        }


        // POST: TreeView/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool contains = true;
            TreeView treeView = db.TreeViews.Find(id);
            
            List<TreeView> toDelete = new List<TreeView>();
            toDelete.Add(treeView);

   
            List<int> parentIds = new List<int>();
            parentIds.Add(id);

           
            do
            {
                foreach (int i in parentIds)
                {
                    var nodes = db.TreeViews.Where(n => n.ParentId == i);

                    if (nodes != null)
                    {
                        foreach (var testNode in nodes)
                        {
                            TreeView t = db.TreeViews.Find(testNode.Id);
                            if (!(toDelete.Contains(t)))
                                toDelete.Add(t);
                        }
                    }


                }

                foreach (TreeView t in toDelete)
                {
                    contains = false;

                    if (!(parentIds.Contains(t.Id)))
                    {
                        contains = true;
                        parentIds.Add(t.Id);
                    }
                }


            } while (contains == true);

            foreach (TreeView t in toDelete)
            {
                db.TreeViews.Remove(t);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: TreeView/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreeView treeView = db.TreeViews.Find(id);
            if (treeView == null)
            {
                return HttpNotFound();
            }
            return View(treeView);
        }

        // POST: TreeView/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParentId")] TreeView treeView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treeView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(treeView);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Move(int? id)
        {
            if (id != 1)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TreeView treeView = db.TreeViews.Find(id);
                if (treeView == null)
                {
                    return HttpNotFound();
                }

                var node = db.TreeViews.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

                TreeView parent = db.TreeViews.Find(treeView.ParentId);

                ViewBag.CurrentParent = parent.Name;
                ViewBag.ParentId = new SelectList(node, "Value", "Text");
                return View(treeView);
            }
            return RedirectToAction("Index", "TreeView");
        }


        // POST: Tree/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move([Bind(Include = "Id,Name,ParentId")] TreeView treeView)
        {
            if (treeView.Id == treeView.ParentId)
                return View(treeView);

            if (ModelState.IsValid)
            {
                db.Entry(treeView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(treeView);
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
