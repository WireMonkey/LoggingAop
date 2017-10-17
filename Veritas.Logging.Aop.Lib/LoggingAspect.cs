using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Veritas.Logging.Aop.Lib
{
    [Serializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {

        public override void OnEntry(MethodExecutionArgs args)
        {
            var stackTrace = new StackTrace().GetFrame(1);

            LogManager.ReconfigExistingLoggers();
            var log = LogManager.GetLogger(stackTrace.GetType().Name);
            log.Debug(string.Format("{0} started with parms {1}", stackTrace.GetMethod().Name, string.Join(", ",args.Arguments)));

            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var stackTrace = new StackTrace().GetFrame(1);
            
            LogManager.ReconfigExistingLoggers();
            var log = LogManager.GetLogger(stackTrace.GetType().Name);
            log.Debug(string.Format("{0} ended.", stackTrace.GetMethod().Name));

            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            var stackTrace = new StackTrace().GetFrame(1);

            LogManager.ReconfigExistingLoggers();
            var log = LogManager.GetLogger(stackTrace.GetType().Name);
            log.Error(args.Exception, string.Format("{0} Errored. Message {1}", stackTrace.GetMethod().Name, args.Exception.Message));

            base.OnException(args);
        }
    }
}
