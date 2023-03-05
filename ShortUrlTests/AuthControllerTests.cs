namespace ShortUrlTests;

[TestFixture]
public class AuthControllerTests
{
    [Test]
    public void Register_ValidCredentials_ReturnsOk()
    {
        var context = ContextGenerator.Generate();

        CredentialsDto credits = new()
        {
            Login = "NotRegisteredUser",
            Password = "password",
        };

        AuthController controller = new(context, new JwtService());

        var result = controller.Register(credits);

        Assert.That(result, Is.InstanceOf(typeof(OkResult)));
    }

    [Test]
    public void Register_NotValidCredentials_ReturnsNotFound()
    {
        var context = ContextGenerator.Generate();

        User user = new()
        {
            Login = "DefaultUser",
            Password = "password",
            Role = UserRole.User,
        };

        context.Add(user);
        context.SaveChanges();

        CredentialsDto credits = new()
        {
            Login = "DefaultUser",
            Password = "password",
        };

        AuthController controller = new(context, new JwtService());

        var result = controller.Register(credits);

        Assert.That(result, Is.InstanceOf(typeof(BadRequestObjectResult)));
    }
}
