using System;
using Reports.DAL.Entities;
using Reports.Shrd.Mappers;
using Reports.Shrd.Mappers.Impl;
using Reports.Shrd.Dto;

namespace Reports.Shrd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Employee employee = new Employee()
            {
                Id = 10,
                ChiefId = 3,
                Name = "Alex"
            };

            IMapper mapper = new SimpleMapper();
            EmployeeDto dto = mapper.Map<EmployeeDto>(employee);
            Console.WriteLine(dto.Id);
            Console.WriteLine(dto.ChiefId);
            Console.WriteLine(dto.Name);

            Employee e = mapper.Map<Employee>(dto);
            Console.WriteLine(e.Id);
            Console.WriteLine(e.ChiefId);
            Console.WriteLine(e.Name);
        }
    }
}