using Newtonsoft.Json;

namespace EventsSubscriber.Models.Abstract
{
    public abstract class EventBase
    {
        [JsonIgnore]
        public string Severity { get; protected set; }
        [JsonIgnore]
        public long? Expiration { get; protected set; }

        private EventBase() { }

        protected EventBase(string severity)
        {
            Severity = severity;
            Expiration = null;
        }

        protected EventBase(string severity, long expiration)
        {
            Severity = severity;
            Expiration = expiration;
        }
    }
}