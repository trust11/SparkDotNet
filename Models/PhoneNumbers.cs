using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class PhoneNumbers : WebexObject
    {
        public Count Count { get; set; }

        public List<PhoneNumberDetails> PhoneNumbersDetails { get; set; }
    }

    public class Count : WebexObject
    {
        public int Assigned { get; set; }

        public int ExtensionOnly { get; set; }

        public int InActive { get; set; }

        public int TollFreeNumbers { get; set; }

        public int Total { get; set; }

        public int UnAssigned { get; set; }
    }

    public class NumberLocation : WebexObject
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class PhoneNumberDetails : WebexObject
    {
        public NumberLocation Location { get; set; }

        public bool MainNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumberType { get; set; }

        public string State { get; set; }

        public bool TollFreeNumber { get; set; }

        public Owner Owner { get; set; }
    }

    public class Owner : WebexObject
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}