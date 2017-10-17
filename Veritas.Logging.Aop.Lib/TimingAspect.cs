using NLog;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veritas.Logging.Aop.Lib
{

    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class TimingAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch _stopwatch;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopwatch = Stopwatch.StartNew();
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var stackTrace = new StackTrace().GetFrame(1);

            LogManager.ReconfigExistingLoggers();
            var log = LogManager.GetLogger(stackTrace.GetType().Name);
            log.Debug(string.Format("{0} took {1}ms to execute", new StackTrace().GetFrame(1).GetMethod().Name, _stopwatch.ElapsedMilliseconds));
            
            base.OnExit(args);
        }
    }
}
