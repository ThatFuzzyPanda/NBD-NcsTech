﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Data;
using System.Diagnostics;

namespace NBDProjectNcstech.Data
{
    public static class ApplicationDbInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            ApplicationDbContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                //Create Roles
                var RoleManager = applicationBuilder.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Admin", "Management" , "Designer" ,"Sales"};
                IdentityResult roleResult;
                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
                //Create Users
                var userManager = applicationBuilder.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (userManager.FindByEmailAsync("admin@outlook.com").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "admin@outlook.com",
                        Email = "admin@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
				if (userManager.FindByEmailAsync("management@outlook.com").Result == null)
				{
					IdentityUser user = new IdentityUser
					{
						UserName = "management@outlook.com",
						Email = "management@outlook.com",
						EmailConfirmed = true
					};

					IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

					if (result.Succeeded)
					{
						userManager.AddToRoleAsync(user, "Management").Wait();
					}
				}
				if (userManager.FindByEmailAsync("designer@outlook.com").Result == null)
				{
					IdentityUser user = new IdentityUser
					{
						UserName = "designer@outlook.com",
						Email = "designer@outlook.com",
						EmailConfirmed = true,
					};

					IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

					if (result.Succeeded)
					{
						userManager.AddToRoleAsync(user, "Designer").Wait();
					}
				}

				if (userManager.FindByEmailAsync("sales@outlook.com").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "sales@outlook.com",
                        Email = "sales@outlook.com",
                        EmailConfirmed = true,
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Sales").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("user@outlook.com").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "user@outlook.com",
                        Email = "user@outlook.com",
                        EmailConfirmed = true,
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;
                    //Not in any role
                }

                // ADDITIONAL USERS
                if (userManager.FindByEmailAsync("jkaluba@niagaracollege.ca").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "jkaluba@niagaracollege.ca",
                        Email = "jkaluba@niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("dkendell@niagaracollege.ca").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "dkendell@niagaracollege.ca",
                        Email = "dkendell@niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
