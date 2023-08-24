using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPIDemo.models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinimalAPIDemo.Test
{
  [TestClass]
  public class ApiTests
  {
    private HttpClient _httpClient;

    public ApiTests()
    {
      var webAppFactory = new WebApplicationFactory<Program>();
      _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task DefaultRoute_ReturnsHelloWorld()
    {
      var response = await _httpClient.GetAsync("/api/health");
      var stringResult = await response.Content.ReadAsStringAsync();

      Assert.AreEqual("running", stringResult);
    }

    [TestMethod]
    public async Task AddTodoTest()
    {
      var todo = new Todo
      {
        Age = 25,
        Name = "khalipha"
      };
      var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(todo);
      var requestContent = new StringContent(jsonData, Encoding.Unicode, "application/json");
      var response = await _httpClient.PostAsync("/api/todos", requestContent);
      var stringResult = await response.Content.ReadAsStringAsync();
      Console.WriteLine(stringResult);

      Assert.AreEqual(response.IsSuccessStatusCode, true);
    }

    // [TestMethod]
    // public async Task GetTodoTest()
    // {

    //   var response = await _httpClient.GetAsync("/api/todos");
    //   var stringResult = await response.Content.ReadAsStringAsync();

    //   Assert.AreEqual(stringResult, true);
    // }
  }
}