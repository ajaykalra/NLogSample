using System;
using System.Reactive.Subjects;
using NLog;
using NLog.Targets;

namespace NLogSample
{
    [Target("MemoryTargetEx")] 
    class MemoryTargetEx : TargetWithLayout
    {
        private readonly Subject<string> _messages = new Subject<string>();

        public MemoryTargetEx()
        {
            Messages = _messages;
        }
        
        protected override void Write(LogEventInfo logEvent)
        {
            string item = Layout.Render(logEvent);
            _messages.OnNext(item);
        }
        
        public IObservable<string> Messages { get; private set; }
    }
}
