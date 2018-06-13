using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsk.ParsingFromWeb
{
    public class Parsing
    {
        List<string> BusList = new List<string>();
        public List<TransportUnit> Shedule = new List<TransportUnit>();

        public string SaveHTMLPage()
        {
            WebRequest wreq = WebRequest.Create("http://www.minsktrans.by/pda/?Transport=Autobus&m=1&day=5");
            string str1="";
                WebResponse wresp = wreq.GetResponse();
                Stream stream = wresp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                str1 = sr.ReadToEnd();
                sr.Close();
                wresp.Close();
            return str1;
            //string L;
            //using (StreamWriter streamWriter = new StreamWriter(@"1.txt"))
            //{
            //    while ((L = sr.ReadLine()) != null)
            //    {

            //        streamWriter.Write(L);
            //    }
            //    streamWriter.Close();
            //}


            //string str1 = File.ReadAllText("1.txt");

        }

        public string SaveHTMLPage(string webSite, string txtName)
        {
            WebRequest wreq = WebRequest.Create(webSite);
            WebResponse wresp = wreq.GetResponse();
            Stream stream = wresp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string str1 = sr.ReadToEnd();
            //string L;
            //using (StreamWriter streamWriter = new StreamWriter(@"1.txt"))
            //{
            //    while ((L = sr.ReadLine()) != null)
            //    {

            //        streamWriter.Write(L);
            //    }
            //    streamWriter.Close();
            //}
            sr.Close();
            wresp.Close();

            //string str1 = File.ReadAllText(txtName + ".txt");
            return str1;
        }

        public void ParseHTMLPage(string str1, string regularExpression)
        {
            Regex regCheckNumberOfBus = new Regex(@"[\d!#h]"); // Соответствует любая цифра, восклицательный знак, решётка или буква h. Если нужны только цифры, то @"\d".
            Regex regCheckNumberOfBus2 = new Regex(@"[-]"); // ищет черточки в предложениях
            List<string> parsingList = new List<string>();
            Regex reg = new Regex(regularExpression);
            MatchCollection matches = reg.Matches(str1);

            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    parsingList.Add(match.Groups[1].ToString());
                    string currentRouteDetail = match.Groups[1].ToString().Split()[0];
                    Match m = regCheckNumberOfBus.Match(currentRouteDetail);
                    Match m2 = regCheckNumberOfBus2.Match(currentRouteDetail);

                    if (m.Success && !m2.Success && currentRouteDetail.Count() < 4) //не совсем корректно
                    {
                        ParseDetailedRoute(currentRouteDetail);
                    }
                    //if (currentRouteDetail == "11") //чтобы долго не ждать))0)
                    //{
                    //    break;
                    //}
                }

            PrintList(parsingList);
        }

        public void ParseHTMLPageBusStop(string str1, string regularExpression, string numberOfBus)
        {
            List<string> parsingList = new List<string>();
            Regex reg = new Regex(regularExpression);
            MatchCollection matches = reg.Matches(str1);

            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    parsingList.Add(match.Groups[1].ToString());
                }

            PrintList(parsingList);

            //Shedule.Add(new TransportUnit(numberOfBus, parsingList.GetRange(0, parsingList.Count / 2), parsingList.GetRange(0, parsingList.Count / 2)));//дописать разбиение листа маршрута на туда и обратно
            Shedule.Add(new TransportUnit(numberOfBus, "dvasdv", "rereer"));
            //File.WriteAllLines(numberOfBus + ".txt", parsingList);
        }

        public void PrintShedule()
        {
            foreach (var item in Shedule)
            {
                //Console.WriteLine(item.number);
                //Console.WriteLine("туда");
                foreach (var wayto in item.wayTo)
                {
                    //Console.WriteLine(wayto);
                }
                //Console.WriteLine("обратно");
                foreach (var wayfrom in item.wayFrom)
                {
                    //Console.WriteLine(wayfrom);
                }
            }
        }

        public void PrintList()
        {
            foreach (var item in BusList)
            {
                //Console.WriteLine(item);
            }
        }

        public void PrintList(List<string> list)
        {
            foreach (var item in list)
            {
                //Console.WriteLine(item);
            }
            //File.WriteAllLines("1adv.txt", list);
        }

        public void ParseDetailedRoute(string numberOfBus)
        {
            string reg = "<a href='\\?RouteNum=" + numberOfBus + "&StopID=.*?&RouteType=[AB]%3E[AB]&day=.*?Transport=Autobus'>(.*?)<\\/a>"; //routetype A и B заменил на [AB]
            //Console.WriteLine(numberOfBus);
            string str = SaveHTMLPage("http://www.minsktrans.by/pda/?RouteNum=" + numberOfBus + "&day=5&Transport=Autobus", numberOfBus);
            ParseHTMLPageBusStop(str, reg, numberOfBus);
        }
    }
}