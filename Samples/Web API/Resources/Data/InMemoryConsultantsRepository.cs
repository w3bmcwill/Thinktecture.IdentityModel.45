using System;
using System.Linq;

namespace Resources
{
    public class InMemoryConsultantsRepository : IConsultantsRepository
    {
        static Consultants _consultants;

        static InMemoryConsultantsRepository()
        {
            _consultants = new Consultants
            {
                new Consultant
                {
                    ID = 1,
                    Name = "Dominick Baier",
                    EmailAddress = "dominick.baier@thinktecture.com",
                    Country = "DE",
                    Owner = "christian"
                },
                new Consultant
                {
                    ID = 2,
                    Name = "Christian Weyer",
                    EmailAddress = "christian.weyer@thinktecture.com",
                    Country = "DE",
                    Owner = "christian"
                },
                new Consultant
                {
                    ID = 3,
                    Name = "Ingo Rammer",
                    EmailAddress = "ingo.rammer@thinktecture.com",
                    Country = "Pampa",
                    Owner = "christian"
                },
                new Consultant
                {
                    ID = 4,
                    Name = "Richard Blewett",
                    EmailAddress = "richard.blewett@thinktecture.com",
                    Country = "UK",
                    Owner = "dominick"
                },
                new Consultant
                {
                    ID = 5,
                    Name = "Oliver Sturm",
                    EmailAddress = "oliver.sturm@thinktecture.com",
                    Country = "UK",
                    Owner = "dominick"
                },
                new Consultant
                {
                    ID = 6,
                    Name = "Jörg Neumann",
                    EmailAddress = "joerg.neumann@thinktecture.com",
                    Country = "DE",
                    Owner = "dominick"
                },
                new Consultant
                {
                    ID = 7,
                    Name = "Christian Nagel",
                    EmailAddress = "christian.nagel@thinktecture.com",
                    Country = "AT",
                    Owner = "dominick"
                },
            };
        }

        public Consultants GetAll()
        {
            return _consultants;
        }

        public int Add(Consultant consultant)
        {
            int newId = _consultants.Max(c => c.ID) + 1;
            consultant.ID = newId;

            _consultants.Add(consultant);
            return newId;
        }

        public void Update(Consultant consultant)
        {
            var oldConsulant = _consultants.Single(c => c.ID == consultant.ID);
            _consultants.Remove(oldConsulant);

            _consultants.Add(consultant);
        }
    }
}
