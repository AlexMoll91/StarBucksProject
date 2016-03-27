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
namespace StarbucksAutomation
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
           
            

                var reloadAmountList = new List<string>();
            var cardNameList = new List<string>();
            var cardUrlDict = CreateCarDictionary();

            var reloadDict = File.ReadAllLines(@"C:\StarBucksAutomation\StarBucksProject\StarbucksAutomation\Input\reload.txt").Select(x => x.Split(':')).ToList();
           
            foreach (var reload in reloadDict)
            {
                cardNameList.Add(reload[0]);
                reloadAmountList.Add(reload[1]);
            }
            Console.WriteLine(cardNameList.Count);
            //Login
            LogIn(driver);
            //using (StreamWriter newTask = new StreamWriter(@"C:\Users\btmt0_000\Desktop\Balances\ReloadResults.txt", false))
            using (StreamWriter newTask = new StreamWriter(@"C:\ReloadResults.txt", false))
            {
               

                //Sign In Button
                var z = 0;
                foreach (var card in cardNameList)
                {
                    driver.Navigate()
                        .GoToUrl("https://www.starbucks.com/account/card/reload/choose/" + cardUrlDict[cardNameList[z]]);
                    var jqf = new JQuery(driver);
                    jqf.Find("#Predefined_Reload_Amount > p").Click();
                    driver.FindElement(By.Id("reload_amount")).Clear();
                    driver.FindElement(By.Id("reload_amount")).SendKeys(reloadAmountList[z]);
                    driver.FindElement(By.Id("reload_submit")).Click();
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    wait.Until(d => driver.PageSource.Contains("Your balance has been updated to reflect the new amount."));
                    newTask.WriteLine(cardNameList[z] + " succesfully loaded with "+reloadAmountList[z]);
                    z++;
                }
            }
        }
        private static Dictionary<string, string> CreateCarDictionary()
        {
            var d = new Dictionary<string, string>
                {
                    {"Master (1126)", "836E72FE9ED71DA3"},
                    {"My Card (6225)", "8C6476FD9FD11BAA"},
                    {"CVS #08330", "836274FD9BD611AB"},
                    {"CVS #06818", "836071F09ED31BAA"},
                    {"CVS #06741", "836071F09CD210A2"},
                    {"CVS #08384", "836E72FD99D91FA2"},
                    {"CVS #08322", "836E72FD9ED11BAF"},
                    {"CVS #06096", "836E72FD9DD11FA3"},
                    {"CVS #05512", "836E71FF9CD41BAF"},
                    {"CVS #08990", "836E71FF9DD918AA"},
                    {"MASTER Card #2", "8C6773F092D211A9"},
                    {"CVS #01339", "8C6772FA92D71EAF"},
                    {"CVS #03178", "8C6772FA92D91AAC"},
                    {"CVS #07805", "8C6770FF9BD11EA3"},
                    {"CVS #05804", "8C6770FF9BD910AB"},
                    {"CVS #05776", "8C6770FF98D41AAF"},
                    {"CVS #07827", "8C6770FF98D41EA8"},
                    {"CVS #06896", "8C6770FF98D518A3"},
                    {"CVS #10487", "8C6770FF98D51FAB"},
                    {"CVS #10821", "8C6770FF99D11BAF"},
                    {"CVS #03177", "8C6777FF9BD619AB"},
                    {"CVS", "8C6777FF9BD91EAC"},
                    {"CVS #06773", "8C6775FB9ED618AD"},
                    {"CVS #06238", "8C677AFC93D81DAF"},
                    {"CVS #03156", "8C6672F09CD31AAE"},
                    {"CVS #07765", "85637AF193D018A891"},
                    {"Master Card #4", "85637AF193D01BA891"},
                    {"Master Card #5", "85637AF193D01AAA91"},
                    {"CVS #07277", "856273F098D118AC90"},
                    {"CVS #06006", "856273F098D11AAA90"},
                    {"CVS #06233", "856273F098D11CA990"},
                    {"CVS #07291", "856272FF99D51BAA9A"},
                    {"CVS #07811", "856272FF99D610AF9D"},
                    {"Master Card #7", "856272FF99D818AF9D"},
                    {"CVS #08312", "856277FB99D51EA29C"},
                    {"CVS #07435", "856277FB99D511A391"},
                    {"CVS #06010", "856277FB99D619A29E"},
                    {"CVS #05959", "856277FB99D619A29F"},
                    {"CVS #06165", "856277FB99D61AA299"},
                    {"CVS #06222", "856277FB99D61CAB99"},
                    {"CVS #05323", "856276FA9BD211A99C"},
                    {"CVS #05544", "856276FA9BD319AC9B"},
                    {"CVS #03114", "856071FB9FD81EA299"},
                    {"CVS #03658", "856071FB9CD11AAC98"},
                    {"CVS #03923", "856071FB9CD110A29A"},
                    {"CVS #03118", "856071FB9CD21EAD9E"}
                };
            return d;
        }
        [Test]
        public void CheckBalances()

        {

            ExcelWorksheet ws;
            DataTable dataTable;

            //Grab list of cards
            var cardUrlDict = CreateCarDictionary();

            
                
           
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
                   /* using (StreamWriter sw = File.AppendText(@"C:\" + DateTime.Now.ToString("dd-M-yy_HH-mm-ss") +
                                  "cardamounts.txt"))
                    {
                        sw.WriteLine(name + " : " + amount);
                    }
                  */  //Increment
                    z++;
                }
            }
            //Close text file and create Excel

            ws.InsertDataTable(dataTable,
                new InsertDataTableOptions("A1") {ColumnHeaders = true});
            if (File.Exists(@"C:\Users\btmt0_000\Desktop\Balances\CardAmounts.xls"))
            {
                File.Delete(@"C:\Users\btmt0_000\Desktop\Balances\CardAmounts.xls");
            }
            //ef.Save(@"C:\Users\btmt0_000\Desktop\Balances\CardAmounts.xls", SaveOptions.XlsDefault);
            ef.Save(@"C:\CardAmounts.xls", SaveOptions.XlsDefault);

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
            var cardUrlDict = CreateCarDictionary();
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
                new StreamWriter(@"C:\" +  DateTime.Now.ToString("dd-M-yy_HH-mm-ss") +
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

