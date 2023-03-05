using Microsoft.AspNetCore.Mvc;
using ShortUrl.Dtos;
using ShortUrl.Helpers;
using ShortUrl.Models;
using ShortUrl.Services;

using static ShortUrl.Helpers.ShortUrlGenerator;

namespace ShortUrl.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : Controller
{
    private readonly DataContext _dataContext;
    private readonly JwtService _jwtService;

    public UrlController(DataContext dataContext, JwtService jwtService)
    {
        _dataContext = dataContext;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Route("/{shortUrl}")]
    public IActionResult ResolveUrl(string shortUrl)
    {
        var record = _dataContext.UrlPairs.FirstOrDefault(uriMap => uriMap.ShortUrl == shortUrl);
        if (record == null)
        {
            return NotFound("The URL was not found in the system.");
        }

        return Redirect(record.LongUrl);
    }

    [HttpGet]
    [Route("{shortUrl}")]
    public IActionResult Read(string shortUrl)
    {
        var pair = _dataContext.UrlPairs.FirstOrDefault(u => u.ShortUrl == shortUrl);
        if (pair == null)
        {
            return NotFound("The URL was not found in the system.");
        }
        UrlPairDto response = new()
        {
            ShortUrl = pair.ShortUrl,
            LongUrl = pair.LongUrl,
            CreatedBy = pair.CreatedBy,
            CreatedDateTime = pair.CreatedDateTime.FormatDateTime(),
        };

        return Ok(response);
    }

    [HttpGet]
    [Route("read")]
    public IActionResult ReadAll()
    {
        var records = _dataContext.UrlPairs.ToList();
        List<UrlPairDto> response = records.Select(
            r => new UrlPairDto
            {
                ShortUrl = r.ShortUrl,
                LongUrl = r.LongUrl,
                CreatedBy = r.CreatedBy,
                CreatedDateTime = r.CreatedDateTime.FormatDateTime(),
            }).ToList();

        return Ok(response);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult Create(CreateUrlDto dto)
    {
        var jwt = Request.Cookies["jwt"];

        var token = _jwtService.Verify(jwt);

        var user = _dataContext.Users.FirstOrDefault(u => u.Login == token.Issuer);

        if (user == null)
        {
            return Unauthorized("Unauthorized user.");
        }

        if (_dataContext.UrlPairs.Any(u => u.LongUrl == dto.LongUrl))
        {
            return Conflict("This URL already exists in the database.");
        }

        string shortUrl;
        int length = 4;
        do
        {
            shortUrl = Generate(length);
            length++;
        }
        while (_dataContext.UrlPairs.Any(u => u.ShortUrl == shortUrl));

        _dataContext.UrlPairs.Add(new UrlPair
        {
            ShortUrl = shortUrl,
            LongUrl = dto.LongUrl,
            CreatedDateTime = DateTime.Now,
            User = user,
        });

        _dataContext.SaveChanges();

        return Ok();
    }

    [HttpDelete]
    [Route("delete/{shortUrl}")]
    public IActionResult Delete(string shortUrl)
    {
        var record = _dataContext.UrlPairs.FirstOrDefault(uriMap => uriMap.ShortUrl == shortUrl);
        if (record == null)
        {
            return NotFound("The URL was not found in the system.");
        }

        _dataContext.UrlPairs.Remove(record);
        _dataContext.SaveChanges();

        return Ok("URL was deleted.");
    }
}
