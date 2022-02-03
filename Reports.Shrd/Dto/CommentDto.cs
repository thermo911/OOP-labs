using System;

namespace Reports.Shrd.Dto
{
    public class CommentDto : IDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int TicketId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}