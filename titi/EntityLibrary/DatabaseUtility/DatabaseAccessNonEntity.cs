using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Shared;
using System.Linq;
using System.Web;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
using EntityLibrary;

namespace DatabaseUtility.EntityLibrary
{
    /// <summary>
    /// The Library is using to access database
    /// </summary>
    public class DatabaseAccessNonEntity
    {
        public static string connectionString = GetConnectionString();
        public static string connectionStringEntity = GetConnectionStringEntity();
        public static string GetConnectionStringEntity()
        {
            return descryptConnectionString();
        }
        public static string descryptConnectionString()
        {
            return @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=DESKTOP-38JCVBS\SQLSERVER2014;initial catalog=Titi;user id=sa;password=123456;multipleactiveresultsets=True;application name=EntityFramework';";
            //return @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=MB2D208\MSSQLSERVER2012;initial catalog=hud17752_ktshophouse;user id=hud17752_ktshophouse;password=123456@123456;MultipleActiveResultSets=True;application name=EntityFramework';";
            //return @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=DESKTOP-38JCVBS\MSSQL2012;initial catalog=Titi;user id=sa;password=123456;MultipleActiveResultSets=True;application name=EntityFramework';";
        }
        public static string GetConnectionString()
        {
            string descrypt = @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=.\SQLSERVER2014;initial catalog=Titi;persist security info=True;user id=sa;password=123456;multipleactiveresultsets=True;application name=EntityFramework';";
            //string descrypt = @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=MB2D208\MSSQLSERVER2012;initial catalog=hud17752_ktshophouse;persist security info=True;user id=hud17752_ktshophouse;password=123456@123456;MultipleActiveResultSets=True;application name=EntityFramework';";
            //string descrypt = @"metadata=res://*/EntityLibContext.csdl|res://*/EntityLibContext.ssdl|res://*/EntityLibContext.msl;provider=System.Data.SqlClient;provider connection string=';data source=DESKTOP-38JCVBS\MSSQL2012;initial catalog=Titi;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True;application name=EntityFramework';";
            string connectionString_ = descrypt.Substring(descrypt.IndexOf("data source"));

            var arr_ = connectionString_.Split(';');

            return arr_[0] + "; " + arr_[1] + "; " + arr_[3] + "; " + arr_[4];
        }
        
