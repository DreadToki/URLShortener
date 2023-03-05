﻿using Microsoft.EntityFrameworkCore;

namespace ShortUrl.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<UrlPair> UrlPairs { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<About> About { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(new User
        {
            Login = "user",
            Password = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b", //Pass: 1
            Role = UserRole.User,
        }, new User
        {
            Login = "admin",
            Password = "4fc82b26aecb47d2868c4efbe3581732a3e7cbcc6c2efb32062c08170a05eeb8", //Pass: 11
            Role = UserRole.Admin,
        });

        modelBuilder.Entity<UrlPair>().HasData(new UrlPair
        {
            ShortUrl = "1",
            LongUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
            CreatedBy = "admin",
            CreatedDateTime = DateTime.Now,
        });

        modelBuilder.Entity<About>().HasData(new About
        {
            Id = 1,
            Text = "Hi, let's get to business straight away.\r\nFirst of all, my \"URL shortener\" algorithm. Logic of my Generate method:\r\n\r\n    private const string charset = \"_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789\";  (1)\r\n\r\n    private static readonly Random random = new();\r\n\r\n    public static string Generate(int length)  (2)\r\n    {\r\n        char[] chars = new char[length];\r\n        for (int i = 0; i < length; i++)  (3)\r\n        {\r\n            chars[i] = charset[random.Next(charset.Length)];\r\n        }\r\n        return new string(chars);  (4)\r\n    }\r\n\r\n1) I'm defining a string, that contains a set of numbers and letters.\r\n2) In \"Generate\" method I'm defining an array of chars that depends on length property. (By default length is equal to \"4\")\r\n3) I initialize a \"for loop\" to pick a random char from the \"charset\" and write it down in an array of chars.\r\n4) I return the value from \"chars\".\r\n-----------------------------------------------------------------------------\r\nMy next step was to call the \"Generate\" method from my controller:\r\n\r\n    string shortUrl;\r\n    int length = 4;\r\n    do\r\n    {\r\n        shortUrl = Generate(length);\r\n        length++;\r\n    }\r\n    while (_dataContext.UrlPairs.Any(u => u.ShortUrl == shortUrl));\r\n\r\nBasically, it's just initializing a string for my short URL and I'm defining a default length for it.\r\nI used \"do while\" loop to call my Generate method and create a short representation of long URL.\r\nAnd I made a condition that checks whether this \"shortUrl\" has already been in a system. If it's true, I'm gonna pick a next random char, if not - I'm gonna write it down in a database.\r\n\r\nPros:\r\n- really short URL, that you can type it in your search bar even with your bare hands;\r\n- this algorithm is not that complicated for a system and programmers in future can update it properly;\r\n\r\nCons:\r\n- if system has already have a lot of short URL representations, then this controller has to make too much calls to a database in order to fit the condition;\r\n\r\nAll in all, I think I chose the best algorithm that suits for this project. Eventually I will upgrade this system in case if I have too many users.",
        });
    }
}
