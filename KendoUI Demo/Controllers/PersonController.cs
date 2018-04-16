using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoUI_Demo.Models;
using System.Data.SqlClient;

namespace KendoUI_Demo.Controllers
{
    public class PersonController : Controller
    {
        private ContextClass db = new ContextClass();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Persons_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Person> persons = db.Persons;
            DataSourceResult result = persons.ToDataSourceResult(request, c => new PersonVM
            {
                ID = c.ID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Age = c.Age,
                DateOfBirth = c.DateOfBirth,
                Address = c.Address
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Persons_Create([DataSourceRequest]DataSourceRequest request, PersonVM person)
        {
            if (ModelState.IsValid)
            {
                var entity = new Person
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age,
                    DateOfBirth = person.DateOfBirth,
                    Address = person.Address
                };

                db.Persons.Add(entity);
                db.SaveChanges();
                person.ID = entity.ID;
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Persons_Update([DataSourceRequest]DataSourceRequest request, PersonVM person)
        {
            if (ModelState.IsValid)
            {
                var entity = new Person
                {
                    ID = person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age,
                    DateOfBirth = person.DateOfBirth,
                    Address = person.Address
                };

                db.Persons.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Persons_Destroy([DataSourceRequest]DataSourceRequest request, PersonVM person)
        {
            if (ModelState.IsValid)
            {
                var entity = new Person
                {
                    ID = person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age,
                    DateOfBirth = person.DateOfBirth,
                    Address = person.Address
                };

                db.Persons.Attach(entity);
                db.Persons.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { person }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpGet]
        public ActionResult AddPersonViaADO()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPersonViaADO(PersonVM personVM)
        {
            #region ADO Code for insertion in database

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ContextClass"].ConnectionString;

            #region ADO.net

            //string query = "INSERT INTO dbo.People (FirstName, Lastname, Age, DateOfBirth , Address , Phone) " +
            //       "VALUES (@FirstName, @LastName, @Age, @DateOfBirth , @Address , @Phone) ";

            //// create connection and command
            //using (SqlConnection cn = new SqlConnection(connectionString))
            //using (SqlCommand cmd = new SqlCommand(query, cn))
            //{
            //    // define parameters and their values
            //    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = personVM.FirstName;
            //    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = personVM.LastName;
            //    cmd.Parameters.Add("@Age", SqlDbType.Int).Value = personVM.Age;
            //    cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime, 50).Value = personVM.DateOfBirth;
            //    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = personVM.Address;
            //    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 50).Value = "52145214552";

            //    // open connection, execute INSERT, close connection
            //    cn.Open();
            //    cmd.ExecuteNonQuery();
            //    cn.Close();

            #endregion

            #region Stored Procedure

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("AddPerson", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@FirstName", personVM.FirstName));
                cmd.Parameters.Add(new SqlParameter("@LastName", personVM.LastName));
                cmd.Parameters.Add(new SqlParameter("@Age", personVM.Age));
                cmd.Parameters.Add(new SqlParameter("@DateOfBirth", personVM.DateOfBirth));
                cmd.Parameters.Add(new SqlParameter("@Phone", "214521456"));


                cmd.ExecuteNonQuery();

                // execute the command
                //using (SqlDataReader rdr = cmd.ExecuteReader())
                //{
                //    // iterate through results, printing each to console
                //    while (rdr.Read())
                //    {
                //        Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                //    }
                //}
            }

            #endregion

            #endregion

            return RedirectToAction("index");
        }

        public ActionResult GetPerson(int ID)
        {
            Person person = new Person();

            #region ADO Code for insertion in database

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ContextClass"].ConnectionString;

            #region Stored Procedure

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("GetPerson", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@ID", ID));

                //cmd.ExecuteNonQuery();

                //execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        person.Address = rdr["Address"].ToString();
                        person.Age = Convert.ToInt32(rdr["Age"].ToString());
                        person.DateOfBirth = DateTime.Parse(rdr["DateOfBirth"].ToString());
                        person.FirstName = rdr["FirstName"].ToString();
                        person.LastName = rdr["LastName"].ToString();
                        person.Phone = rdr["Phone"].ToString();
                        //Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                    }
                }
            }

            #endregion

            #endregion

            return View(person);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
