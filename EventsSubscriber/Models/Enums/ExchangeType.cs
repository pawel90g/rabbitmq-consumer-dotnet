namespace EventsSubscriber.Models.Enums {
    public static class ExchangeType
    {
        public const string Direct = "direct";
        public const string Fanout = "fanout";

        public static string Parse(string value)
        {
            if (ExchangeType.Fanout.Equals(value))
                return ExchangeType.Fanout;

            return ExchangeType.Direct;
        }
    }
}