using Entity;
using Newtonsoft.Json;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApi1.Helpers;
using WebApi1.Models;
using System.Net.Mail;

namespace WebApi1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DefaultController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        public DefaultController(IEmployeeService empRepos)
        {
            _employeeService = empRepos;
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            var data = _employeeService.GetAll();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        public IHttpActionResult GetById(int id)
        {
            var data = _employeeService.GetById(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);

        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult PostProduct(Employee item)
        {
            _employeeService.Insert(item);
            _employeeService.Save();

            return Ok(item);
        }
                
        [System.Web.Http.HttpPut]
        public IHttpActionResult Put(Employee model)
        {

            if (!_employeeService.Update(model))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _employeeService.Save();
            return Ok("Successsful");
        }

        [System.Web.Http.HttpDelete]
        public void Delete(int id)
        {
            var emp = _employeeService.GetById(id);
            _employeeService.Delete(emp);
            _employeeService.Save();

        }

        //backend pagination api
        //public IEnumerable<Employee> GetAllProducts([FromUri]PagingParameterModel pagingparametermodel)
        //{ 

        //    // Return List of Customer  
        //    var source = (from prod in _employeeService.GetAll().
        //                    OrderBy(a => a.UserId)
        //                  select prod).AsQueryable();


        //    // ------------------------------------ Search Parameter-------------------   

        //    if (!string.IsNullOrEmpty(pagingparametermodel.QuerySearch))
        //    {
        //        source = source.Where(a => a.First_Name.Contains(pagingparametermodel.QuerySearch));
        //    }

        //    // ------------------------------------ Search Parameter-------------------  


        //    // Get's No of Rows Count   
        //    int count = source.Count();

        //    // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
        //    int CurrentPage = pagingparametermodel.pageNumber;

        //    // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
        //    int PageSize = pagingparametermodel.pageSize;

        //    // Display TotalCount to Records to User  
        //    int TotalCount = count;

        //    // Calculating Totalpage by Dividing (No of Records / Pagesize)  
        //    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

        //    // Returns List of Customer after applying Paging   
        //    var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

        //    // if CurrentPage is greater than 1 means it has previousPage  
        //    var previousPage = CurrentPage > 1 ? "Yes" : "No";

        //    // if TotalPages is greater than CurrentPage means it has nextPage  
        //    var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

        //    // Object which we are going to send in header   
        //    var paginationMetadata = new
        //    {
        //        totalCount = TotalCount,
        //        pageSize = PageSize,
        //        currentPage = CurrentPage,
        //        totalPages = TotalPages,
        //        previousPage,
        //        nextPage,

        //        QuerySearch = string.IsNullOrEmpty(pagingparametermodel.QuerySearch) ?
        //              "No Parameter Passed" : pagingparametermodel.QuerySearch

        //    };

        //    // Setting Header  
        //    HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
        //    // Returing List of Customers Collections  
        //    return items;

        //}

        //public IHttpActionResult Get(string sort)
        //{
        //    // Convert data source into IQueryable
        //    // ApplySort method needs IQueryable data source hence we need to convert it
        //    // Or we can create ApplySort to work on list itself
        //    var data = (from prod in _employeeService.GetAll().
        //                       OrderBy(a => a.UserId)
        //                select prod).AsQueryable();

        //    // Apply sorting
        //    data = data.ApplySort(sort);

        //    // Return response
        //    return Ok(data);
        //}
    }

}
