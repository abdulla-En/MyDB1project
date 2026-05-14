using MyDBproject.DAL;
using MyDBproject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyDBproject.BLL
{
    public class AppService
    {
        // INSERT Student
        public string AddStudent(Student st)
        {
            string checkSql = @"SELECT * from student s where s.ST_EMAIL = @em";
            SqlParameter[] checkParams = { new SqlParameter("@em", st.Email) };
            DataTable dt = Databasehelper.ExecuteQuery(checkSql, checkParams);
            if (dt.Rows.Count > 0)
            {
                return $"Error: this email is already registerd  {st.Email}";
            }

            string sql = "INSERT INTO STUDENT (ST_FIRST_NAME, ST_LAST_NAME, ST_EMAIL ) VALUES (@fn, @ln, @em)";
            SqlParameter[] ps = { new SqlParameter("@fn", st.FirstName), new SqlParameter("@ln", st.LastName), new SqlParameter("@em", st.Email)};
            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Student Added" : "Error: Operation Failed";
        }

        // INSERT Program
        public string AddProgram(ProgramModel pm)
        {
            string sql = "INSERT INTO PROGRAM (TITLE, CATEGORY_ID, INSTRUCTOR_ID, DIFFICULTY, FEE ) VALUES (@t, @c, @i, @d, @f)";
            SqlParameter[] ps = {
                new SqlParameter("@t", pm.Title), new SqlParameter("@c", pm.CategoryId),
                new SqlParameter("@i", pm.InstructorId), new SqlParameter("@d", pm.Difficulty),
                new SqlParameter("@f", pm.Fee) , 
               
            };
            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Program Added" : "Error: Operation Failed";
        }

        // UPDATE Fee
        public string UpdateFee(int progId, decimal newFee)
        {
            string sql = "UPDATE PROGRAM SET FEE = @f WHERE PROGRAM_ID = @id";
            SqlParameter[] ps = { new SqlParameter("@f", newFee), new SqlParameter("@id", progId) };
            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Fee Updated" : "Error: ID not found";
        }

        // UPDATE Progress(Condition: units_completed <= TotalUnits)
        public string UpdateProgress(int enrollId, int units)
        {
           
            string checkSql = @"SELECT COUNT(*) 
                        FROM UNITS U 
                        JOIN ENROLLMENTS E ON U.PROGRAM_ID = E.PROGRAM_ID 
                        WHERE E.ENROLLMENT_ID = @id";

            SqlParameter[] checkParams = { new SqlParameter("@id", enrollId) };
            DataTable dt = Databasehelper.ExecuteQuery(checkSql, checkParams); 

            if (dt.Rows.Count > 0)
            {
                int totalUnits = Convert.ToInt32(dt.Rows[0][0]);
                if (units > totalUnits)
                {
                    return $"Error: Max units for this program is {totalUnits}";
                }
            }

            string sql = "UPDATE ENROLLMENTS SET UNITS_COMPLETED = @u WHERE ENROLLMENT_ID = @id";
            SqlParameter[] ps = {
        new SqlParameter("@u", units),
        new SqlParameter("@id", enrollId)
    };

            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Progress Updated" : "Error: ID not found";
        }

        // DELETE Enrollment (Condition: units_completed = 0)
        public string DeleteEnrollment(int enrollId)
        {
            string sql = "DELETE FROM ENROLLMENTS WHERE ENROLLMENT_ID = @id AND UNITS_COMPLETED = 0";
            SqlParameter[] ps = { new SqlParameter("@id", enrollId) };
            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Enrollment Deleted" : "Error: Condition not met or ID not found";
        }

        // DELETE Unit
        public string DeleteUnit(int progId, int seqNum)
        {
            string sql = "DELETE FROM UNITS WHERE PROGRAM_ID = @pid AND SEQUENCE_NO = @sn";
            SqlParameter[] ps = { new SqlParameter("@pid", progId), new SqlParameter("@sn", seqNum) };
            return Databasehelper.ExecuteNonQuery(sql, ps) > 0 ? "Success: Unit Deleted" : "Error: ID not found";
        }

        // SELECTs
        public DataTable GetInstructors() { return Databasehelper.ExecuteQuery("SELECT INSTRUCTOR_ID, INS_FIRST_NAME + ' ' + INS_LAST_NAME AS full_name, INS_EMAIL FROM INSTRUCTORS"); }
        public DataTable GetEnrollmentsJoin()
        {
            string sql = @"SELECT S.ST_FIRST_NAME + ' ' + S.ST_LAST_NAME AS student_name, P.TITLE AS program_title, E.ENROLLED_AT , E.UNITS_COMPLETED 
                           FROM ENROLLMENTS E JOIN STUDENT S ON E.STUDENT_ID = S.STUDENT_ID JOIN PROGRAM P ON E.PROGRAM_ID = P.PROGRAM_ID";
            return Databasehelper.ExecuteQuery(sql);
        }
    }
}
