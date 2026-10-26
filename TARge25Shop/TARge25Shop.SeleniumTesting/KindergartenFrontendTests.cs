using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TARge25Shop.SeleniumTesting
{
    [TestCaseOrderer("TARge25Shop.SeleniumTesting.PriorityOrderer", "TARge25Shop.SeleniumTesting")]
    public class KindergartenFrontendTests : IDisposable
    {
        private const string BaseUrl = "https://localhost:7062/";
        private readonly IWebDriver driver;
        private static string thisRunGroupName;

        public KindergartenFrontendTests()
        {
            var options = new FirefoxOptions();
            driver = new FirefoxDriver(options);
        }

        public void Dispose()
        {
            driver.Quit();
        }

        /// <summary>
        /// Kontrollib uue lasteaia edukat loomist ja sisestatud andmete korrektset kuvamist tabelis
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_CreateKindergarten_AndShowItInIndex()
        {
            OpenKindergartenIndex();
            driver.FindElement(By.Id("CreateInIndex")).Click();

            thisRunGroupName = Guid.NewGuid().ToString();
            FillForm(thisRunGroupName, "20", "Asdasdasd", "Pets");
            driver.FindElement(By.Id("CU_CreateKindergarten")).Click();

            IWebElement row = FindRow(thisRunGroupName);

            Assert.NotNull(row);
            Assert.Equal("20", row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text);
            Assert.Equal("Asdasdasd", row.FindElement(By.Id("IndexKindergartenNameKindergarten")).Text);
            Assert.Equal("Pets", row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text);
        }

        /// <summary>
        /// Kontrollib, kas tabelist detailide vaatesse liikumisel kuvatakse eelnevalt loodud lasteaia andmed korrektselt
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_ShowDetails_OfCreatedKindergarten()
        {
            OpenKindergartenIndex();
            IWebElement row = FindRow(thisRunGroupName);
            Assert.NotNull(row);

            row.FindElement(By.Id("IndexKindergartenDetails")).Click();

            Assert.False(string.IsNullOrEmpty(driver.FindElement(By.Id("Details_IdKindergarten")).Text));
            Assert.Equal(thisRunGroupName, driver.FindElement(By.Id("Details_GroupNameKindergarten")).Text);
            Assert.Equal("20", driver.FindElement(By.Id("Details_ChildrenCountKindergarten")).Text);
            Assert.Equal("Asdasdasd", driver.FindElement(By.Id("Details_KindergartenNameKindergarten")).Text);
            Assert.Equal("Pets", driver.FindElement(By.Id("Details_TeacherNameKindergarten")).Text);
        }

        /// <summary>
        /// Kontrollib lasteaia andmete muutmist ja muudetud andmete korrektset uuendamist tabelis
        /// </summary>
        [Fact]
        public void Should_UpdateKindergarten_AndShowNewDataInIndex()
        {
            OpenKindergartenIndex();
            IWebElement row = FindRow(thisRunGroupName);
            Assert.NotNull(row);

            row.FindElement(By.Id("IndexKindergartenUpdate")).Click();

            // grupi jääb samaks, et objekt oleks tabelis ikka leitav
            FillForm(thisRunGroupName, "25", "Bcdbcd", "Rita");
            driver.FindElement(By.Id("CU_UpdateKindergarten")).Click();

            row = FindRow(thisRunGroupName);

            Assert.NotNull(row);
            Assert.Equal("25", row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text);
            Assert.Equal("Bcdbcd", row.FindElement(By.Id("IndexKindergartenNameKindergarten")).Text);
            Assert.Equal("Rita", row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text);
        }

        /// <summary>
        /// Kontrollib vastava lasteaia kustutamist
        /// </summary>
        [Fact]
        public void Should_DeleteKindergarten_ViaDeleteView()
        {
            OpenKindergartenIndex();
            IWebElement row = FindRow(thisRunGroupName);
            Assert.NotNull(row);

            row.FindElement(By.Id("IndexKindergartenDelete")).Click();
            driver.FindElement(By.Id("Delete_Delete")).Click();

            Assert.Null(FindRow(thisRunGroupName));
        }

        // 4 LISATESTI

        /// <summary>
        /// Delete lehel "Cancel" ei tohi objekti kustutada
        /// </summary>
        [Fact]
        public void Should_NotDeleteKindergarten_WhenCancelIsClickedInDeleteView()
        {
            OpenKindergartenIndex();
            IWebElement row = FindRow(thisRunGroupName);
            Assert.NotNull(row);

            row.FindElement(By.Id("IndexKindergartenDelete")).Click();

            Assert.Equal(thisRunGroupName, driver.FindElement(By.Id("Delete_GroupNameKindergarten")).Text);

            driver.FindElement(By.Id("Delete_Cancel")).Click();

            Assert.NotNull(FindRow(thisRunGroupName));
        }

        /// <summary>
        /// Create lehel "Back to List" ei tohi uut objekti salvestada
        /// </summary>
        [Fact]
        public void Should_NotAddKindergarten_WhenBackToListIsClickedInCreateView()
        {
            OpenKindergartenIndex();
            int rowsBefore = CountRows();

            driver.FindElement(By.Id("CreateInIndex")).Click();
            string notSavedName = Guid.NewGuid().ToString();
            FillForm(notSavedName, "3", "Ghahaha", "Katse");
            driver.FindElement(By.Id("CU_BackKindergarten")).Click();

            Assert.Equal(rowsBefore, CountRows());
            Assert.Null(FindRow(notSavedName));
        }

        /// <summary>
        /// Pärast muutmist peab "Updated at" olema erinev kui "Created at" ja Details lehel peavad olema uued andmed
        /// </summary>
        [Fact]
        public void Should_ShowNewDataAndDifferentUpdatedAt_InDetails_AfterUpdate()
        {
            OpenKindergartenIndex();
            IWebElement row = FindRow(thisRunGroupName);
            Assert.NotNull(row);

            row.FindElement(By.Id("IndexKindergartenDetails")).Click();

            string createdAt = driver.FindElement(By.Id("Details_CreatedAtKindergarten")).Text;
            string updatedAt = driver.FindElement(By.Id("Details_UpdatedAtKindergarten")).Text;

            Assert.Equal("25", driver.FindElement(By.Id("Details_ChildrenCountKindergarten")).Text);
            Assert.Equal("Bcdbcd", driver.FindElement(By.Id("Details_KindergartenNameKindergarten")).Text);
            Assert.Equal("Rita", driver.FindElement(By.Id("Details_TeacherNameKindergarten")).Text);
            Assert.NotEqual(createdAt, updatedAt);
        }

        
        /// <summary>
        /// Kontrollib kas negatiivne laste arv salvestatakse nullina
        /// </summary>
        [Fact]
        public void Should_SaveNegativeChildrenCountAsZero()
        {
            OpenKindergartenIndex();
            driver.FindElement(By.Id("CreateInIndex")).Click();

            string name = Guid.NewGuid().ToString();
            FillForm(name, "-5", "Negatiivne", "Test");
            driver.FindElement(By.Id("CU_CreateKindergarten")).Click();

            IWebElement row = FindRow(name);
            Assert.NotNull(row);
            string childrenCount = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;

            row.FindElement(By.Id("IndexKindergartenDelete")).Click();
            driver.FindElement(By.Id("Delete_Delete")).Click();

            Assert.Equal("0", childrenCount);
            Assert.Null(FindRow(name));
        }

        // Abistavad meetodid
        private void OpenKindergartenIndex()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            driver.FindElement(By.Id("KindergartenNavigate")).Click();
        }

        private IWebElement FindRow(string groupName)
        {
            var rows = driver.FindElement(By.Id("IndexTable")).FindElements(By.TagName("tr"));

            foreach (var row in rows)
            {
                if (row.FindElement(By.Id("IndexGroupNameKindergarten")).Text == groupName)
                {
                    return row;
                }
            }

            return null;
        }

        private void FillForm(string groupName, string childrenCount, string kindergartenName, string teacherName)
        {
            FillField("CU_GroupNameKindergarten", groupName);
            FillField("CU_ChildrenCountKindergarten", childrenCount);
            FillField("CU_KindergartenNameKindergarten", kindergartenName);
            FillField("CU_TeacherNameKindergarten", teacherName);
        }

        private void FillField(string id, string value)
        {
            IWebElement field = driver.FindElement(By.Id(id));
            field.Clear();
            field.SendKeys(value);
        }

        private int CountRows()
        {
            var table = driver.FindElement(By.Id("IndexTable"));
            return table.FindElements(By.TagName("tr")).Count;
        }
    }
}
