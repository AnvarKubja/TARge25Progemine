using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TARge25Shop.SeleniumTesting
{
    public class SpaceshipFrontendTests
    {
        [Fact]
        public void Should_NavigateToCreate_AddSpaceshipWithCorrectData()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://localhost:7062/";
            IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Spaceship"));
            navigateToSpaceship.Click();
            IWebElement createInIndex = driver.FindElement(By.Id("SpaceshipNavigate"));
            createInIndex.Click();
        }
    }
}
