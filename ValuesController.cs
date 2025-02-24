using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        // GET api/values - Fetch all employees
        public HttpResponseMessage Get()
        {
            List<Employee> employees = new List<Employee>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", con);
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Active = Convert.ToInt32(reader["Active"])
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error fetching employees: " + ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, employees);
        }

        // GET api/values/5 - Fetch a specific employee by Id
        public HttpResponseMessage Get(int id)
        {
            Employee employee = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Active = Convert.ToInt32(reader["Active"])
                    };
                }
                con.Close();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error fetching employee: " + ex.Message);
            }

            if (employee == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee not found");

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // POST api/values - Create a new employee
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            string msg = "";
            if (employee != null)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Employees (Name, Age, Active) VALUES (@Name, @Age, @Active)", con);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Active", employee.Active);

                try
                {
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    msg = i > 0 ? "Employee has been successfully inserted." : "Error inserting employee.";
                }
                catch (Exception ex)
                {
                    msg = "Error: " + ex.Message;
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { message = msg });
        }

        // PUT api/values/5 - Update an existing employee
        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            string msg = "";
            if (employee != null)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Employees SET Name = @Name, Age = @Age, Active = @Active WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Active", employee.Active);

                try
                {
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    msg = i > 0 ? "Employee has been successfully updated." : "Error while updating.";
                }
                catch (Exception ex)
                {
                    msg = "Error: " + ex.Message;
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { message = msg });
        }

        // DELETE api/values/5 - Delete an employee
        public HttpResponseMessage Delete(int id)
        {
            string msg = "";
            SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                msg = i > 0 ? "Employee has been successfully deleted." : "Error while deleting.";
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { message = msg });
        }
    }
}
