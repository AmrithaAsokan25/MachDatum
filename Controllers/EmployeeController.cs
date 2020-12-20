using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController: ControllerBase{
        private static readonly List<Employee> value = new List<Employee>(){
            new Employee(){
                Id = 1,
                Name = "Amritha",
                EmpNo = "CSE01",
                DOB = new DateTime(1999,1,1),
                Age= 20,
                Department="BE.CSE"
            },
            new Employee(){
                Id = 2,
                Name = "Abcd",
                EmpNo = "CSE02",
                DOB = new DateTime(1999,1,1),
                Age= 21,
                Department="BE.CSE"
            },
        };

        [HttpGet]
        public ActionResult<List<Employee>> Get(){
            return value;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetStudent(long id)
        {
            Employee employee = value.Single(x=>x.Id == id);
            return employee;
        }

        [HttpPost]
        public ActionResult<Employee> Post(Employee employee){
            employee.Id = value.Select(x=>x.Id).Max() + 1;
            value.Add(employee);
            return Created(employee.Id.ToString(), employee);
        }

        [HttpPut] 
        public ActionResult<Employee> Put(Employee employee){
            Employee updatedEmployee = value.SingleOrDefault(x => x.Id == employee.Id);
            if(updatedEmployee is null){
                return NotFound();
            }

            updatedEmployee.Name = employee.Name;
            updatedEmployee.EmpNo = employee.EmpNo;
            updatedEmployee.DOB = employee.DOB;
            updatedEmployee.Age = employee.Age;
            updatedEmployee.Department = employee.Department;
            return updatedEmployee;
        }

        [HttpDelete]
        public ActionResult<long> Delete(long id){
            Employee deletedEmployee = value.SingleOrDefault(x=>x.Id == id);

            if (deletedEmployee is null)
            {
                return NotFound();
            }

            value.Remove(deletedEmployee);
            return id;
        }
    }
}