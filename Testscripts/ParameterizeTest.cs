using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using WebDriverManager.DriverConfigs.Impl;

namespace MyNunitProjectPractice
{
    public class ParameterizeTest
    {
        public ExtentReports extentReports;
        public ExtentTest extentTest;
        IWebDriver driver;

        [OneTimeSetUp]
        public void OnTimeSetupMethod()
        {
            extentReports = new ExtentReports();

            string reportPath = "C:\\Users\\Amandeep Saini\\source\\repos\\MyNunitProjectPractice\\TestResults\\" + $"Report_{DateTime.UtcNow.ToString("yyyyMMdd_hhMMss")}.html";
            ExtentSparkReporter extentSparkReporter = new ExtentSparkReporter(reportPath);

            extentReports.AttachReporter(extentSparkReporter);
            extentReports.AddSystemInfo("Env", "UAT01");
            extentReports.AddSystemInfo("User", "Amandeep Saini");
        }

        [SetUp]
        public void InitiateBrowser()
        {
            var browserName = ConfigurationManager.AppSettings.Get("BrowserName");

            if (string.IsNullOrEmpty(browserName))
            {
                throw new Exception("browserName is null");
            }
            InitiatebRowser(browserName);
            driver.Manage().Window.Maximize();
        }

        private void InitiatebRowser(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "chrome":
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver();
                    break;
                case "edge":
                    new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    driver = new ChromeDriver();
                    break;
            }
        }

        [Test, Category("Parameterization")]
        [TestCase("rahulshettyacademy", "learning")]
        [TestCase("Aman", "Pswd1")]
        public void A_TestParmeterize_UsingTestcase(string username, string password)
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            extentTest.CreateNode(username, password);
            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            driver.FindElement(By.CssSelector("#username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            extentTest.Log(Status.Pass, "Submite bug clicked");
        }

        [Test, Category("Parameterization")]
        [TestCaseSource(nameof(TestcaseSouceMethodWithTestCaseData))]
        public void B_TestParmeterize_UsingTestDataSource(string username, string password)
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            extentTest.CreateNode(username, password);
            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            driver.FindElement(By.CssSelector("#username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            extentTest.Log(Status.Pass, "Submitted button clicked");
        }

        private static IEnumerable<TestCaseData> TestcaseSouceMethodWithTestCaseData()
        {
            yield return new TestCaseData("rahulshettyacademy", "learning");
            yield return new TestCaseData("Aman2", "Pswd2");
            yield return new TestCaseData("akshay3", "pswd3");
        }

        [TearDown]
        public void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var message = TestContext.CurrentContext.Result.Message;
            if (testStatus == TestStatus.Failed)
            {
                extentTest.Fail("Test outcome is Fail.", TakeScreenshot(driver));
                extentTest.Log(Status.Fail, "Log stackTrace = " + stackTrace, TakeScreenshot(driver));
                extentTest.Log(Status.Fail, "Log message = " + message, TakeScreenshot(driver));
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status.ToString().ToLower().Equals("inconclusive"))
            {
                extentTest.Log(Status.Info, "Log-Test outcome is inconclusive.", TakeScreenshot(driver));
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Passed))
            {
                extentTest.Log(Status.Pass, "Log-Test outcome is pass.", TakeScreenshot(driver));
            }

            driver.Quit();

            extentReports.Flush();
        }

        private Media TakeScreenshot(IWebDriver driver)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            string scrAsBas64Str = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(scrAsBas64Str, $"Screenshot_{DateTime.UtcNow.ToString("yyyyMMdd_hhMMss")}.png").Build();
        }
    }
}