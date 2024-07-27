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
    public class UnitTestClass1
    {
        public ExtentReports extentReports;
        public ExtentTest extentTest;
        IWebDriver driver;

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
            //new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            //this.driver = new ChromeDriver();

            var browserName = ConfigurationManager.AppSettings.Get("BrowserName");

            if (string.IsNullOrEmpty(browserName))
            {
                throw new Exception("browserName is null");

            }
            InitiatebRowser(browserName);
            driver.Manage().Window.Maximize();
        }

        [Test, Category("basic")]
        public void TestTitlePass()
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            driver.Url = "https://www.google.com/";
            string expectedTitle = "Google";
            Assert.IsTrue(driver.Title.Equals(expectedTitle));
        }

        [Test,Category("basic")]
        public void TestTitlePass_LogAssertion()
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            driver.Url = "https://www.google.com/";
            string expectedTitle = "Google";
            try
            {
                Assert.IsTrue(driver.Title.Equals(expectedTitle));
                extentTest.Log(Status.Pass, "Title matches");
            }
            catch (AssertionException ex)
            {
                extentTest.Log(Status.Fail, $"Assertion failed: Expected title was '{expectedTitle}'," +
                    $" but actual title was '{driver.Title}'. Exception: {ex.Message}");
            }
        }


        [Test, Category("basic")]
        public void TestTitleFail()
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            driver.Url = "https://cosmocode.io/automation-practice-webtable/";
            Assert.IsTrue(driver.Title.Equals("WrongTitle"));
        }

        [TearDown]
        public void TearDown()
        {
            //NUnit.Framework.TestContext.CurrentContext.Result.Outcome.Status
            //TestStatus testStatus = TestContext.CurrentContext.Result.Outcome.Status;
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

            //close browser            
            driver.Quit();

            // save report
            extentReports.Flush();
        }

        private Media TakeScreenshot(IWebDriver driver)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            string scrAsBas64Str = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(scrAsBas64Str, $"Screenshot_{DateTime.UtcNow.ToString("yyyyMMdd_hhMMss")}.png").Build();
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
    }
}