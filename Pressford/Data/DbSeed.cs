using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pressford.Models;

namespace Pressford.Data
{
    public static class DbSeed
    {
        public static async Task Initialize(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create Publisher Role :
            if (!await roleManager.RoleExistsAsync("Publisher"))
            {
                var publisherRole = new IdentityRole("Publisher");

                await roleManager.CreateAsync(publisherRole);
            }

            // Create Reader Role :
            if (!await roleManager.RoleExistsAsync("Reader"))
            {
                var readerRole = new IdentityRole("Reader");

                await roleManager.CreateAsync(readerRole);
            }

            // Create User Scott Pressfor (Publisher) :
            const string email1 = "scott.pressford@gmail.com";

            if (await userManager.FindByEmailAsync(email1) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "spressford",
                    Email = email1,
                    FirstName = "Scott",
                    LastName = "Pressford",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, "Password2018!");

                if (result.Succeeded && !await userManager.IsInRoleAsync(user, "Publisher"))
                {
                    await userManager.AddToRoleAsync(user, "Publisher");
                }
            }

            // Create User Alan Turing (Reader) :
            const string email2 = "alan.turing@gmail.com";

            if (await userManager.FindByEmailAsync(email2) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "aturing",
                    Email = email2,
                    FirstName = "Alan",
                    LastName = "Turing",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, "Password2018!");

                if (result.Succeeded && !await userManager.IsInRoleAsync(user, "Reader"))
                {
                    await userManager.AddToRoleAsync(user, "Reader");
                }
            }

            // Create Articles + Likes + Comments : 
            if (context.Articles.Any())
            {
                return;
            }

            var article = new Article()
            {
                Title = "Introducing ASP.NET 5",
                Author = await userManager.FindByEmailAsync(email1),
                CreationDate = new DateTime(2015, 02, 23)
            };

            article.Body = "<p>The first preview release of ASP.NET 1.0 came out almost 15 years ago.  Since then millions of developers have used it to build and run great web applications, and over the years we have added and evolved many, many capabilities to it.</p>";
            article.Body += "<p>I'm excited today to post about a new release of ASP.NET that we are working on that we are calling ASP.NET 5.  This new release is one of the most significant architectural updates we've done to ASP.NET.  As part of this release we are making ASP.NET leaner, more modular, cross-platform, and cloud optimized.  The ASP.NET 5 preview is now available as a preview release, and you can start using it today by downloading the latest CTP of Visual Studio 2015 which we just made available.</p>";
            article.Body += "<p>ASP.NET 5 is an open source web framework for building modern web applications that can be developed and run on Windows, Linux and the Mac. It includes the MVC 6 framework, which now combines the features of MVC and Web API into a single web programming framework.  ASP.NET 5 will also be the basis for SignalR 3 - enabling you to add real time functionality to cloud connected applications. ASP.NET 5 is built on the .NET Core runtime, but it can also be run on the full .NET Framework for maximum compatibility.</p>";

            await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();

            var alanTuring = await userManager.FindByEmailAsync("alan.turing@gmail.com");

            var comment = new ArticleComment()
            {
                Article = article,
                Commenter = alanTuring,
                Text = "Great article !! Thanks for sharing !",
                TimeStamp = DateTime.Now
            };

            await context.ArticleComments.AddAsync(comment);
            await context.SaveChangesAsync();

            var like = new ArticleLike()
            {
                Article = article,
                Liker = alanTuring,
                TimeStamp = DateTime.UtcNow
            };

            await context.ArticleLikes.AddAsync(like);
            await context.SaveChangesAsync();
        }
    }
}
