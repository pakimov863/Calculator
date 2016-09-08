using System;
using System.Collections.Generic;

namespace WolframAlphaAPI
{
    public class WolframAlphaPod
    {
        private List<WolframAlphaSubPod> WA_SubPods;
        private String WA_Title;
        private String WA_Scanner;
        private Int32 WA_Position;
        private Boolean WA_Error;
        private Int32 WA_NumberOfSubPods;

        public List<WolframAlphaSubPod> SubPods
        {
            get { return WA_SubPods; }
            set { WA_SubPods = value; }
        }

        public String Title
        {
            get { return WA_Title; }
            set { WA_Title = value; }
        }

        public String Scanner
        {
            get { return WA_Scanner; }
            set { WA_Scanner = value; }
        }

        public Int32 Position
        {
            get { return WA_Position; }
            set { WA_Position = value; }
        }

        public Boolean ErrorOccured
        {
            get { return WA_Error; }
            set { WA_Error = value; }
        }

        public Int32 NumberOfSubPods
        {
            get { return WA_NumberOfSubPods; }
            set { WA_NumberOfSubPods = value; }
        }
    }
}