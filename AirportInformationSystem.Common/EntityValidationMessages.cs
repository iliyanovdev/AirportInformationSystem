namespace AirportInformationSystem.Common
{
    public static class EntityValidationMessages
    {
        public static class FlightRoute
        {
            public const string DepartureCityRequiredMessage = "Please enter a departure city.";
            public const string ArrivalCityRequiredMessage = "Please enter an arrival city.";
        }

        public static class Flight
        {
            public const string FlightDateRequiredMessage = "Please enter a flight date.";
            public const string DepartureDateRequireMessage = "Please select a departure date.";
            public const string ReturnDateRequireMessage = "Please select a return date.";
            public const string ReturnDateConstraintMessage = "Return date cannot be before departure date.";
        }

        
    }
}
