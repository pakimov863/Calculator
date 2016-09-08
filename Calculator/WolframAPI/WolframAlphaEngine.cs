using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace WolframAlphaAPI
{
    public class WolframAlphaEngine
    {
        private String WA_APIKey;

        private WolframAlphaQueryResult WA_QueryResult;
        private WolframAlphaValidationResult WA_ValidationResult;

        public WolframAlphaEngine(String APIKey)
        {
            WA_APIKey = APIKey;
        }

        public String APIKey
        {
            get { return WA_APIKey; }
            set { WA_APIKey = value; }
        }

        public WolframAlphaQueryResult QueryResult
        {
            get { return WA_QueryResult; }
        }

        public WolframAlphaValidationResult ValidationResult
        {
            get { return WA_ValidationResult; }
        }

        #region Overloads of ValidateQuery
        public WolframAlphaValidationResult ValidateQuery(WolframAlphaQuery Query)
        {
            if (Query.APIKey == "")
            {
                if (this.APIKey == "")
                {
                    throw new Exception("To use the Wolfram Alpha API, you must specify an API key either through the parsed WolframAlphaQuery, or on the WolframAlphaEngine itself.");
                }
                Query.APIKey = this.APIKey;
            }

            if (Query.Asynchronous && Query.Format == WolframAlphaQueryFormat.HTML)
            {
                throw new Exception("Wolfram Alpha does not allow asynchronous operations while the format for the query is not set to \"HTML\".");
            }

            //System.Net.HttpWebRequest WebRequest = (System.Net.HttpWebRequest)(System.Net.WebRequest.Create("http://preview.wolframalpha.com/api/v1/validatequery.jsp" + Query.FullQueryString));
            System.Net.HttpWebRequest WebRequest = (System.Net.HttpWebRequest)(System.Net.WebRequest.Create("http://api.wolframalpha.com/v2/validatequery" + Query.FullQueryString));
            WebRequest.KeepAlive = true;
            String Response = new System.IO.StreamReader(WebRequest.GetResponse().GetResponseStream()).ReadToEnd();

            return ValidateQuery(Response);
        }

        public WolframAlphaValidationResult ValidateQuery(String Response)
        {
            XmlDocument Document = new XmlDocument();
            WolframAlphaValidationResult Result = null;
            try
            {
                Document.LoadXml(Response);
                Result = ValidateQuery(Document);
            }
            catch
            {
            }
            Document = null;

            return Result;
        }

        public WolframAlphaValidationResult ValidateQuery(XmlDocument Resp)
        {
            System.Threading.Thread.Sleep(1);

            XmlNode MainNode = Resp.SelectNodes("/validatequeryresult")[0];

            WA_ValidationResult = new WolframAlphaValidationResult();
            WA_ValidationResult.Success = Convert.ToBoolean(MainNode.Attributes["success"].Value);
            WA_ValidationResult.ErrorOccured = Convert.ToBoolean(MainNode.Attributes["error"].Value);
            WA_ValidationResult.Timing = Convert.ToDouble(MainNode.Attributes["timing"].Value.Replace(".",","));
            WA_ValidationResult.ParseData = MainNode.SelectNodes("parsedata")[0].InnerText;
            WA_ValidationResult.Assumptions = new List<WolframAlphaAssumption>();

            foreach (XmlNode Node in MainNode.SelectNodes("assumptions"))
            {
                System.Threading.Thread.Sleep(1);
                WolframAlphaAssumption Assumption = new WolframAlphaAssumption();
                Assumption.Word = Node.SelectNodes("word")[0].InnerText;
                XmlNode SubNode = Node.SelectNodes("categories")[0];
                foreach (XmlNode ContentNode in SubNode.SelectNodes("category"))
                {
                    System.Threading.Thread.Sleep(1);
                    Assumption.Categories.Add(ContentNode.InnerText);
                }
                WA_ValidationResult.Assumptions.Add(Assumption);
            }
            return WA_ValidationResult;
        }
        #endregion

        #region Overloads of LoadResponse
        public WolframAlphaQueryResult LoadResponse(WolframAlphaQuery Query)
        {
            if (Query.APIKey == "")
            {
                if (this.APIKey == "")
                {
                    throw new Exception("To use the Wolfram Alpha API, you must specify an API key either through the parsed WolframAlphaQuery, or on the WolframAlphaEngine itself.");
                }
                Query.APIKey = this.APIKey;
            }

            if (Query.Asynchronous && Query.Format == WolframAlphaQueryFormat.HTML)
            {
                throw new Exception("Wolfram Alpha does not allow asynchronous operations while the format for the query is not set to \"HTML\".");
            }

            //System.Net.HttpWebRequest WebRequest = (System.Net.HttpWebRequest)(System.Net.WebRequest.Create("http://preview.wolframalpha.com/api/v1/query.jsp" + Query.FullQueryString));
            System.Net.HttpWebRequest WebRequest = (System.Net.HttpWebRequest)(System.Net.WebRequest.Create("http://api.wolframalpha.com/v2/query" + Query.FullQueryString));
            WebRequest.KeepAlive = true;
            String Response = new System.IO.StreamReader(WebRequest.GetResponse().GetResponseStream()).ReadToEnd();

            return LoadResponse(Response);
        }

        public WolframAlphaQueryResult LoadResponse(String Response)
        {
            XmlDocument Document = new XmlDocument();
            WolframAlphaQueryResult Result = null;
            try
            {
                Document.LoadXml(Response);
                Result = LoadResponse(Document);
            }
            catch
            {
            }
            Document = null;

            return Result;
        }

        public WolframAlphaQueryResult LoadResponse(XmlDocument Response)
        {
            System.Threading.Thread.Sleep(1);

            XmlNode MainNode = Response.SelectNodes("/queryresult")[0];
            WA_QueryResult = new WolframAlphaQueryResult();
            WA_QueryResult.Success = Convert.ToBoolean(MainNode.Attributes["success"].Value);
            WA_QueryResult.ErrorOccured = Convert.ToBoolean(MainNode.Attributes["error"].Value);
            WA_QueryResult.NumberOfPods = Convert.ToInt32(MainNode.Attributes["numpods"].Value);
            WA_QueryResult.Timing = Convert.ToDouble(MainNode.Attributes["timing"].Value.Replace(".",","));
            WA_QueryResult.TimedOut = MainNode.Attributes["timedout"].Value;
            WA_QueryResult.DataTypes = MainNode.Attributes["datatypes"].Value;
            WA_QueryResult.Pods = new List<WolframAlphaPod>();

            foreach (XmlNode Node in MainNode.SelectNodes("pod"))
            {
                System.Threading.Thread.Sleep(1);

                WolframAlphaPod Pod = new WolframAlphaPod();

                Pod.Title = Node.Attributes["title"].Value;
                Pod.Scanner = Node.Attributes["scanner"].Value;
                Pod.Position = Convert.ToInt32(Node.Attributes["position"].Value);
                Pod.ErrorOccured = Convert.ToBoolean(Node.Attributes["error"].Value);
                Pod.NumberOfSubPods = Convert.ToInt32(Node.Attributes["numsubpods"].Value);
                Pod.SubPods = new List<WolframAlphaSubPod>();

                foreach (XmlNode SubNode in Node.SelectNodes("subpod"))
                {
                    System.Threading.Thread.Sleep(1);

                    WolframAlphaSubPod SubPod = new WolframAlphaSubPod();
                    SubPod.Title = SubNode.Attributes["title"].Value;

                    foreach (XmlNode ContentNode in SubNode.SelectNodes("plaintext"))
                    {
                        System.Threading.Thread.Sleep(1);
                        SubPod.PodText = ContentNode.InnerText;
                    }

                    foreach (XmlNode ContentNode in SubNode.SelectNodes("img"))
                    {
                        System.Threading.Thread.Sleep(1);

                        WolframAlphaImage Image = new WolframAlphaImage();
                        Image.Location = new Uri(ContentNode.Attributes["src"].Value);
                        Image.HoverText = ContentNode.Attributes["alt"].Value;
                        Image.Title = ContentNode.Attributes["title"].Value;
                        Image.Width = Convert.ToInt32(ContentNode.Attributes["width"].Value);
                        Image.Height = Convert.ToInt32(ContentNode.Attributes["height"].Value);
                        SubPod.PodImage = Image;
                    }

                    Pod.SubPods.Add(SubPod);
                }

                WA_QueryResult.Pods.Add(Pod);
            }

            return WA_QueryResult;
        }
        #endregion
    }
}