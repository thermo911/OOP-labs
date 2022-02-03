using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reports.DAL.Entities
{
    public class Comment : IIdentifiable
    {
        private string _text;
        private int _authorId;
        private int _ticketId;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuthorId
        {
            get => _authorId;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(AuthorId)} is default");
                }
                
                _authorId = value;
            }
        }

        public int TicketId
        {
            get => _ticketId;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(AuthorId)} is default");
                }
                
                _ticketId = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(Text)} is null or whitespace");
                }

                _text = value;
            }
        }

        public DateTime CreationDateTime { get; set; }
    }
}