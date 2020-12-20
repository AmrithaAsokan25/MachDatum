using System;
using FluentNHibernate.Mapping;

namespace app
{
    public class Students
    {
        public virtual long Id { get; set; }
        public virtual string RollNo { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual string Department { get; set; }
    }

    public class StudentsMapping : ClassMap<Students>
    {
        public StudentsMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();
            Map(x=> x.RollNo);
            Map(x => x.Name);
            Map(x => x.DOB);
            Map(x => x.Department);
        }
    }
}