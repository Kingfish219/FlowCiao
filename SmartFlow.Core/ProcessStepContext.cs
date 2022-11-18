using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFlow.Core
{
    public class ProcessStepContext
    {
        private static ProcessStepContext _instance=new ProcessStepContext();
        public Dictionary<string, object> Context { get; private set; }

        public ProcessStepContext() 
        {
            Context = new Dictionary<string, object>();
        }

        public static ProcessStepContext ReturnInstance()
        {
            return _instance ?? new ProcessStepContext();
        }

        internal static void ClearDictionary()
        {
            _instance = ReturnInstance();
            _instance.Context = new Dictionary<string, object>();
        }
    }
}
