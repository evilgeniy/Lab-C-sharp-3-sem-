using System;

namespace DB_Models
{
    [Serializable]
    public class Person
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

    }
}