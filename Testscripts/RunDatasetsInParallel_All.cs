using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace MyNunitProjectPractice
{
    [Parallelizable(ParallelScope.All)]
    public class RunDatasetsInParallel_All : BaseTest
    {
        [Test, Category("Parallel threads - dataSet1 - ParallelScope.All")]
        [TestCase("rahulshettyacademy", "learning")]
        [TestCase("Aman", "Pswd1")]
        //[Parallelizable(ParallelScope.All)]
        public void ParallelizeTest1(string username, string password)
        {
            driver.Value.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            driver.Value.FindElement(By.CssSelector("#username")).SendKeys(username);
            driver.Value.FindElement(By.Name("password")).SendKeys(password);
            driver.Value.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
            extentTest.Value.Log(Status.Pass, "Submite bug clicked");
        }

        [Test, Category("Parallel threads - dataSet2 - ParallelScope.All")]
        [TestCaseSource(nameof(TestcaseSouceMethodWithTestCaseData))]
        //[Parallelizable(ParallelScope.All)]
        public void ParallelizeTest2(string username, string password)
        {
            try
            {

                driver.Value.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
                driver.Value.FindElement(By.CssSelector("#username")).SendKeys(username);
                driver.Value.FindElement(By.Name("password")).SendKeys(password);
                driver.Value.FindElement(By.XPath("//*[@id='signInBtn']")).Click();
                extentTest.Value.Log(Status.Pass, "Submitted button clicked");
            }
            catch (Exception ex)
            {
                extentTest.Value.Log(Status.Fail, $"Test failed with exception: {ex.Message}");
                throw;
            }
        }

        private static IEnumerable<TestCaseData> TestcaseSouceMethodWithTestCaseData()
        {
            yield return new TestCaseData("rahulshettyacademy", "learning");
            yield return new TestCaseData("Aman2", "Pswd2");
            yield return new TestCaseData("akshay3", "pswd3");
        }
    }
}