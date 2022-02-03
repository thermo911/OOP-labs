using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Entities.Accounts;
using Banks.Exceptions;

namespace Banks.Entities.Clients
{
    [Table("Clients")]
    public class Client : IEquatable<Client>, IIdentifiable
    {
        public Client()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }
        public HashSet<BankAccount> Accounts { get; init; }

        public void AddAccount(BankAccount account)
        {
            if (!Accounts.Add(account))
            {
                throw new BanksException(
                    $"client {Id.ToString()} already has account {account.Id.ToString()}");
            }
        }

        public bool IsSuspicious()
            => string.IsNullOrWhiteSpace(Passport)
               || string.IsNullOrWhiteSpace(Address);

        public void OnNotification()
        {
            // Do some _quite important_ stuff!!!
        }

        public bool Equals(Client other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Client)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Id.ToString();
    }
}