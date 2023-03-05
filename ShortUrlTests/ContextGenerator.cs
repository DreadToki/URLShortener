using Microsoft.EntityFrameworkCore;
using ShortUrl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrlTests;

public static class ContextGenerator
{
    public static DataContext Generate()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: "ShortUrl.Tests")
        .Options;

        using var context = new DataContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return new DataContext(options);
    }
}
