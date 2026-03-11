using Moq;
using StackExchange.Redis;

namespace EmployeeManagement.Infrastructure.Cache.Tests;

public class RedisCacheServiceTests
{
    private readonly Mock<IConnectionMultiplexer> _redisMock;
    private readonly Mock<IDatabase> _databaseMock;
    private readonly RedisCacheService _service;

    public RedisCacheServiceTests()
    {
        _redisMock = new Mock<IConnectionMultiplexer>();
        _databaseMock = new Mock<IDatabase>();

        _redisMock
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_databaseMock.Object);

        _service = new RedisCacheService(_redisMock.Object);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreValueInRedis()
    {
        var key = "employee:1";
        var value = new EmployeeTestModel
        {
            Name = "Jason"
        };

        await _service.SetAsync(key, value);

        _databaseMock.Verify(
            x => x.StringSetAsync(
                key,
                It.IsAny<RedisValue>(),
                null,
                false,
                When.Always,
                CommandFlags.None),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDeserializedObject()
    {
        var key = "employee:1";
        var json = "{\"Name\":\"Jason\"}";

        _databaseMock
            .Setup(x => x.StringGetAsync(key, CommandFlags.None))
            .ReturnsAsync(json);

        var result = await _service.GetAsync<EmployeeTestModel>(key);

        Assert.NotNull(result);
        Assert.Equal("Jason", result!.Name);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        var key = "employee:1";

        _databaseMock
            .Setup(x => x.StringGetAsync(key, CommandFlags.None))
            .ReturnsAsync(RedisValue.Null);

        var result = await _service.GetAsync<EmployeeTestModel>(key);

        Assert.Null(result);
    }

    private class EmployeeTestModel
    {
        public string Name { get; set; } = string.Empty;
    }
}