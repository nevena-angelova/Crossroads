namespace Crossroads.Data.Migrations
{
    using Crossroads.Common;
    using Crossroads.Models;
    using Crossroads.Models.Profile;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Crossroads.Data.CrossroadsDbContext>
    {

        private UserManager<User> userManager;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(CrossroadsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            this.userManager = new UserManager<User>(new UserStore<User>(context));

            this.SeedRoles(context);
            this.SeedUsers(context);
            this.SeedInterests(context);
            this.SeedMusicGenres(context);
        }

        private void SeedRoles(CrossroadsDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.AdminRole));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.ModeratorRole));
            context.SaveChanges();
        }

        private void SeedUsers(CrossroadsDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            User user = new User
            {
                Email = "test-user@gmail.com",
                UserName = "TestUser"
            };

            this.userManager.Create(user, "123456");

            UserProfile profile = new UserProfile
            {
                ProfileUser = user,
                DateCreated = DateTime.Now,
                ForumPoints = 0
            };

            context.Profiles.Add(profile);

            User adminUser = new User
            {
                Email = "admin@gmail.com",
                UserName = "Administrator"
            };

            this.userManager.Create(adminUser, "123456");

            this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdminRole);

            UserProfile adminProfile = new UserProfile
            {
                ProfileUser = adminUser,
                DateCreated = DateTime.Now,
                ForumPoints = 0
            };

            context.Profiles.Add(adminProfile);

            User moderatorUser = new User
            {
                Email = "moderator@gmail.com",
                UserName = "Moderator"
            };

            this.userManager.Create(moderatorUser, "123456");

            this.userManager.AddToRole(moderatorUser.Id, GlobalConstants.ModeratorRole);

            UserProfile moderatorProfile = new UserProfile
            {
                ProfileUser = moderatorUser,
                DateCreated = DateTime.Now,
                ForumPoints = 0
            };

            context.Profiles.Add(moderatorProfile);

            context.SaveChanges();
        }

        private void SeedInterests(CrossroadsDbContext context)
        {
            if (context.Interests.Any())
            {
                return;
            }

            ProfileInterest cooking = new ProfileInterest()
            {
                Name = "Готвене"
            };

            context.Interests.Add(cooking);

            ProfileInterest technologies = new ProfileInterest()
            {
                Name = "Технологии"
            };

            context.Interests.Add(technologies);

            ProfileInterest mistery = new ProfileInterest()
            {
                Name = "Мистерии"
            };

            ProfileInterest sports = new ProfileInterest()
            {
                Name = "Спорт"
            };

            context.Interests.Add(sports);

            context.SaveChanges();

        }

        private void SeedMusicGenres(CrossroadsDbContext context)
        {
            if (context.MusicGenres.Any())
            {
                return;
            }

            MusicGenre rock = new MusicGenre()
            {
                Name = "Рок"
            };

            context.MusicGenres.Add(rock);

            MusicGenre folkMetal = new MusicGenre()
            {
                Name = "Фолк Метал"
            };

            context.MusicGenres.Add(folkMetal);

            MusicGenre alternative = new MusicGenre()
            {
                Name = "Алтърнатив"
            };

            MusicGenre punk = new MusicGenre()
            {
                Name = "Пънк"
            };

            context.MusicGenres.Add(punk);

            context.SaveChanges();
        }
    }
}
