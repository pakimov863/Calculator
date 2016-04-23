using System;
using System.Collections.Generic;

namespace WolframAlphaAPI
{
    public class WolframAlphaQueryResult
    {
        private List<WolframAlphaPod> WA_Pods;
        private Boolean WA_Success;
        private Boolean WA_Error;
        private Int32 WA_NumberOfPods;
        private String WA_DataTypes;
        private String WA_TimedOut;
        private Double WA_Timing;
        private Double WA_ParseTiming;

        public List<WolframAlphaPod> Pods
        {
            get { return WA_Pods; }
            set { WA_Pods = value; }
        }

        public Boolean Success
        {
            get { return WA_Success; }
            set { WA_Success = value; }
        }

        public Boolean ErrorOccured
        {
            get { return WA_Error; }
            set { WA_Error = value; }
        }

        public Int32 NumberOfPods
        {
            get { return WA_NumberOfPods; }
            set { WA_NumberOfPods = value; }
        }

        public String DataTypes
        {
            get { return WA_DataTypes; }
            set { WA_DataTypes = value; }
        }

        public String TimedOut
        {
            get { return WA_TimedOut; }
            set { WA_TimedOut = value; }
        }

        public Double Timing
        {
            get { return WA_Timing; }
            set { WA_Timing = value; }
        }

        public Double ParseTiming
        {
            get { return WA_ParseTiming; }
            set { WA_ParseTiming = value; }
        }
    }
}