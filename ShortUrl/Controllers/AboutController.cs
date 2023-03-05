using Microsoft.AspNetCore.Mvc;
using ShortUrl.Dtos;
using ShortUrl.Models;

namespace ShortUrl.Controllers;

[ApiController]
[Route("[controller]")]
public class AboutController : Controller
{
    private readonly DataContext _dataContext;

    public AboutController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    [Route("read")]
    public IActionResult Read()
    {
        var record = _dataContext.About.FirstOrDefault();
        AboutDto dto = new()
        {
            Text = record != null ? record.Text : string.Empty,
        };

        return Ok(dto);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult Create(AboutDto dto)
    {
        var record = _dataContext.About.FirstOrDefault();
        if (record == null)
        {
            record = new About
            {
                Text = dto.Text,
            };
            _dataContext.About.Add(record);
        }
        else
        {
            record.Text = dto.Text;
        }

        _dataContext.SaveChanges();
        return Ok();
    }
}
