using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace app.Controllers
{
    public class Helper
    {
        private static ISessionFactory _sessionFactory;
        private static string _connectionString;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(_connectionString))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Students>())
                        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                        .BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession(string connectionString)
        {
            _connectionString = connectionString;
            return SessionFactory.OpenSession();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private ISession Session;

        public StudentController()
        {
            // Session = Helper.OpenSession("Server=192.168.1.50;User Id=AURORA;Password=MachDatum.1;Database=kct;Connection Timeout=30;");
             Session = Helper.OpenSession("Server=ANOOP;Database=kct; Integrated Security = True;");
        }

        [HttpGet]
        public ActionResult<List<Students>> Get()
        {
            List<Students> students = Session.Query<Students>().ToList();
            return students;
        }

        [HttpGet("{id}")]
        public ActionResult<Students> GetStudent(long id)
        {
            Students students = Session.Get<Students>(id);
            return students;
        }

        [HttpPost]
        public ActionResult<Students> Post(Students students)
        {
            Session.Save(students);
            Session.Flush();
            return Created(students.Id.ToString(), students);
        }

        [HttpPut]
        public ActionResult<Students> Put(Students students)
        {
            Students updateStudent = Session.Get<Students>(students.Id);

            if (updateStudent is null)
            {
                return NotFound();
            }

            updateStudent.Name = students.Name;
            updateStudent.RollNo = students.RollNo;
            updateStudent.Department = students.Department;
            updateStudent.DOB = students.DOB;

            Session.Update(updateStudent);
            Session.Flush();
            return updateStudent;
        }

        [HttpDelete]
        public ActionResult<long> Delete(long id)
        {
            Students deletedStudent= Session.Get<Students>(id);

            if (deletedStudent is null)
            {
                return NotFound();
            }

            Session.Delete(deletedStudent);
            Session.Flush();
            return id;
        }
    }
}