        /// <summary>
        /// Search by store with condition search, and return dataset
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data set</returns>
        public static DataSet SearchStoreProcedure(string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        public static DataSet SearchStoreProcedure(string _connectionString, string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(_connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Search by store with condition search, and return dataset
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="commandTimeout">CommandTimeout</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data set</returns>
        public static DataSet SearchStoreProcedure(string storeProcedureName, int commandTimeout, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandTimeout = commandTimeout;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Search by store with condition search, and return dataset
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <param name="outputParameters">List output paramaters</param>
        /// <returns>Data set</returns>
        public static DataSet SearchStoreProcedure(string storeProcedureName, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            // Input Parameters
            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            // Output Parameters
            if (outputParameters != null && outputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in outputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, ParameterDirection.Output, param.ParameterName, param.SqlDbType, param.Size, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);

                // return out parameters.
                if (outputParameters != null)
                {
                    foreach (SqlDBParameter outParam in outputParameters)
                    {
                        SqlParameter param = cmm.Parameters[outParam.ParameterName];
                        outParam.Value = param.Value;
                        outParam.SqlDbType = param.SqlDbType;
                        outParam.Size = param.Size;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Search by store with condition search, and return dataset
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <param name="outputParameters">List output paramaters</param>
        /// <returns>Data set</returns>
        public static DataSet SearchStoreProcedure(string storeProcedureName, int commandTimeout, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandTimeout = commandTimeout;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            // Input Parameters
            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            // Output Parameters
            if (outputParameters != null && outputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in outputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, ParameterDirection.Output, param.ParameterName, param.SqlDbType, param.Size, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);

                // return out parameters.
                if (outputParameters != null)
                {
                    foreach (SqlDBParameter outParam in outputParameters)
                    {
                        SqlParameter param = cmm.Parameters[outParam.ParameterName];
                        outParam.Value = param.Value;
                        outParam.SqlDbType = param.SqlDbType;
                        outParam.Size = param.Size;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Search by store with condition search, and return datatable
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data table</returns>
        public static DataTable SearchStoreProcedureDataTable(string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            DataSet ds = SearchStoreProcedure(storeProcedureName, inputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public static DataTable SearchStoreProcedureDataTable(string _connectionString, string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            DataSet ds = SearchStoreProcedure(_connectionString,storeProcedureName, inputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by store with condition search, and return datatable
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data table</returns>
        public static DataTable SearchStoreProcedureDataTable(string storeProcedureName, int commandTimeout, params SqlDBParameter[] inputParameters)
        {
            DataSet ds = SearchStoreProcedure(storeProcedureName, commandTimeout, inputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by store with condition search, and return datatable
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <param name="outputParameters">List output paramaters</param>
        /// <returns>Data table</returns>
        public static DataTable SearchStoreProcedureDataTable(string storeProcedureName, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            DataSet ds = SearchStoreProcedure(storeProcedureName, inputParameters, outputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by store with condition search, and return datatable
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Condition search</param>
        /// <param name="outputParameters">List output paramaters</param>
        /// <returns>Data table</returns>
        public static DataTable SearchStoreProcedureDataTable(string storeProcedureName, int commandTimeout, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            DataSet ds = SearchStoreProcedure(storeProcedureName, commandTimeout, inputParameters, outputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by comment text with condition search, and return dataset
        /// </summary>
        /// <param name="commandText">Comand text to search</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data set</returns>
        public static DataSet SearchCommandText(string commandText, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(commandText);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.Text;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);

            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Search by comment text with condition search, and return dataset
        /// </summary>
        /// <param name="commandText">Comand text to search</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data set</returns>
        public static DataSet SearchCommandText(string commandText, int commandTimeout, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(commandText);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.Text;
            cmm.CommandTimeout = commandTimeout;
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = cmm;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            DataSet result = new DataSet();
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                adp.Fill(result);

            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Search by command text with condiction search, and return datatable
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data table</returns>
        public static DataTable SearchCommandTextDataTable(string commandText, params SqlDBParameter[] inputParameters)
        {
            DataSet ds = SearchCommandText(commandText, inputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by command text with condiction search, and return datatable
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>Data table</returns>
        public static DataTable SearchCommandTextDataTable(string commandText, int commandTimeout, params SqlDBParameter[] inputParameters)
        {
            DataSet ds = SearchCommandText(commandText, commandTimeout, inputParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Search by command text with condiction search and return List data base on type of T
        /// </summary>
        /// <typeparam name="T">The type of element in the list</typeparam>
        /// <param name="commandText">Comand text to search</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>List data with type is T</returns>
        public static List<T> SearchCommandObjects<T>(string commandText, params SqlDBParameter[] inputParameters)
        {
            List<T> result = new List<T>();
            Type myType = typeof(T);
            if (myType != null)
            {
                SqlCommand cmm = new SqlCommand(commandText);
                cmm.Connection = new SqlConnection(connectionString);
                cmm.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmm;

                if (inputParameters != null && inputParameters.Length > 0)
                {
                    foreach (SqlDBParameter param in inputParameters)
                    {
                        if (param != null)
                        {
                            SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                        }
                    }
                }

                try
                {
                    if (cmm.Connection.State != ConnectionState.Open)
                        cmm.Connection.Open();

                    IDataReader dre = cmm.ExecuteReader();

                    List<Type> originalTypes = new List<Type>(new Type[] { typeof(int), typeof(string), typeof(DateTime), typeof(double) });
                    if (originalTypes.Contains(myType))
                    {
                        while (dre.Read())
                        {
                            object obj = dre.GetValue(0);
                            result.Add((T)obj);
                        }
                    }
                    else
                    {
                        FieldInfo[] myFieldInfo;

                        // Get the type and fields of FieldInfoClass.
                        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                        if (myFieldInfo.Length <= dre.FieldCount)
                        {
                            while (dre.Read())
                            {
                                object objNew = System.Activator.CreateInstance(typeof(T), true);
                                SetValueForObject(dre, myFieldInfo, objNew);
                                result.Add((T)objNew);
                            }
                        }
                        else
                        {
                            while (dre.Read())
                            {
                                object objNew = System.Activator.CreateInstance(typeof(T), true);
                                SetValueForObject(dre, myType, objNew);
                                result.Add((T)objNew);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = null;
                }
                finally
                {
                    if (cmm.Connection.State != ConnectionState.Closed)
                    {
                        cmm.Connection.Close();
                    }
                    cmm.Connection.Dispose();
                    cmm.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// Search by command text with condiction search and return List data base on type of T
        /// </summary>
        /// <typeparam name="T">The type of element in the list</typeparam>
        /// <param name="commandText">Comand text to search</param>
        /// <param name="inputParameters">Condition search</param>
        /// <returns>List data with type is T</returns>
        public static List<T> SearchStoreProcedure<T>(string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            List<T> result = new List<T>();
            Type myType = typeof(T);
            if (myType != null)
            {
                SqlCommand cmm = new SqlCommand(storeProcedureName);
                cmm.Connection = new SqlConnection(connectionString);
                cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmm;

                if (inputParameters != null && inputParameters.Length > 0)
                {
                    foreach (SqlDBParameter param in inputParameters)
                    {
                        if (param != null)
                        {
                            SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                        }
                    }
                }

                try
                {
                    if (cmm.Connection.State != ConnectionState.Open)
                    {
                        cmm.Connection.Open();
                    }

                    IDataReader dre = cmm.ExecuteReader();

                    FieldInfo[] myFieldInfo;

                    // Get the type and fields of FieldInfoClass.
                    myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                    if (myFieldInfo.Length <= dre.FieldCount)
                    {
                        while (dre.Read())
                        {
                            object objNew = System.Activator.CreateInstance(typeof(T), true);
                            SetValueForObject(dre, myFieldInfo, objNew);
                            result.Add((T)objNew);
                        }
                    }
                    else
                    {
                        while (dre.Read())
                        {
                            object objNew = System.Activator.CreateInstance(typeof(T), true);
                            SetValueForObject(dre, myType, objNew);
                            result.Add((T)objNew);
                        }
                    }

                }
                catch (Exception ex)
                {
                    result = null;
                }
                finally
                {
                    if (cmm.Connection.State != ConnectionState.Closed)
                    {
                        cmm.Connection.Close();
                    }
                    cmm.Connection.Dispose();
                    cmm.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// Execute by store with input and outupt params
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Input params</param>
        /// <param name="outputParameters">Output params</param>
        public static void ExecuteStoreProcedure(string storeProcedureName, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            // Output Parameters
            if (outputParameters != null && outputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in outputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, ParameterDirection.Output, param.ParameterName, param.SqlDbType, param.Size, param.Value);
                    }
                }
            }
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                cmm.ExecuteNonQuery();

                if (outputParameters != null && outputParameters.Length > 0)
                {
                    foreach (SqlDBParameter outParam in outputParameters)
                    {
                        if (outParam != null)
                        {
                            SqlParameter param = cmm.Parameters[outParam.ParameterName];
                            outParam.Value = param.Value;
                            outParam.SqlDbType = param.SqlDbType;
                            outParam.Size = param.Size;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
        }

        /// <summary>
        /// Execute by store with input and outupt params
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Input params</param>
        /// <param name="outputParameters">Output params</param>
        public static void ExecuteStoreProcedure(string storeProcedureName, int commandTimeOut, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandTimeout = commandTimeOut;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            // Output Parameters
            if (outputParameters != null && outputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in outputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, ParameterDirection.Output, param.ParameterName, param.SqlDbType, param.Size, param.Value);
                    }
                }
            }
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                cmm.ExecuteNonQuery();

                if (outputParameters != null && outputParameters.Length > 0)
                {
                    foreach (SqlDBParameter outParam in outputParameters)
                    {
                        if (outParam != null)
                        {
                            SqlParameter param = cmm.Parameters[outParam.ParameterName];
                            outParam.Value = param.Value;
                            outParam.SqlDbType = param.SqlDbType;
                            outParam.Size = param.Size;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
        }

        /// <summary>
        /// Execute by store with input and outupt params
        /// </summary>
        /// <param name="ExecuteStoreProcedureTransaction">Store name</param>
        /// <param name="commandTimeOut">Input params</param>
        /// <param name="inputParameters">Input params</param>
        /// <param name="outputParameters">Output params</param>
        public static void ExecuteStoreProcedureTransaction(SqlConnection connection, SqlTransaction tran, string storeProcedureName, int commandTimeOut, SqlDBParameter[] inputParameters, params SqlDBParameter[] outputParameters)
        {
            try {
                SqlCommand command = new SqlCommand(storeProcedureName, connection, tran);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeOut;

                if (inputParameters != null && inputParameters.Length > 0)
                {
                    foreach (SqlDBParameter param in inputParameters)
                    {
                        if (param != null)
                        {
                            SqlDataParameter.AddParameter(command, param.ParameterName, param.SqlDbType, param.Value);
                        }
                    }
                }

                // Output Parameters
                if (outputParameters != null && outputParameters.Length > 0)
                {
                    foreach (SqlDBParameter param in outputParameters)
                    {
                        if (param != null)
                        {
                            SqlDataParameter.AddParameter(command, ParameterDirection.Output, param.ParameterName, param.SqlDbType, param.Size, param.Value);
                        }
                    }
                }

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery();

                if (outputParameters != null && outputParameters.Length > 0)
                {
                    foreach (SqlDBParameter outParam in outputParameters)
                    {
                        if (outParam != null)
                        {
                            SqlParameter param = command.Parameters[outParam.ParameterName];
                            outParam.Value = param.Value;
                            outParam.SqlDbType = param.SqlDbType;
                            outParam.Size = param.Size;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// Executes the query, and returns the first column of the first row in the
        /// result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="commantText">Command Text</param>
        /// <param name="inputParameters">Input params</param>
        /// <returns>object</returns>
        public static object ExecuteScalarCommandText(string commantText, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(commantText);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.Text;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            object result = null;
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                result = cmm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Execute store with input params, and return bool
        /// </summary>
        /// <param name="storeProcedureName">Store name</param>
        /// <param name="inputParameters">Input params</param>
        /// <returns>True if success, False if error</returns>
        public static bool ExecuteStoreProcedure(string storeProcedureName, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(storeProcedureName);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.StoredProcedure;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            bool result = false;
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                int i = cmm.ExecuteNonQuery();
                result = true;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Execute comand text with input params, and return bool
        /// </summary>
        /// <param name="commantText">Comand text</param>
        /// <param name="inputParameters">Inpurt param</param>
        /// <returns>True if success, False if error</returns>
        public static bool ExecuteCommandText(string commantText, params SqlDBParameter[] inputParameters)
        {
            SqlCommand cmm = new SqlCommand(commantText);
            cmm.Connection = new SqlConnection(connectionString);
            cmm.CommandType = CommandType.Text;

            if (inputParameters != null && inputParameters.Length > 0)
            {
                foreach (SqlDBParameter param in inputParameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(cmm, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            bool result = false;
            try
            {
                if (cmm.Connection.State != ConnectionState.Open)
                {
                    cmm.Connection.Open();
                }
                cmm.ExecuteNonQuery();
                result = true;
            }
            finally
            {
                if (cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Connection.Close();
                }
                cmm.Connection.Dispose();
                cmm.Dispose();
            }

            return result;
        }

        public static bool UpdateDataTable(DataTable dataSource, string commandText, params SqlDBParameter[] parameters)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = new SqlConnection(connectionString);
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (SqlDBParameter param in parameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(command, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(dataSource);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                }
                command.Connection.Dispose();
                command.Dispose();
            }
        }

        public static bool UpdateDataTable(DataRow[] rows, string commandText, params SqlDBParameter[] parameters)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = new SqlConnection(connectionString);
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (SqlDBParameter param in parameters)
                {
                    if (param != null)
                    {
                        SqlDataParameter.AddParameter(command, param.ParameterName, param.SqlDbType, param.Value);
                    }
                }
            }
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(rows);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                }
                command.Connection.Dispose();
                command.Dispose();
            }
        }

        #region Private Methods

        private static void SetValueForObject(IDataReader dre, Type type, object obj)
        {
            for (int i = 0; i < dre.FieldCount; i++)
            {
                //get type of field
                string fieldName = dre.GetName(i);
                PropertyInfo property = type.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    continue;
                }
                SetValue(property, dre, obj);
            }
        }

        /// <summary>
        /// Get Original Name for type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetNameFieldInfo(string name)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"\<(.*?)\>");
            var matches = regex.Matches(name);
            var res = string.Join(";", matches.Cast<System.Text.RegularExpressions.Match>().Select(x => x.Value.Replace("<", "").Replace(">", "")).ToArray());

            return res;
        }

        // Set value from database for fields of the object
        private static void SetValueForObject(IDataReader dre, FieldInfo[] myFieldInfo, object obj)
        {
            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                //get type of field
                Type type = myFieldInfo[i].FieldType;
                string strParamName = GetNameFieldInfo(myFieldInfo[i].Name);

                // convert value to DB value
                object value = DBConvert.ParseToDBValue(myFieldInfo[i].GetValue(obj));

                // if type is char
                if (type == typeof(System.Char))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToChar(dre, strParamName));
                }
                // if type is string
                else if (type == typeof(System.String))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToString(dre, strParamName));
                }
                // if type is smallint
                else if (type == typeof(System.Int16))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToSmallInt(dre, strParamName));
                }
                // if type is int
                else if (type == typeof(System.Int32))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToInt(dre, strParamName));
                }
                // if type is long
                else if (type == typeof(System.Int64))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToLong(dre, strParamName));
                }
                // if type is decimal
                else if (type == typeof(System.Decimal))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToDecimal(dre, strParamName));
                }
                // if type is double
                else if (type == typeof(System.Double))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToDouble(dre, strParamName));
                }
                // if type is datetime
                else if (type == typeof(System.DateTime))
                {
                    myFieldInfo[i].SetValue(obj, DBConvert.ParseDBToDateTime(dre, strParamName));
                }
            }
        }

        private static void SetValue(PropertyInfo field, IDataReader dre, object obj)
        {
            // if type is char
            if (field.PropertyType == typeof(System.Char))
            {
                field.SetValue(obj, DBConvert.ParseDBToChar(dre, field.Name), null);
            }
            // if type is string
            else if (field.PropertyType == typeof(System.String))
            {
                field.SetValue(obj, DBConvert.ParseDBToString(dre, field.Name), null);
            }
            // if type is smallint
            else if (field.PropertyType == typeof(System.Int16))
            {
                field.SetValue(obj, DBConvert.ParseDBToSmallInt(dre, field.Name), null);
            }
            // if type is int
            else if (field.PropertyType == typeof(System.Int32))
            {
                field.SetValue(obj, DBConvert.ParseDBToInt(dre, field.Name), null);
            }
            // if type is long
            else if (field.PropertyType == typeof(System.Int64))
            {
                field.SetValue(obj, DBConvert.ParseDBToLong(dre, field.Name), null);
            }
            // if type is decimal
            else if (field.PropertyType == typeof(System.Decimal))
            {
                field.SetValue(obj, DBConvert.ParseDBToDecimal(dre, field.Name), null);
            }
            // if type is double
            else if (field.PropertyType == typeof(System.Double))
            {
                field.SetValue(obj, DBConvert.ParseDBToDouble(dre, field.Name), null);
            }
            // if type is datetime
            else if (field.PropertyType == typeof(System.DateTime))
            {
                field.SetValue(obj, DBConvert.ParseDBToDateTime(dre, field.Name), null);
            }
        }
        #endregion Pethods
    }
}
