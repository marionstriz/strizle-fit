using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Measurement = App.Public.DTO.v1.Measurement;
using MeasurementType = App.Public.DTO.v1.MeasurementType;
using Unit = App.Public.DTO.v1.Unit;

namespace Tests.WebApp.ApiControllers;

public class IntegrationTestMeasurementApiController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    
    public IntegrationTestMeasurementApiController(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        _client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Fact]
    public async Task NotLoggedIn_CannotAccessMeasurementData()
    {
        var response = await _client.GetAsync("/api/v1/Measurement");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task LoggedInUser_CanAccessMeasurementData()
    {
        // Arrange
        var registerDto = new Register
        {
            Email = "test1@test.ee",
            Password = "Test1.test1",
            FirstName = "TestFirst",
            LastName = "TestLast"
        };

        var resultJwt = await GetRegisteredUserJwtAsync(registerDto);

        // Act
        var apiResponse = await GetGetRequestResponseAsync(new Uri("/api/v1/Measurement"), resultJwt);

        // Assert
        apiResponse.EnsureSuccessStatusCode();
        
        var content = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonDeserialize<List<Measurement>>(content);
        Assert.Empty(resultData!);
    }

    [Fact]
    public async Task LoggedInUser_CanAddMeasurementData()
    {
        // Arrange
        var registerDto = new Register
        {
            Email = "test2@test.ee",
            Password = "Test2.test1",
            FirstName = "Test2First",
            LastName = "Test2Last"
        };
        
        var resultJwt = await GetRegisteredUserJwtAsync(registerDto);
        var kgId = await GetKgUnitId(resultJwt);
        var weightId = await GetWeightMeasurementTypeId(resultJwt);
        var measurement = new Measurement
        {
            Value = 60,
            ValueUnitId = kgId.GetValueOrDefault(),
            MeasurementTypeId = weightId.GetValueOrDefault()
        };

        // Act
        var postApiResponse = await GetPostRequestResponseAsync(
            new Uri("/api/v1/Measurement"), 
            resultJwt, 
            JsonSerializer.Serialize(measurement));
        
        // Assert
        postApiResponse.EnsureSuccessStatusCode();
        
        var content = await postApiResponse.Content.ReadAsStringAsync();
        var resultData = JsonDeserialize<Measurement>(content);
        Assert.IsType<Measurement>(resultData);

        var getApiResponse = await GetGetRequestResponseAsync(new Uri("/api/v1/Measurement"), resultJwt);
        content = await getApiResponse.Content.ReadAsStringAsync();
        var getResultData = JsonDeserialize<List<Measurement>>(content);
        Assert.Single(getResultData!);
    }

    [Fact]
    public async Task UnauthorisedUser_CannotAccessOtherUserData()
    {
        // Arrange
        var registerDtoOne = new Register
        {
            Email = "test3@test.ee",
            Password = "Test3.test1",
            FirstName = "Test3First",
            LastName = "Test3Last"
        };
        
        var resultJwt = await GetRegisteredUserJwtAsync(registerDtoOne);
        var kgId = await GetKgUnitId(resultJwt);
        var weightId = await GetWeightMeasurementTypeId(resultJwt);
        var measurement = new Measurement
        {
            Value = 60,
            ValueUnitId = kgId.GetValueOrDefault(),
            MeasurementTypeId = weightId.GetValueOrDefault()
        };

        // Act
        var postApiResponse = await GetPostRequestResponseAsync(
            new Uri("/api/v1/Measurement"), 
            resultJwt, 
            JsonSerializer.Serialize(measurement));
        
        // Assert
        postApiResponse.EnsureSuccessStatusCode();
        
        var registerDtoTwo = new Register
        {
            Email = "test4@test.ee",
            Password = "Test4.test1",
            FirstName = "Test4First",
            LastName = "Test4Last"
        };
        
        var resultJwtTwo = await GetRegisteredUserJwtAsync(registerDtoTwo);

        var getApiResponse = await GetGetRequestResponseAsync(new Uri("/api/v1/Measurement"), resultJwtTwo);
        var content = await getApiResponse.Content.ReadAsStringAsync();
        var getResultData = JsonDeserialize<List<Measurement>>(content);
        Assert.Empty(getResultData!);
    }

    private async Task<JwtResponse?> GetRegisteredUserJwtAsync(Register registerDto)
    {
        var jsonStr = JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/identity/Account/Register/", data);

        _testOutputHelper.WriteLine(response.ToString());
        response.EnsureSuccessStatusCode();
        
        var requestContent = await response.Content.ReadAsStringAsync();
        
        return JsonDeserialize<JwtResponse>(requestContent);
    }

    private async Task<Guid?> GetKgUnitId(JwtResponse? jwt)
    {
        var jsonResponse = await GetGetRequestResponseAsync(new Uri("/api/v1/Unit"), jwt);
        var response = JsonDeserialize<List<Unit>>(await jsonResponse.Content.ReadAsStringAsync());

        var unit = response?.FirstOrDefault();

        return unit?.Id;
    }
    
    private async Task<Guid?> GetWeightMeasurementTypeId(JwtResponse? jwt)
    {
        var jsonResponse = await GetGetRequestResponseAsync(new Uri("/api/v1/MeasurementType"), jwt);
        var response = JsonDeserialize<List<MeasurementType>>(await jsonResponse.Content.ReadAsStringAsync());

        var measurementType = response!.FirstOrDefault();

        return measurementType?.Id;
    }
    
    private T? JsonDeserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, 
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
    }

    private async Task<HttpResponseMessage> GetGetRequestResponseAsync(Uri uri, JwtResponse? jwt)
    {
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Get;
        if (jwt != null)
        {
            apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Token);
        }

        apiRequest.RequestUri = uri;
        
        return await _client.SendAsync(apiRequest);
    }
    
    private async Task<HttpResponseMessage> GetPostRequestResponseAsync(Uri uri, JwtResponse jwt, string jsonContent)
    {
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Post;
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Token);
        apiRequest.RequestUri = uri;
        apiRequest.Content = new StringContent(
            jsonContent, 
            Encoding.UTF8,
            "application/json");
        
        return await _client.SendAsync(apiRequest);
    }
}