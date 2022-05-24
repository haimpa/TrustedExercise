using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TrustedPartners.DataAccessLayer
{
    public class DataAccess
    {
        private static string ConnString = ConfigurationManager.ConnectionStrings["TrustedPartners"].ConnectionString;
    
        //The AGENT_CODE who has the highest sum of ADVANCE_AMOUNT in the first quarter of the specific year sent in the parameter
        //The result comes already in JSON format from SQL Server DB
        public string HighestAgent(int year)
        {
            using (SqlConnection con = new SqlConnection(ConnString))            
                using (SqlCommand cmd = new SqlCommand("HighestAgentFistQuarter", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@year", SqlDbType.Int).Value = year;
                        con.Open();
                        var response = (string)cmd.ExecuteScalar();

                        return response;
                    }
                    catch(Exception e)
                    {
                        return e.Message; //Failed
                    }
                    finally
                    {
                        con.Close();
                    }
                }
        }
        public string GetListOfAgents(string[] agents, int number)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand cmd = new SqlCommand("GetListOfAgents", con))
            {
                try
                {
                    var agentList = string.Join(',', agents);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@agentList", SqlDbType.NVarChar).Value = agentList;
                    cmd.Parameters.AddWithValue("@number", SqlDbType.Int).Value = number;
                    con.Open();
                    var response = (string)cmd.ExecuteScalar();

                    return response;
                }
                catch (Exception e)
                {
                    return e.Message; //Failed
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public string GetTopAgents(int number, int year)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand cmd = new SqlCommand("GetTopAgents", con))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@number", SqlDbType.Int).Value = number;
                    cmd.Parameters.AddWithValue("@year", SqlDbType.Int).Value = year;
                    con.Open();
                    var response = (string)cmd.ExecuteScalar();

                    return response;
                }
                catch (Exception e)
                {
                    return e.Message; //Failed
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
