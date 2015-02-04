using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft.File;
using Globussoft.RegexExp;
using Globussoft.Utils;
using System.Text.RegularExpressions;
using System.Threading;


namespace UkCrawler
{
    public partial class frmMain : Form
    {
        GlobusHttpHelper httpHelper = new Globussoft.Utils.GlobusHttpHelper();
        GlobussRegex globussRegex = new GlobussRegex();

        List<string> lstEmail = new List<string>();
        List<string> lstTempData;
        List<string> lstUrl = new List<string>();

        #region Thread

        /* We will store our worker threads in this list. */
        private List<Thread> threads = new List<Thread>();

        /* The number of worker threads to use */
        private int numberOfThread = 50;

        /* synchronization lock */
        private object locker = new object();

        /* The queue in which jobs are stored */
        private Queue<string> queueOfCsv = new Queue<string>();

        /* public variable for accessing threadsToUseProperty */
        public int NumberOfThread { set { numberOfThread = value; } get { return NumberOfThread; } }

        /* wait handle - used to wake up sleeping threads and to make them wait (sleep) */
        EventWaitHandle wh = new AutoResetEvent(false);

        Thread threadCategory;
        Thread mythread;

        private void WorkerQueueOfCategoryUrl()
        {
            /* we will start worker threads, which will wait for a queueCatlog to be entered into the queue.*/
            for (int i = 0; i < numberOfThread; i++)
            {
                threadCategory = new Thread(workCategoryUrl);
                threads.Add(threadCategory);
                threadCategory.Start();
            }
        }

        public void AddCategoryUrl(string Url)
        {
            lock (locker)
            {
                queueOfCsv.Enqueue(Url);
                Console.WriteLine("Eequeue" + Url);
            }
            wh.Set();
        }

        private void workCategoryUrl()
        {
            while (true)
            {
                string Url = null;
                lock (locker)
                {
                    if (queueOfCsv.Count > 0)
                    {
                        Console.WriteLine("Dequeue" + Url);
                        Url = queueOfCsv.Dequeue();
                        /* return if a null is found in the queue */
                        if (Url == null) return;
                    }
                }
                if (Url != null)
                {
                    /* if a job was found then process it */
                    GetData(Url);

                }
                else
                {
                    /* if a job was not found (meaning list is empty) then
                    * wait till something is added to it
                    */
                    wh.WaitOne();
                }
            }
        }

        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Thread threadGetUrl = new Thread(filterdata);
            threadGetUrl.Start();

           
        }

