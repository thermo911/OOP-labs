using System;
using System.Collections.Generic;
using Reports.DAL.Tools;

namespace Reports.Shrd.Dto
{
    public class ReportDto : IDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}