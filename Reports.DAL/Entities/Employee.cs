using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Reports.DAL.Entities
{
    public class Employee : IIdentifiable
    {
        private string _name;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(Name)} is null or whitespace");
                }

                _name = value;
            }
        }

        public int? ChiefId { get; set; }
    }
}