using System;
using System.Collections.Generic;
using System.IO;
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
                    UserName = email1,
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
                    UserName = email2,
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

            // Article 1
            {
                var article = new Article()
                {
                    Title = "A humanist approach to teaching kids",
                    Author = await userManager.FindByEmailAsync(email1),
                    CreationDate = new DateTime(2018, 03, 13)
                };

                article.Body = Pressford.Properties.Resources.Article1;

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

            // Article 2
            {
                var article = new Article()
                {
                    Title = "5 amazing books I read this year",
                    Author = await userManager.FindByEmailAsync(email1),
                    CreationDate = new DateTime(2017, 12, 04)
                };

                article.Body = Pressford.Properties.Resources.Article2;

                await context.Articles.AddAsync(article);
                await context.SaveChangesAsync();

                var alanTuring = await userManager.FindByEmailAsync("alan.turing@gmail.com");

                var comment = new ArticleComment()
                {
                    Article = article,
                    Commenter = alanTuring,
                    Text = "Great article again !! Thanks for sharing !",
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

            // Article 3
            {
                var article = new Article()
                {
                    Title = "Strength in Numbers",
                    Author = await userManager.FindByEmailAsync(email1),
                    CreationDate = new DateTime(2018, 04, 18)
                };

                article.Body = Pressford.Properties.Resources.Article3;

                await context.Articles.AddAsync(article);
                await context.SaveChangesAsync();

                var alanTuring = await userManager.FindByEmailAsync("alan.turing@gmail.com");

                var comment = new ArticleComment()
                {
                    Article = article,
                    Commenter = alanTuring,
                    Text = "Wonderful !! Thanks for sharing !",
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
}