        private void GetUrl()
        {
            StringBuilder strBuilder = new StringBuilder();
            string Result = string.Empty;
            string TempHtmlData = string.Empty;
            string AnchorUrl = string.Empty;
            string Email = string.Empty;
            string Website = string.Empty;
            string TempWebsite = string.Empty;
            string Fax = string.Empty;
            string TempFax = string.Empty;
            string Id = string.Empty;
            string TempId = string.Empty;
            string Web = string.Empty;
            string TmpDatas = string.Empty;
           
            //string ResponseHtmlData1 = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/home.law"));
            Console.WriteLine("Fetching First Html");
            string ResponseHtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor.law"));
            Console.WriteLine("Fetching Second Html");

            int j=1050;//549;737//1241;2000
            for (int i = 749; i <= j; )
            {
                 List<string> lstEmailNew = new List<string>();;
                //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=F:Criminal%20law&PANELMEM=&LAWFIRM="));
                //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=JJ:Computer%20and%20IT%20law&PANELMEM=&LAWFIRM="));
                //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=N:Commercial%20litigation&PANELMEM=&LAWFIRM="));
                //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=PP:Fraud&PANELMEM=&LAWFIRM="));
                 //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=AB:Advocacy%20&PANELMEM=&LAWFIRM="));

                 //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=UU:Media%20and%20entertainment%20law&PANELMEM=&LAWFIRM="));
                 string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=KK:Construction%20and%20civil%20engineering&PANELMEM=&LAWFIRM="));
                 //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=UU:Media%20and%20entertainment%20law&PANELMEM=&LAWFIRM="));
                 //string HtmlData = httpHelper.getHtmlfromUrl(new Uri("http://www.lawsociety.org.uk/choosingandusing/findasolicitor/action=lawfirmsearch.law?startrow="+i+"&COUNTRY=ENGLAND&AREA=X:Intellectual%20property%20law&PANELMEM=&LAWFIRM="));
                Console.WriteLine("Fetching Third Html");

                //List<string> lst = new List<string>();
                //List<string> lstNew = new List<string>();
                //lst = globussRegex.GetUrlsFromString(HtmlData);

                //foreach (string str in lst)
                //{
                //    lstNew.Add(str);
                //}

                //string jj;

                int FirstPoint = HtmlData.IndexOf("width=\"550\">");
                if (FirstPoint > 0)
                {
                    TempHtmlData = HtmlData.Substring(FirstPoint);
                }
                int SecondPoint = TempHtmlData.IndexOf("<tr>");
                int ThirdPoint = TempHtmlData.IndexOf("</table>");

                if (FirstPoint > 0 && ThirdPoint > 0)
                {
                    string TempData = TempHtmlData.Substring(SecondPoint, ThirdPoint - SecondPoint);
                    string[] arTr = Regex.Split(TempData, "<tr>");
                    lstTempData = new List<string>();
                    foreach (string strTr in arTr)
                    {
                        string[] arTd = Regex.Split(strTr, "</td>");
                        foreach (string strTd in arTd)
                        {
                            if (string.IsNullOrEmpty(AnchorUrl))
                            {

                                AnchorUrl = globussRegex.GetAnchorTag(strTd);
                                if (!string.IsNullOrEmpty(AnchorUrl))
                                {
                                    int HerfSecondPoint = AnchorUrl.IndexOf("href=");
                                    int HerfThirdPoint = AnchorUrl.IndexOf("=L");

                                    if (HerfSecondPoint > 0 && HerfThirdPoint > 0)
                                    {
                                        string TempWeb = AnchorUrl.Substring(HerfSecondPoint, HerfThirdPoint - HerfSecondPoint).Replace("href=", "").Replace("\"", "").Replace(":80", "");
                                        Web = globussRegex.StripTagsRegex(TempWeb + "=L");
                                    }

                                    string AnchorUrlHtmlData = httpHelper.getHtmlfromUrl(new Uri(Web));
                                    List<string> lstEmail = new List<string>();
                                    lstEmail = globussRegex.GetEmailsFromString(AnchorUrlHtmlData);

                                    //foreach (string str in lstEmail)
                                    //{
                                    //    Console.WriteLine("Saving Data" + str+"Page Number"+i);
                                    //    Savedata(str);
                                    //}

                                    if (lstEmail.Count > 0)
                                    {
                                        Email = lstEmail[0].ToString();
                                        Console.WriteLine("Email --" + Email);
                                    }




                                    int FaxFirstPoint = AnchorUrlHtmlData.IndexOf("Fax Number");
                                    if (FaxFirstPoint > 0)
                                    {
                                        TempFax = AnchorUrlHtmlData.Substring(FaxFirstPoint);
                                    }

                                    int FaxSecondPoint = TempFax.IndexOf(":");
                                    int FaxThirdPoint = TempFax.IndexOf("</b>");

                                    if (FaxSecondPoint > 0 && FaxThirdPoint > 0)
                                    {
                                        Fax = TempFax.Substring(FaxSecondPoint, FaxThirdPoint - FaxSecondPoint).Replace(":", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                                        Console.WriteLine("Fax --" + Fax);
                                    }

                                    int WebSiteFirstPoint = AnchorUrlHtmlData.IndexOf("Web Site");
                                    if (WebSiteFirstPoint > 0)
                                    {
                                        TempWebsite = AnchorUrlHtmlData.Substring(WebSiteFirstPoint);
                                    }

                                    int WebsiteSecondPoint = TempWebsite.IndexOf(":");
                                    int WebsiteThirdPoint = TempWebsite.IndexOf("</b>");

                                    if (WebsiteSecondPoint > 0 && WebsiteThirdPoint > 0)
                                    {
                                        string TempWeb = TempWebsite.Substring(WebsiteSecondPoint, WebsiteThirdPoint - WebsiteSecondPoint);
                                        Website = globussRegex.StripTagsRegex(TempWeb.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                                        Console.WriteLine("Website --" + Website);
                                    }

                                    int IdFirstPoint = AnchorUrlHtmlData.IndexOf("ID:");
                                    if (IdFirstPoint > 0)
                                    {
                                        TempId = AnchorUrlHtmlData.Substring(IdFirstPoint);
                                    }

                                    int IdSecondPoint = TempId.IndexOf("width=\"330\"");
                                    if (IdSecondPoint > 0)
                                    {
                                        TempId = TempId.Substring(IdSecondPoint);
                                    }


                                    int IdThirdPoint = TempId.IndexOf("</td>");

                                    if (IdThirdPoint > 0)
                                    {
                                        string TempIds = TempId.Substring(0, IdThirdPoint);
                                        Id = globussRegex.StripTagsRegex(TempIds).Replace(":", "").Replace("\r", "").Replace("\n", "").Replace("\t", "")
                                                                            .Replace("class=\"text\"", "").Replace(">", "").Replace(" ", "").Replace("width=\"330\"", "");
                                        Console.WriteLine("Id --" + Id);
                                    }

                                    TmpDatas = Email + "," + Fax + "," + Website + "," + Id;
                                    Console.WriteLine("TmpDatas --" + TmpDatas);
                                    //if (!string.IsNullOrEmpty(Email))
                                    //{
                                    //    string FilePath = Application.StartupPath + "\\UkEmail.csv";
                                    //    GlobusFileHelper.AppendStringToTextfileNewLine(Email, FilePath);
                                    //}

                                }
                            }

                            string tempstrTd = globussRegex.StripTagsRegex(strTd).Replace("\t", "").Replace("\r", "").Replace("\n", "");
                            Result += tempstrTd + ",";
                            
                        }

                        Console.WriteLine("Result" + Result + TmpDatas);

                        Monitor.Enter(this);
                        Console.WriteLine("writting --" + Result + TmpDatas);
                      
                       
                        Savedata(Result + TmpDatas);
                        Monitor.Exit(this);
                        lstTempData.Add(Result + TmpDatas);

                        Result = string.Empty;
                        TempHtmlData = string.Empty;
                        AnchorUrl = string.Empty;
                        Email = string.Empty;
                        Website = string.Empty;
                        TempWebsite = string.Empty;
                        Fax = string.Empty;
                        TempFax = string.Empty;
                        Id = string.Empty;
                        TempId = string.Empty;
                        Web = string.Empty;
                        TmpDatas = string.Empty;
                    }

                    //foreach (string strData in lstEmailNew.Distinct().ToList())
                    //{
                    //    string Temp = strData.Replace(",", "");
                    //    if (!string.IsNullOrEmpty(Temp))
                    //    {
                    //        Console.WriteLine("writting --" + Temp);
                    //        Savedata(strData);
                    //    }
                    //}

                   
                }
                i = i + 50;
            }
            
         
            
        }

        private void GetData(string Url) { }
        //{
        //    GlobusHttpHelper objhttpHelper = new GlobusHttpHelper();
        //    string HtmlData = objhttpHelper.getHtmlfromUrl(new Uri(Url));
        //    Console.Write("Fetching Email");
        //    lstEmail = globussRegex.GetEmailsFromString(HtmlData);
        //    Console.WriteLine("Fetched Email");
        //    Monitor.Enter(this);
        //    Console.Write("SavingData");
        //    Savedata();
        //    Monitor.Exit(this);
        //}

        private void Savedata(string rslt)
        {
            string FilePath = Application.StartupPath + "\\md.csv";
            GlobusFileHelper.AppendStringToTextfileNewLine(rslt, FilePath);

            //foreach (string str in lstEmail)
            //{
            //    GlobusFileHelper.AppendStringToTextfileNewLine(str, FilePath);
            //}
        }

        private void filterdata()
        {
            List<string> lst = new List<string>();
            string FilePath = Application.StartupPath + "\\asdfg.csv";
            lst = GlobusFileHelper.readcsvfile(Application.StartupPath + "\\md.csv");
            foreach (string str in lst)
            {
                string Temp = str.Replace(",", "");
                if(!string.IsNullOrEmpty(Temp) && !str.Contains("Name") && !str.Contains("Office"))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(str, FilePath);
                }
            }
        }

        
    }
}
