using AventStack.ExtentReports;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace MyNunitProjectPractice
{
    public class FetchJsonData : BaseTest
    {
        [Test, Category("Json")]       
        public void a_TestFetchJsonData()
        {
            extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);

            driver.Value.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");

            //FetchJsonData
            string textFromJson = File.ReadAllText("C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestData\\testData.json");

            var useName = JToken.Parse(textFromJson).SelectToken("username").Value<string>();
            var paswd = JToken.Parse(textFromJson).SelectToken("password").Value<string>();
            var products = JToken.Parse(textFromJson).SelectTokens("products").Values<string>();

            extentTest.Value.Log(Status.Info, "Username fetched from json = " + useName);
            extentTest.Value.Log(Status.Info, "Password fetched from json = " + paswd);
            foreach (var product in products)
            {
                extentTest.Value.Log(Status.Info, "product fetched from json = " + product);
            }

            driver.Value.FindElement(By.CssSelector("#username")).SendKeys(useName);
            driver.Value.FindElement(By.Name("password")).SendKeys(paswd);
            driver.Value.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            extentTest.Value.Log(Status.Pass, "Submite bug clicked");
        }

        [Test, Category("Json")]
        public void b_TestFetchJsonDataUsingCommonMethod()
        {
            extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);

           var node1 = extentTest.Value.CreateNode("Node1 : Go to url");
            driver.Value.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            node1.Log(Status.Info, "Go to url.");

            var node2 = extentTest.Value.CreateNode("Node2 : I am fetching data from json.");
            var useName = ExtractDataFromJson("username");
            node2.Log(Status.Info, "Username fetched from json = " + useName);

            var paswd = ExtractDataFromJson("password");
            node2.Log(Status.Info, "Password fetched from json = " + paswd);

            //var product = ExtractDataFromJson("products"); 
            var products = ExtractDataFromJsonArray("products");
            foreach (var prod in products)
            {
                node2.Log(Status.Info, "product fetched from json = " + prod);
            }

            var node3 =extentTest.Value.CreateNode("Node3 : Enter fetched data in login page and click submit");
            driver.Value.FindElement(By.CssSelector("#username")).SendKeys(useName);
            driver.Value.FindElement(By.Name("password")).SendKeys(paswd);
            driver.Value.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            node3.Log(Status.Pass, "Submitted bug clicked");
        }
       
        //private static string ExtractDataFromJson(string tokenName)
        //{
        //    //FetchJsonData - return single value.
        //    string myJsonString = File.ReadAllText("C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestData\\testData.json");
        //    return JToken.Parse(myJsonString).SelectToken(tokenName).Value<string>();
        //}

        //private static string[] ExtractDataFromJsonArray(string tokenName)
        //{
        //    //FetchJsonData - return array value.
        //    string myJsonString = File.ReadAllText("C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestData\\testData.json");
        //    JToken jtokenObj = JToken.Parse(myJsonString);
        //    return jtokenObj.SelectTokens(tokenName).Values<string>().ToArray();
        //}

        [Test, Category("Json")]
        [TestCaseSource(nameof(TestCaseSource_method))]
        public void c_TestFetchJsonData_parametrized(string UsrName1, string Pswd1, string[] products)
        {
            extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            extentTest.Value.CreateNode("I am fetching data from json.");

            driver.Value.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");

            extentTest.Value.Log(Status.Info, "Username fetched from TestCaseSource_method = " + UsrName1);
            extentTest.Value.Log(Status.Info, "Password fetched from TestCaseSource_method = " + Pswd1);
            foreach (var product in products)
            {
                extentTest.Value.Log(Status.Info, "product fetched from TestCaseSource_method = " + product);
            }

            driver.Value.FindElement(By.CssSelector("#username")).SendKeys(UsrName1);
            driver.Value.FindElement(By.Name("password")).SendKeys(Pswd1);
            driver.Value.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            extentTest.Value.Log(Status.Pass, "Submite bug clicked");
        }

        private static IEnumerable<TestCaseData> TestCaseSource_method()
        {
            yield return new TestCaseData(ExtractDataFromJson("username"), ExtractDataFromJson("password"), ExtractDataFromJsonArray("products"));
            yield return new TestCaseData(ExtractDataFromJson("userwrong"), ExtractDataFromJson("paswdswrong"), ExtractDataFromJsonArray("products"));
        }
    }
}