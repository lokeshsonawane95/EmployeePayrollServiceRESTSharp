using Newtonsoft.Json;
using RestSharp;
using EmployeePayrollServiceRESTSharp;
using Newtonsoft.Json.Linq;

namespace EmployeePayrollServiceRESTSharpMSTest
{
    [TestClass]
    public class RestSharpTestCases
    {
        RestClient client;

        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGetApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id: " + employee.id + " Name: " + employee.name + " Salary: " + employee.salary);
            }

            Assert.AreEqual(5, dataResponse.Count);
        }

        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);

            JObject jObject = new JObject();
            jObject.Add("name", "Clark");
            jObject.Add("salary", "150000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("150000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }
    }
}