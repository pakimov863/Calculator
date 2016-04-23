using System;
using System.Collections.Generic;

namespace WolframAlphaAPI
{
    public class WolframAlphaValidationResult
    {
        private String WA_ParseData;
        private List<WolframAlphaAssumption> WA_Assumptions;
        private Boolean WA_Success;
        private Boolean WA_Error;
        private Double WA_Timing;

        public Boolean Success
        {
            get { return WA_Success; }
            set { WA_Success = value; }
        }

        public String ParseData
        {
            get { return WA_ParseData; }
            set { WA_ParseData = value; }
        }

        public List<WolframAlphaAssumption> Assumptions
        {
            get { return WA_Assumptions; }
            set { WA_Assumptions = value; }
        }

        public Boolean ErrorOccured
        {
            get { return WA_Error; }
            set { WA_Error = value; }
        }

        public Double Timing
        {
            get { return WA_Timing; }
            set { WA_Timing = value; }
        }
    }
}