using OpenQA.Selenium;
using System.Drawing.Text;

namespace MyNunitProjectPractice
{
    public class WebTableTest : BaseTest
    {
        [Test,Category("Parallel threads")]
        public void FetchTableContent()
        {
            driver.Value.Url = "https://cosmocode.io/automation-practice-webtable/";
            FetchLanguage("India", driver.Value);

        }
        private string FetchLanguage(string ExpectedCountry, IWebDriver driver)
        {
            string lang = null;

            //Fetch  column values of [country]
            IList<IWebElement> countries = driver.FindElements(By.XPath("//table[@id='countries']/descendant::tr/td[2]"));

            //find country
            foreach (IWebElement country in countries)
            {
                if (country.Text.Equals(ExpectedCountry))
                {
                    // go to country immediate <tr> and get data from desendent [expected td]

                    lang = country.FindElement(By.XPath("./parent::tr/descendant::td[5]")).Text;
                    break;
                }
            }
            return lang;
        }
    }
}