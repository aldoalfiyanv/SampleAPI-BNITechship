using Microsoft.Extensions.Configuration;
using SampleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SampleAPI.Data
{
    public class StudentDataSQL : IStudent
    {

        private IConfiguration _config;
        public StudentDataSQL(IConfiguration config)
        {
            _config = config;
        }

        private string GetConnStr()
        {
            return _config.GetConnectionString("DefaultConnection");
        }

        public void Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"delete from Students where ID=@ID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    var student = GetById(id);
                    if (student != null)
                    {
                        conn.Open();
                        var result = cmd.ExecuteNonQuery();
                        if (result != 1)
                            throw new Exception("Gagal delete data student");
                    }
                    else
                    {
                        throw new Exception($"Data student id: {id} tidak ditemukan");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.InnerException.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Student> GetAll()
        {
            List<Student> lstStudents = new List<Student>();
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"select * from Students 
                                  order by FirstMidName asc";
                SqlCommand cmd = new SqlCommand(strSql, conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lstStudents.Add(new Student
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstMidName = dr["FirstMidName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            EnrollmentDate = Convert.ToDateTime(dr["EnrollmentDate"])
                        });
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return lstStudents;
        }

        public Student GetById(string id)
        {
            Student student = new Student();
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"select * from Students
                                  where ID=@ID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    student = new Student
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        FirstMidName = dr["FirstMidName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        EnrollmentDate = Convert.ToDateTime(dr["EnrollmentDate"])
                    };
                }
                else
                {
                    throw new Exception($"Data Student ID: {id} tidak ditemukan");
                }

                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return student;
        }

        public IEnumerable<Student> GetByName(string studentName)
        {
            List<Student> lstStudents = new List<Student>();
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"select * from Students where FirstMidName like @FirstMidName 
                                  OR LastName like @LastName order by FirstMidName asc";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@FirstMidName", "%" + studentName + "%");
                cmd.Parameters.AddWithValue("@LastName", "%" + studentName + "%");
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lstStudents.Add(new Student
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstMidName = dr["FirstMidName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            EnrollmentDate = Convert.ToDateTime(dr["EnrollmentDate"])
                        });
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return lstStudents;
            }
        }

        public void Insert(Student obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"insert into Students(LastName,FirstMidName,EnrollmentDate) 
                                  values(@LastName,@FirstMidName,@EnrollmentDate)";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                cmd.Parameters.AddWithValue("@FirstMidName", obj.FirstMidName);
                cmd.Parameters.AddWithValue("@EnrollmentDate", obj.EnrollmentDate);
                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result != 1)
                        throw new Exception("Gagal untuk menambahkan data student");
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception($"Error: {sqlEx.InnerException.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public int InsertWithIndentity(Student obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"insert into Students(LastName,FirstMidName,EnrollmentDate) 
                                  values(@LastName,@FirstMidName,@EnrollmentDate);
                                  select @@identity";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                cmd.Parameters.AddWithValue("@FirstMidName", obj.FirstMidName);
                cmd.Parameters.AddWithValue("@EnrollmentDate", obj.EnrollmentDate);
                try
                {
                    conn.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception($"Error: {sqlEx.InnerException.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void Update(string id, Student obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                string strSql = @"update Students set LastName=@LastName,FirstMidName=@FirstMidName,
                                  EnrollmentDate=@EnrollmentDate where ID=@ID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                cmd.Parameters.AddWithValue("@FirstMidName", obj.FirstMidName);
                cmd.Parameters.AddWithValue("@EnrollmentDate", obj.EnrollmentDate);
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    var student = GetById(id);
                    if (student != null)
                    {
                        conn.Open();
                        var result = cmd.ExecuteNonQuery();
                        if (result != 1)
                            throw new Exception("Gagal update data");
                    }
                    else
                    {
                        throw new Exception($"Data Student ID {id} tidak ditemukan");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception($"Error: {sqlEx.InnerException.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }
    }
}