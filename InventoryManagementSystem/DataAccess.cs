using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace InventoryManagementSystem
{
    public class DataAccess : IDisposable
    {
        public SqlConnection sqlcon;
        private SqlCommand sqlcom;
        private SqlDataAdapter sda;
        private DataSet ds;

        // Constructor
        public DataAccess()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;
            this.sqlcon = new SqlConnection(connectionString);
        }

        // Method to open the connection
        private void OpenConnection()
        {
            if (this.sqlcon.State == ConnectionState.Closed || this.sqlcon.State == ConnectionState.Broken)
            {
                this.sqlcon.Open();
            }
        }

        // Method to close the connection
        private void CloseConnection()
        {
            if (this.sqlcon.State == ConnectionState.Open)
            {
                this.sqlcon.Close();
            }
        }

        // Method to begin a transaction
        public SqlTransaction BeginTransaction()
        {
            OpenConnection();
            return sqlcon.BeginTransaction();
        }

        // Method to execute select queries
        public DataSet ExecuteQuery(string sql)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    sda = new SqlDataAdapter(sqlcom);
                    ds = new DataSet();
                    sda.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute select queries with parameters
        public DataSet ExecuteQuery(string sql, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        // Handle image parameters specially
                        foreach (var param in parameters)
                        {
                            if (param.SqlDbType == SqlDbType.Image || param.Value is byte[])
                            {
                                // Ensure proper handling of image parameters
                                param.SqlDbType = SqlDbType.Image;
                            }
                            sqlcom.Parameters.Add(param);
                        }
                    }
                    sda = new SqlDataAdapter(sqlcom);
                    ds = new DataSet();
                    sda.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute select queries and return a DataTable
        public DataTable ExecuteQueryTable(string sql)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    sda = new SqlDataAdapter(sqlcom);
                    ds = new DataSet();
                    sda.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute insert, update, delete queries
        public int ExecuteDMLQuery(string sql)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    return sqlcom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing DML query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute insert, update, delete queries with parameters
        public int ExecuteDMLQuery(string sql, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing parameterized DML query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute parameterized queries
        public int ExecuteParameterizedQuery(string sql, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing parameterized query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // New method to execute scalar queries
        public object ExecuteScalarQuery(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing scalar query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute scalar queries with transaction support
        public object ExecuteScalarQuery(string sql, SqlTransaction transaction, SqlParameter[] parameters = null)
        {
            try
            {
                using (sqlcom = new SqlCommand(sql, sqlcon, transaction))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing scalar query with transaction: " + ex.Message);
            }
        }

        // Method to execute non-query SQL commands (insert, update, delete) with parameters
        public int ExecuteNonQuery(string sql, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (sqlcom = new SqlCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing non-query: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Method to execute non-query SQL commands (insert, update, delete) with transaction support
        public int ExecuteNonQuery(string sql, SqlTransaction transaction, SqlParameter[] parameters = null)
        {
            try
            {
                using (sqlcom = new SqlCommand(sql, sqlcon, transaction))
                {
                    if (parameters != null)
                    {
                        sqlcom.Parameters.AddRange(parameters);
                    }
                    return sqlcom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing non-query with transaction: " + ex.Message);
            }
        }


        // Implement IDisposable to properly dispose of the SqlConnection
        public void Dispose()
        {
            if (sqlcon != null)
            {
                sqlcon.Dispose();
            }
        }
    }
}
