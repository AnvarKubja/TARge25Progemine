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
            //firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();
            //aadress millele draiver navigeerib
            driver.Url = "https://localhost:7062/";
            //lehelt otsitav element
            IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Spaceship"));
            //selle elemendiga tehtav tegevus
            navigateToSpaceship.Click();
            IWebElement createInIndex = driver.FindElement(By.Id("SpaceshipNavigate"));
            createInIndex.Click();

            //sisestatavad andmed
            InsertSpaceShipData(driver);

            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("cu_CreateSpaceship"));
            cu_CreateSpaceship.Click();

            Thread.Sleep(1000);

            IWebElement indexNameSpaceship = driver.FindElement(By.Id("indexNameSpaceship"));
            var spaceShipNameData = indexNameSpaceship.Text;

            IWebElement indexTypeSpaceship = driver.FindElement(By.Id("indexTypeSpaceship"));
            var spaceShipTypeData = indexTypeSpaceship.Text;

            IWebElement indexCrewSpaceship = driver.FindElement(By.Id("indexCrewSpaceship"));
            var spaceShipCrewData = indexCrewSpaceship.Text;

            //kontroll
            Assert.Equal(spaceShipNameData, "i add name for spaceship");
            Assert.True(spaceShipTypeData == "i add ship type for spaceship");
            Assert.Equal(spaceShipCrewData, "123456");
        }

        private void InsertSpaceShipData(IWebDriver driver)
        {
            IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
            cu_NameEntrySpaceship.Clear();
            cu_NameEntrySpaceship.SendKeys("i add name for spaceship");

            IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("cu_ShipTypeEntrySpaceship"));
            cu_ShipTypeEntrySpaceship.Clear();
            cu_ShipTypeEntrySpaceship.SendKeys("i add ship type for spaceship");

            IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("cu_CrewEntrySpaceship"));
            cu_CrewEntrySpaceship.Clear();
            cu_CrewEntrySpaceship.SendKeys("123456");

            IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("cu_EnginePowerEntrySpaceship"));
            cu_EnginePowerEntrySpaceship.Clear();
            cu_EnginePowerEntrySpaceship.SendKeys("3456");
        }
    }
}
