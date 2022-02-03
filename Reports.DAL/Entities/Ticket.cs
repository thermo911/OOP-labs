using System;
using System.ComponentModel.DataAnnotations.Schema;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class Ticket : IIdentifiable
    {
        private string _title;
        private int _authorId;
        private int _assigneeId;
        private string _description;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(Title)} is null or whitespace");
                }
                
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(Description)} is null or whitespace");
                }
                
                _description = value;
            }
        }

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

        public int AssigneeId
        {
            get => _assigneeId;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(AssigneeId)} is default");
                }
                
                _assigneeId = value;
            }
        }

        public DateTime OpenDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public TicketStatus Status { get; set; }
    }
}
