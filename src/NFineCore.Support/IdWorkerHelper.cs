using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.Support
{
    public class IdWorkerHelper
    {
        private static IdWorker _idWorker = null;
        static IdWorkerHelper()
        {
            _idWorker = new IdWorker(1, 1);
        }
        public static long GenId64()
        {
            return _idWorker.NextId();
        }
    }
}
