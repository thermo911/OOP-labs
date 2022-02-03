using System.Collections.Generic;
using Shops.Entities;
using Shops.Services;
using Shops.Tools.Exceptions;

namespace Shops.Managers
{
    public class PersonManager
    {
        private readonly IService<Person> _personService;

        public PersonManager(IService<Person> personService)
        {
            _personService = personService;
        }

        public void AddPerson(Person person) => _personService.Save(person);

        public void AddMoneyToPerson(uint personId, double money)
        {
            if (!_personService.TryGetById(personId, out Person person))
                throw new PersonNotFoundException($"Person with id {personId} not found");

            person.AddMoney(new Money(money));
        }

        public bool PersonCanAffordPurchase(uint personId, double requiredMoney)
        {
            if (!_personService.TryGetById(personId, out Person person))
                throw new PersonNotFoundException($"Person with id {personId} not found");

            return person.CanAffordPurchase(new Money(requiredMoney));
        }

        public bool PersonExists(uint personId) => _personService.ExistsWithId(personId);

        public bool TryGetPerson(uint personId, out Person result)
            => _personService.TryGetById(personId, out result);

        public IReadOnlyCollection<Person> GetAllPersons() => _personService.GetAll();
    }
}