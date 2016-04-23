using System;
using System.Collections.Generic;

namespace WolframAlphaAPI
{
    public class WolframAlphaAssumption
    {
        private String WA_Word;
        private List<String> WA_Categories = new List<string>();

        public String Word
        {
            get { return WA_Word; }
            set { WA_Word = value; }
        }

        public List<String> Categories
        {
            get { return WA_Categories; }
            set { WA_Categories = value; }
        }
    }
}