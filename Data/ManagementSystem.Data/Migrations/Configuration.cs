namespace ManagementSystem.Data.Migrations
{
    using ManagementSystem.Common;
    using ManagementSystem.Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ManagementSystem.Data;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ManagementSystem.Data.ManagementSystemDbContext>
    {
        private UserManager<User> userManager;

        Random random = new Random();

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            // TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ManagementSystemDbContext context)
        {
            this.userManager = new UserManager<User>(new UserStore<User>(context));

            if (context.Users.FirstOrDefault(u => u.Email == "manager@com.com") == null)
            {
                if (context.Roles.FirstOrDefault(r => r.Name == GlobalConstants.ManagerRole) == null)
                {
                    var adminRole = new IdentityRole(GlobalConstants.ManagerRole);
                    context.Roles.Add(adminRole);
                    context.SaveChanges();
                }

                var user = new User();
                user.Email = "manager@com.com";
                user.UserName = user.Email;
                this.userManager.Create(user, "manager");
                this.userManager.AddToRole(user.Id, GlobalConstants.ManagerRole);
                context.SaveChanges();
            }

            var usersInDb = context.Users.Count();
            var minUsers = 5;
            if (usersInDb < minUsers)
            {
                this.userManager = new UserManager<User>(new UserStore<User>(context));
                var nextUserNumber = 1;
                while (usersInDb < minUsers)
                {
                    var user = new User();
                    user.Email = "user" + nextUserNumber.ToString() + "@com.com";
                    if (context.Users.FirstOrDefault(u => u.Email == user.Email) == null)
                    {
                        user.UserName = user.Email;
                        this.userManager.Create(user, "111111");
                        context.SaveChanges();
                    }
                    usersInDb = context.Users.Count();
                    nextUserNumber++;
                }
            }

            while (context.Tasks.Count() < 10)
            {
                var task = new Task();
                var users = this.GetRandomUsers(context);
                foreach (var user in users)
                {
                    task.AssignedToUsers.Add(user);
                }
                
                task.Type = (TaskType)GetRandomNumber(0,2);
                task.RequiredByDate = DateTime.Now.AddDays(30);
                task.Status = TaskStatus.Opened;
                task.Description = "Test task";
                context.Tasks.Add(task);
                context.SaveChanges();
            }

        }

        public ICollection<User> GetRandomUsers(ManagementSystemDbContext context)
        {
            var randomUsers = new List<User>();
            var allUsers = context.Users.ToList();
            var usersCount = allUsers.Count();
            var usersToList = this.GetRandomNumber(1, usersCount - 1);
            for (int i = 0; i < usersToList; i++)
            {
                var randomNumber = this.GetRandomNumber(0, usersCount - 1);
                var randomUser = allUsers[randomNumber];
                if (randomUser.UserName=="manager@com.com")
                {
                    i--;
                }
                else
                {
                    randomUsers.Add(randomUser);
                }
            }
           
            return randomUsers;
        }

        public int GetRandomNumber(int min, int max)
        {
            return this.random.Next(min, max + 1);
        }
    }
}
