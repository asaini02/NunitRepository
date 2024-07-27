using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using WebDriverManager.DriverConfigs.Impl;

namespace MyNunitProjectPractice
{
    public class BaseTest
    {
        public ExtentReports extentReports;
        //public ExtentTest extentTest;
        //public IWebDriver driver;

        //Make these theadLocal
        public ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        //public ThreadLocal<ExtentReports> extentReports = new ThreadLocal<ExtentReports>();
        public ThreadLocal<ExtentTest> extentTest = new ThreadLocal<ExtentTest>();

        [OneTimeSetUp]
        public void OnTimeSetupMethod()
        {
            //Main class
            extentReports = new ExtentReports();

            //Report path info pass to ExtentSparkerReporter
            string reportPath = "C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestResults\\" + $"Report_{DateTime.UtcNow.ToString("yyyyMMdd_hhMMss")}.html";
            ExtentSparkReporter extentSparkReporter = new ExtentSparkReporter(reportPath);

            //attach reporter in ExtentReport
            extentReports.AttachReporter(extentSparkReporter);
            extentReports.AddSystemInfo("Env", "UAT01");
            extentReports.AddSystemInfo("User", "Amandeep Saini");
        }

        [SetUp]
        public void InitiateBrowser()
        {
            extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name, "Execution started from base test");
            string browserName = null;
            //if (browserName != null)
            //{
            //    browserName = TestContext.Parameters["browserName"];
            //}
            //else
            //{
            browserName = ConfigurationManager.AppSettings.Get("BrowserName");
            //}


            if (string.IsNullOrEmpty(browserName))
            {
                throw new Exception("browserName is null");
            }
            Initiatebrowser(browserName);
            driver.Value.Manage().Window.Maximize();
            driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        private void Initiatebrowser(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "chrome":
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    driver.Value = new ChromeDriver();
                    break;
                case "edge":
                    new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    driver.Value = new ChromeDriver();
                    break;
                case "firefox":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    driver.Value = new ChromeDriver();
                    break;
            }
        }

        [TearDown]
        public void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var message = TestContext.CurrentContext.Result.Message;
            if (testStatus == TestStatus.Failed)
            {
                extentTest.Value.Fail("Test outcome is Fail.", TakeScreenshot(driver.Value));
                extentTest.Value.Log(Status.Fail, "Log stackTrace = " + stackTrace, TakeScreenshot(driver.Value));
                extentTest.Value.Log(Status.Fail, "Log message = " + message, TakeScreenshot(driver.Value));
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status.ToString().ToLower().Equals("inconclusive"))
            {
                extentTest.Value.Log(Status.Info, "Log-Test outcome is inconclusive.", TakeScreenshot(driver.Value));
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Passed))
            {
                extentTest.Value.Log(Status.Pass, " [Tear down] Log-Test outcome is pass.", TakeScreenshot(driver.Value));
            }

            driver.Value.Quit();
            try
            {
                extentReports.Flush();  // Finalize and save the report
            }
            catch (IOException ex)
            {
                extentTest.Value.Log(Status.Error, $"Error flushing report: {ex.Message}");
            }


        }

        private Media TakeScreenshot(IWebDriver driver)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            string scrAsBas64Str = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(scrAsBas64Str, $"Screenshot_{DateTime.UtcNow.ToString("yyyyMMdd_hhMMss")}.png").Build();
        }

        public static string ExtractDataFromJson(string tokenName)
        {
            //FetchJsonData - return single value.
            string myJsonString = File.ReadAllText("C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestData\\testData.json");
            return JToken.Parse(myJsonString).SelectToken(tokenName).Value<string>();
        }

        public static string[] ExtractDataFromJsonArray(string tokenName)
        {
            //FetchJsonData - return array value.
            string myJsonString = File.ReadAllText("C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestData\\testData.json");
            JToken jtokenObj = JToken.Parse(myJsonString);
            return jtokenObj.SelectTokens(tokenName).Values<string>().ToArray();
        }
    }
}