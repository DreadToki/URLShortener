namespace ShortUrlTests;

[TestFixture]
public class UrlControllerTests
{
    [Test]
    public void ResolveUrl_ShortUrlExists_ReturnsRedirectResult()
    {
        var context = ContextGenerator.Generate();

        UrlPair url = new()
        {
            ShortUrl = "abc",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(url);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.ResolveUrl("abc");
        Assert.IsInstanceOf<RedirectResult>(result);
        Assert.That(((RedirectResult)result).Url, Is.EqualTo(url.LongUrl));
    }

    [Test]
    public void ResolveUrl_ShortUrlDoesNotExist_ReturnsNotFound()
    {
        var context = ContextGenerator.Generate();

        UrlPair url = new()
        {
            ShortUrl = "url",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(url);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.ResolveUrl("notFoundUrl") as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public void Read_ShortUrlExists_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        UrlPair url = new()
        {
            ShortUrl = "shortUrlExists",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(url);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.Read("shortUrlExists") as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void Read_ShortUrlDoesNotExist_ReturnsNotFound()
    {
        var context = ContextGenerator.Generate();

        UrlPair url = new()
        {
            ShortUrl = "shortUrlDoesNotExist",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(url);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.Read("shortUrl") as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public void ReadAll_UrlsExist_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        UrlPair firstRequestPush = new()
        {
            ShortUrl = "firstShortUrlRequest",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(firstRequestPush);

        UrlPair secondRequestPush = new()
        {
            ShortUrl = "secondShortUrlRequest",
            LongUrl = "https://www.microsoft.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };

        context.Add(secondRequestPush);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.ReadAll() as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void Delete_UrlExists_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        UrlPair firstRequestPush = new()
        {
            ShortUrl = "firstShortUrlRequest",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };
        context.Add(firstRequestPush);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.Delete("firstShortUrlRequest") as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void Delete_UrlDoesNotExist_ReturnsNotFound()
    {
        var context = ContextGenerator.Generate();

        UrlPair firstRequestPush = new()
        {
            ShortUrl = "shortUrlExist",
            LongUrl = "https://www.google.com",
            CreatedBy = "user",
            CreatedDateTime = DateTime.Now,
        };
        context.Add(firstRequestPush);
        context.SaveChanges();

        UrlController controller = new(context, new JwtService());

        var result = controller.Delete("shortUrlDoesNotExist") as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(404));
    }
}
