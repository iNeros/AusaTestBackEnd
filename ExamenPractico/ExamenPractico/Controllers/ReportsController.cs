using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace ExamenPractico.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("OrigenesExternos")]
    public class ReportsController : ControllerBase
    {
        public string dbConString;

        public ReportsController(IConfiguration configuration)
        {
            dbConString = configuration.GetConnectionString("dbContext").ToString();
        }

        [HttpGet]
        public ActionResult<object> ObtieneCamposPorFechas([FromHeader] string sFechaIni, [FromHeader] string sFechaFin)
        {
            //string connString = @"Server =azureausa.database.windows.net; Database = Pruebon; User Id=candidato;Password=3ch@l3Gan1t4zs;MultipleActiveResultSets=True";
            string connString = dbConString;
            //variables to store the query results
            List<string> nrreporte = new List<string>();
            List<string> svindice = new List<string>();
            List<string> svfhreporte = new List<string>();
            List<string> prnombre = new List<string>();
            List<string> prrfc = new List<string>();
            List<string> clnombre = new List<string>();
            List<string> clestatus = new List<string>();
            List<string> sunombre = new List<string>();
            List<string> susupervisoria = new List<string>();

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[getDataByDates]";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@FechaIni", sFechaIni);
                    cmd.Parameters.AddWithValue("@FechaFin", sFechaFin);

                    //set the SqlCommand type to stored procedure and execute
                    

                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;


                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            nrreporte.Add(dr["nrreporte"].ToString());
                            svindice.Add(dr["svindice"].ToString());
                            svfhreporte.Add(dr["svfhreporte"].ToString());
                            prnombre.Add(dr["prnombre"].ToString());
                            prrfc.Add(dr["prrfc"].ToString());
                            clnombre.Add(dr["clnombre"].ToString());
                            clestatus.Add(dr["clestatus"].ToString());
                            sunombre.Add(dr["sunombre"].ToString());
                            susupervisoria.Add(dr["susupervisoria"].ToString());
                        }
                    }
                    else
                    {
                        return "NO DATA FOUND";
                    }

                    //close data reader
                    dr.Close();
                    conn.Close();

                    List<List<string>> result = new List<List<string>>() { nrreporte, svindice, svfhreporte, prnombre, prrfc, clnombre, clestatus, sunombre, susupervisoria };

                    //Array< List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>> result =
                    //    new Array <List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>> 
                    //    (nrreporte, svindice, svfhreporte, prnombre, prrfc, clnombre, clestatus, sunombre, susupervisoria);
 
                    return result;

                    //close connection
                    
                }

            }
            catch (Exception ex)
            {
                return "Error: "+ex;
            }
           

        }
    }

}
