using System;
using System.Collections.Generic;
using Banks.Entities.Clients;
using Banks.Repositories;

namespace Banks.Services.Impl
{
    public class ClientService : IService<Client>
    {
        private IRepository<Client> _clientRepository;

        public ClientService(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client GetById(Guid id) => _clientRepository.GetById(id);

        public void Save(Client value) => _clientRepository.Save(value);

        public IEnumerable<Client> GetAll() => _clientRepository.GetAll();

        public bool ExistsWithId(Guid id) => _clientRepository.ExistsWithId(id);
    }
}