using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNunitProjectPractice
{
    public class Helper : BaseTest
    {
        public Helper(IWebDriver driver)
        {
            this.driver.Value = driver;
        }

        public void ClickElement(By locator)
        {
            driver.Value.FindElement(locator).Click();
        }
        public void SendKeys(By locator, string text)
        {
            driver.Value.FindElement(locator).SendKeys(text);
        }
        public void ExplicitWait(string expectedUrl)
        {
            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe(expectedUrl));
        }
    }
}
