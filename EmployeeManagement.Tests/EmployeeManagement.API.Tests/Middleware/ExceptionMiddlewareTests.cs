//using Microsoft.AspNetCore.Http;

//namespace EmployeeManagement.API.Middleware.Tests;

//public class ExceptionMiddlewareTests
//{
//    [Fact]
//    public async Task Invoke_Should_Call_Next_When_No_Exception()
//    {
//        // Arrange
//        var context = new DefaultHttpContext();

//        var middleware = new ExceptionMiddleware(async (innerHttpContext) =>
//        {
//            await innerHttpContext.Response.WriteAsync("OK");
//        });

//        // Act
//        await middleware.Invoke(context);

//        // Assert
//        Assert.Equal(200, context.Response.StatusCode);
//    }

//    [Fact]
//    public async Task Invoke_Should_Return_500_When_Exception_Thrown()
//    {
//        // Arrange
//        var context = new DefaultHttpContext();
//        var responseBody = new MemoryStream();
//        context.Response.Body = responseBody;

//        var middleware = new ExceptionMiddleware((innerHttpContext) =>
//        {
//            throw new Exception("Something went wrong");
//        });

//        // Act
//        await middleware.Invoke(context);

//        // Assert
//        Assert.Equal(500, context.Response.StatusCode);

//        responseBody.Seek(0, SeekOrigin.Begin);
//        var reader = new StreamReader(responseBody);
//        var bodyText = await reader.ReadToEndAsync();

//        Assert.Equal("Something went wrong", bodyText);
//    }
//}