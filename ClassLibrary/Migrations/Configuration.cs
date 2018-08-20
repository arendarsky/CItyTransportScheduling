namespace ClassLibrary.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ClassLibrary.Interfaces;

    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            IStorage repo = Factory.Instance.GetFileStorage(true);
            foreach (var s in repo.Stations.Items)
            {
                context.Stations.AddOrUpdate(c => c.Name, s);
            }
            foreach (var r in repo.Routes.Items)
            {
                context.Routes.AddOrUpdate(c => c.Name, r);
            }
            foreach (var u in repo.Users.Items)
            {
                context.Users.AddOrUpdate(c => c.Login, u);
            }
        }
    }
}
