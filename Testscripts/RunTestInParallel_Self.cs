using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace MyNunitProjectPractice
{
    public class RunTestInParallel_Self : BaseTest
    {
        private By usernameByCssSelector { get { return By.CssSelector("#user-name"); } }
        private By passwordByXpath { get { return By.XPath("//*[@id='user-name']/parent::div/following-sibling::div/descendant::input[@type='password']"); } }
        private By loginByName { get { return By.Name("login-button"); } }
        private By productNamesByXpath
        {
            get
            {
                return By.XPath("//div[@class='inventory_list']/descendant::div[@class='inventory_item']/descendant::a/div");
            }
        }

        [Test, Category("Parallel threads - ParallelScope.Self")]
        [Parallelizable(ParallelScope.Self)]
        public void ParallelizeTest3()
        {
            //extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            driver.Value.Url = "https://www.saucedemo.com";
            Helper helper = new Helper(driver.Value);
            helper.SendKeys(usernameByCssSelector, "standard_user");
            helper.SendKeys(passwordByXpath, "secret_sauce");
            helper.ClickElement(loginByName);
            int itemscount = driver.Value.FindElements(By.XPath("//div[@class='inventory_list']/descendant::div[@class='inventory_item']")).Count;

            try
            {
                Assert.AreEqual(6, itemscount);
                extentTest.Value.Log(Status.Pass, "Items count =" + itemscount);
            }
            catch (AssertionException assException)
            {
                extentTest.Value.Log(Status.Fail, "Item count mismatch and excepton = " + assException);
            }
        }

        [Test, Category("Parallel threads - ParallelScope.Self")]
        [Parallelizable(ParallelScope.Self)]
        public void ParallelizeTest4()
        {
            //extentTest.Value = extentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            driver.Value.Url = "https://www.saucedemo.com";
            Helper helper = new Helper(driver.Value);
            helper.SendKeys(usernameByCssSelector, "standard_user");
            helper.SendKeys(passwordByXpath, "secret_sauce");
            helper.ClickElement(loginByName);
            IList<IWebElement> ee = driver.Value.FindElements(productNamesByXpath);
            foreach (IWebElement e in ee)
            {
                extentTest.Value.Log(Status.Pass, "product names=" + e.Text);
            }
        }
    }
}