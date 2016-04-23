using System;

namespace WolframAlphaAPI
{
    public class WolframAlphaSubPod
    {
        private String WA_Title;
        private String WA_PodText;
        private WolframAlphaImage WA_PodImage;

        public String Title
        {
            get { return WA_Title; }
            set { WA_Title = value; }
        }

        public String PodText
        {
            get { return WA_PodText; }
            set { WA_PodText = value; }
        }

        public WolframAlphaImage PodImage
        {
            get { return WA_PodImage; }
            set { WA_PodImage = value; }
        }
    }
}