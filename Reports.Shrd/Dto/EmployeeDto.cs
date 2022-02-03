namespace Reports.Shrd.Dto
{
    public class EmployeeDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ChiefId { get; set; }
    }
}