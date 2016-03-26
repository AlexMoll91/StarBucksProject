using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using GemBox.Spreadsheet;
using JQSelenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
namespace NUnitTestProject1
{
    [TestFixture]
    public class Tests
    {
        private const string baseUrl = "https://www.starbucks.com/account/signin";
        private const string userName = "drugdiscounts@gmail.com";
        private const string userPass = "Temppass2!";

        /// <summary>
        ///     reload.txt setup is cardname:amounttoreload i.e CVS 032423:20
        /// </summary>
        [Test]
        public void ReloadCardsAutomation()
        {
            //Setting up driver
            var driver = new ChromeDriver();


            //Initialize dictionary -- KEY (Card Name) Value (Url addition)
            File.Create(@"C:\Users\Public\TestFolder\reloadresults" + DateTime.Today.ToString("h_mm tt MM-dd-yy") +
                        ".txt");
            var sw =
                new StreamWriter(@"C:\Users\Public\TestFolder\reloadresults" +
                                 DateTime.Today.ToString("h_mm tt MM-dd-yy") +
                                 ".txt");
            var reloadAmountList = new List<string>();
            var cardNameList = new List<string>();
            var cardUrlDict = Data.CreateCarDictionary();

            var reloadDict = File.ReadAllLines("reload.txt")
                .Select(x => x.Split(':'));

            foreach (var reload in reloadDict)
            {
                cardNameList.Add(reload[0]);
                reloadAmountList.Add(reload[1]);
            }

            //Login
            LogIn(driver);

            //Sign In Button
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

       
  
            for (var z = 0; z == cardNameList.Count; z++)
            {
                driver.Navigate()
                    .GoToUrl("https://www.starbucks.com/account/card/reload/choose/" + cardUrlDict[cardNameList[z]]);
                var jqf = new JQuery(driver);
                jqf.Find("#Predefined_Reload_Amount > p").Click();
                driver.FindElement(By.Id("reload_amount")).Clear();
                driver.FindElement(By.Id("reload_amount")).SendKeys("20");
                driver.FindElement(By.Id("reload_submit")).Click();
                sw.WriteLine(cardNameList[z] + " successfully loaded with " + reloadAmountList[z] + "$!");
            }
        }

        [Test]
        public void CheckBalances()

        {

            ExcelWorksheet ws;
            DataTable dataTable;

            //Grab list of cards
            var cardUrlDict = Data.CreateCarDictionary();

            //Initialize Streamwriter
            var sw =
                new StreamWriter(@"C:\" + DateTime.Today.ToString("h_mm tt MM-dd-yy") +
                                 "cardamounts.txt");
           
            //Setup Excel Sheet and DataTable
            var ef = Data.CreateDataTable(out ws, out dataTable);

            //Setting up driver
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            //Login
            LogIn(driver);

            //set iterator for loop
            var z = 0;
            //Visit card page and extract data
            foreach (var entry in cardUrlDict)
            {
                //Get Card URL
                driver.Navigate()
                    .GoToUrl("https://www.starbucks.com/account/card?cardId=" + entry.Value);
                //if CSC 
                if (driver.PageSource.Contains("The Card Security Code (CSC) is required to check this card’s balance."))
                {
                    //Create Row and place in DT
                    var row = dataTable.NewRow();
                    row["Card Name"] = "Needs CSC";
                    row["Card Amount"] = entry.Key;
                    sw.WriteLine(entry.Key + " : needs CSC");
                    dataTable.Rows.Add(row);
                }
                else
                {
                    //Grab Amount on Card & name on Card
                    var amount = driver.FindElement(By.XPath("//div[@class='balance-amount numbers']")).Text;
                    var name = driver.FindElement(By.XPath("//span[@class='nickname']")).Text;
                    //Write to DataTable
                    var row = dataTable.NewRow();
                    row["Card Name"] = amount;
                    row["Card Amount"] = name;
                    dataTable.Rows.Add(row);

                    //Write to Text
                    sw.WriteLine(name + " : " + amount);

                    //Increment
                    z++;
                }
            }
            //Close text file and create Excel
            sw.Close();
            ws.InsertDataTable(dataTable,
                new InsertDataTableOptions("A1") {ColumnHeaders = true});
            ef.Save(@"C:\" + DateTime.Now.ToString("h_mm tt MM-dd-yy") + "CardAmounts.xls");
           
        }

        private static void LogIn(ChromeDriver driver)
        {
            //Logging in initially
            driver.Navigate().GoToUrl(baseUrl);
            //Enter Email
            driver.FindElement(By.XPath("//input[@placeholder='Username or email']")).SendKeys(userName);
            //Enter Pass
            driver.FindElement(By.XPath("//input[@placeholder='Password']")).SendKeys(userPass);
            //Sign In Button
            driver.FindElement(By.Id("AT_SignIn_Button")).Click();
            //Iteration Counter
        }

       

        /// <summary>
        ///     transfer.txt setup is cardnametransferfrom:cardnametransferto:amount
        /// </summary>
        [Test]
        public void TransferBalance()
        {
            //Grab list of cards
            var cardUrlDict = Data.CreateCarDictionary();
            //grab transfers to do
            var transferDict = File.ReadAllLines("transfer.txt")
                .Select(x => x.Split(':'));
            //Input Data Lists
            List<string> cardTransferTo;
            List<string> cardTransferAmount;
            List<string> cardTransferFrom = Data.GetTransferInfo(transferDict, out cardTransferTo, out cardTransferAmount);
            //Setting up driver
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            //Login
            LogIn(driver);

            //initiate sw
            var sw =
                new StreamWriter(@"C:\" + DateTime.Today.ToString("h_mm tt MM-dd-yy") +
                                 "cardtransfers.txt");
            //Go to card transfer url
            for (var z = 0; z == cardTransferFrom.Count; z++)
            {
                //Navigate card we will be transferring money from
                driver.Navigate()
                    .GoToUrl("https://www.starbucks.com/account/card/transfer/" 
                    + cardUrlDict[cardTransferFrom[z]]);

                //Click Transfer From Button
                driver.FindElement(By.Id("TransferFrom")).Click();

                //Setup dropdown and select card to transfer to
                var carddL = driver.FindElement(By.Id("TransferFrom_ExistingCardId"));
                var ddl = new SelectElement(carddL);
                ddl.SelectByValue(cardUrlDict[cardTransferTo[z]]);

                //Click Contingue
                driver.FindElement(By.XPath("//button[contains(.,'Continue')]")).Click();

                //Enter Transfer Amount
                driver.FindElement(By.Id("TransferFrom_TransferAmount")).SendKeys(cardTransferAmount[z]);

                //Click Preview Transfer
                driver.FindElement(By.XPath("//button[contains(.,'Preview My Transfer')]")).Click();

                //Click Complete
                driver.FindElement(By.XPath("//button[contains(.,'Complete My Transfer')]"));

                //Assert Success
                Assert.AreEqual(driver.PageSource.Contains("Transfer Complete"), true);

                //Write Results
                sw.WriteLine("Transferred from: " + cardTransferFrom[z] + " to " + cardTransferTo[z]);
            }
            sw.Close();
        }

        
    }
}

