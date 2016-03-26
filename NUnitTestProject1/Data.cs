using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject1
{
    class Data
    {
        

        public static Dictionary<string, string> CreateCarDictionary()
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

    }

}

