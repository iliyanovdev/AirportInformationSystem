namespace AirportInformationSystem.Common
{
    public static class EntityValidationConstants
    {
        public static class Country
        {
            public const int NameMaxLength = 100;
        }

        public static class City
        {
            public const int NameMaxLength = 100;
        }

        public static class AirplaneType
        {
            public const int ModelMaxLength = 30;
        }

        public static class Company
        {
            public const int NameMaxLength = 100;
            public const int PrefixMaxLength = 3;
        }

        public static class FlightRoute
        {
            
        }

        public static class Flight
        {
            public const int FlightCodeMaxLength = 25;
            public const string FlightCodeDateFormat = "yyyyMMdd";
        }
    }
}
