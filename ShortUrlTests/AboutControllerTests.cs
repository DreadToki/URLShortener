namespace ShortUrlTests;

[TestFixture]
public class AboutControllerTests
{
    [Test]
    public void Read_TextExists_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        AboutController controller = new(context);

        var result = controller.Read();

        Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
    }

    [Test]
    public void Create_RewriteText_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        AboutDto aboutRequest = new()
        {
            Text = "Confidence is built by facing what you fear.",
        };

        AboutController controller = new(context);

        var result = controller.Create(aboutRequest);

        Assert.That(result, Is.InstanceOf(typeof(OkResult)));
    }
}
