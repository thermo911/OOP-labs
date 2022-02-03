using System;
using Reports.DAL.Tools;

namespace Reports.Shrd.Dto
{
    public class TicketDto : IDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public int AssigneeId { get; set; }

        public DateTime OpenDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public TicketStatus Status { get; set; }
    }
}