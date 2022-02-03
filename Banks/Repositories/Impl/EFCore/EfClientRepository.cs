using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Clients;
using Banks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Banks.Repositories.Impl.EFCore
{
    public class EfClientRepository : IRepository<Client>
    {
        private BanksApplicationContext context = BanksApplicationContext.Context;

        public Client GetById(Guid id)
        {
            Client result = context.Clients
                .Include(client => client.Accounts)
                .FirstOrDefault(client => client.Id == id);

            if (result == null)
                throw new NotFoundException($"client with id {id} not found");

            return result;
        }

        public bool ExistsWithId(Guid id)
        {
            return context.Clients.Any(client => client.Id == id);
        }

        public IEnumerable<Client> GetAll()
        {
            return context.Clients
                .Include(client => client.Accounts);
        }

        public void Save(Client value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            context.Clients.Update(value);
            context.SaveChanges();
        }
    }
}