using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee GetById(Int64 id);
        Employee Insert(Employee model);
        bool Update(Employee model);
        void Delete(Employee model);
        void Save();
    }
}
