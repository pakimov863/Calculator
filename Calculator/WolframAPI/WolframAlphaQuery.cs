using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolframAlphaAPI
{
    public class WolframAlphaQuery
    {
        //private const String MainRoot = "http://api.wolframalpha.com/v1/query.jsp";

        private String WA_APIKey;

        public String APIKey
        {
            get { return WA_APIKey; }
            set { WA_APIKey = value; }
        }

        private String WA_Format;
        private String WA_Substitution;
        private String WA_Assumption;
        private String WA_Query;
        private String WA_PodTitle;
        private Int32 WA_TimeLimit;
        private Boolean WA_AllowCached;
        private Boolean WA_Asynchronous;
        private Boolean WA_MoreOutput;

        public Boolean MoreOutput
        {
            get { return WA_MoreOutput; }
            set { WA_MoreOutput = value; }
        }

        public String Format
        {
            get { return WA_Format; }
            set { WA_Format = value; }
        }

        public Boolean Asynchronous
        {
            get { return WA_Asynchronous; }
            set { WA_Asynchronous = value; }
        }

        public Boolean AllowCaching
        {
            get { return WA_AllowCached; }
            set { WA_AllowCached = false; }
        }

        public String Query
        {
            get { return WA_Query; }
            set { WA_Query = value; }
        }

        public Int32 TimeLimit
        {
            get { return WA_TimeLimit; }
            set { WA_TimeLimit = value; }
        }

        public void AddPodTitle(String PodTitle, Boolean CheckForDuplicates = false)
        {
            if (CheckForDuplicates && WA_PodTitle.Contains("&podtitle=" + HttpUtility.UrlEncode(PodTitle))) return; //было "&PodTitle="
            WA_PodTitle += "&podtitle=" + HttpUtility.UrlEncode(PodTitle);
        }

        public void AddSubstitution(String Substitution, Boolean CheckForDuplicates = false)
        {
            if (CheckForDuplicates && WA_Substitution.Contains("&substitution=" + HttpUtility.UrlEncode(Substitution))) return;
            WA_Substitution += "&substitution=" + HttpUtility.UrlEncode(Substitution);
        }

        public void AddAssumption(String Assumption, Boolean CheckForDuplicates = false)
        {
            if (CheckForDuplicates && WA_Assumption.Contains("&substitution=" + HttpUtility.UrlEncode(Assumption))) return;
            WA_Assumption += "&assumption=" + HttpUtility.UrlEncode(Assumption);
        }

        public void AddAssumption(WolframAlphaAssumption Assumption, Boolean CheckForDuplicates = false)
        {
            if (CheckForDuplicates && WA_Assumption.Contains("&substitution=" + HttpUtility.UrlEncode(Assumption.Word))) return;
            WA_Assumption += "&assumption=" + HttpUtility.UrlEncode(Assumption.Word);
        }

        public String[] Substitutions
        {
            get { return WA_Substitution.Split(new String[] { "&substitution=" }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public String[] Assumptions
        {
            get { return WA_Assumption.Split(new String[] { "&assumption=" }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public String[] PodTitles
        {
            get { return WA_PodTitle.Split(new String[] { "&assumption=" }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public String FullQueryString
        {
            get { return "?appid=" + WA_APIKey + "&moreoutput=" + MoreOutput + "&timelimit=" + TimeLimit + "&format=" + WA_Format + "&input=" + HttpUtility.UrlEncode(WA_Query) + WA_Assumption + WA_Substitution; }
        }

    }
}