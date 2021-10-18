namespace Zadanie_Rekrutacyjne.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Zadanie_Rekrutacyjne.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Zadanie_Rekrutacyjne.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Zadanie_Rekrutacyjne.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [TreeViews]");
            var treeViews = new List<TreeView>()
            {
                new TreeView { Id = 1, Name = "Katalog g³ówny",   ParentId = 0 },
                new TreeView { Id = 2, Name = "1",   ParentId = 1 },
                new TreeView { Id = 3, Name = "1.1",   ParentId = 2 },
                new TreeView { Id = 4, Name = "1.1",   ParentId = 2 },
                new TreeView { Id = 5, Name = "2",   ParentId = 1 },
                new TreeView { Id = 6, Name = "2.2",   ParentId = 5 },
                new TreeView { Id = 7, Name = "2.3",   ParentId = 5 },
                new TreeView { Id = 8, Name = "2.2.1", ParentId =  6},
                new TreeView { Id = 9, Name = "3",   ParentId = 1 },
                new TreeView { Id = 10, Name = "3.1",   ParentId = 9 },
                new TreeView { Id = 11, Name = "3.2",   ParentId = 9 }
            };
            foreach (TreeView treeView in treeViews)
                context.TreeViews.Add(treeView);
            context.SaveChanges();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string rola = "Admin";
            IdentityResult roleResult;

            if (!RoleManager.RoleExists(rola))
            {
                roleResult = RoleManager.Create(new IdentityRole(rola));
            }


            string e_mail = "patryk@onet.pl";
     
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (UserManager.FindByEmail(e_mail) == null)
            {
                var Admin = new ApplicationUser { UserName = e_mail, Email = e_mail };
                UserManager.Create(Admin, "Admin@1");
                UserManager.AddToRole(Admin.Id, "Admin");
            }





        }
    }
}
