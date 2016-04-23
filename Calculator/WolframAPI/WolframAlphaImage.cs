using System;
using System.Web;

namespace WolframAlphaAPI
{
    public class WolframAlphaImage
    {
        private Uri WA_Location;
        private Int32 WA_Width;
        private Int32 WA_Height;
        private String WA_Title;
        private String WA_HoverText;

        public Uri Location
        {
            get { return WA_Location; }
            set { WA_Location = value; }
        }

        public Int32 Width
        {
            get { return WA_Width; }
            set { WA_Width = value; }
        }

        public Int32 Height
        {
            get { return WA_Height; }
            set { WA_Height = value; }
        }

        public String Title
        {
            get { return WA_Title; }
            set { WA_Title = value; }
        }

        public String HoverText
        {
            get { return WA_HoverText; }
            set { WA_HoverText = value; }
        }
    }
}