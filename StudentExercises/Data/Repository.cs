using System.Collections.Generic;
using System.Data.SqlClient;
using StudentExercises.Models;

namespace StudentExercises.Data
{
    /// <summary>
    /// This class contains all database interactions
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Sets up the database connection
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost,1433;Initial Catalog=StudentExercises;User=sa;Password=Strong!Passw0rd;Trusted_Connection=False;MultipleActiveResultSets=true;";
                return new SqlConnection(_connectionString);
            }
        }

        /// <summary>
        /// Get a list of all exercises
        /// </summary>
        /// <returns>List of exercises</returns>
        public List<Exercise> GetAllExercises()
        {
            using (SqlConnection c = Connection)
            {
                c.Open();

                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise";

                    SqlDataReader reader = cmd.ExecuteReader();

                    var exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        var exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }

                    reader.Close();
                    return exercises;
                }
            }
        }

        /// <summary>
        /// Retrieve all exercises of the specified language
        /// </summary>
        /// <param name="language"></param>
        public List<Exercise> GetAllExercisesByLanguage(string language)
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Language FROM Exercise
                                        WHERE Language = @Language";
                    cmd.Parameters.Add(new SqlParameter("@Language", language));
                    SqlDataReader reader = cmd.ExecuteReader();

                    var exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        var exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }

                    reader.Close();
                    return exercises;

                }
            }
                
        }

        /// <summary>
        /// Save a new exercise to the Database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Language"></param>
        public void AddNewExercise(string Name, string Language)
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Exercise (Name, Language) VALUES (@Name, @Language)";
                    cmd.Parameters.Add(new SqlParameter("@Name", Name));
                    cmd.Parameters.Add(new SqlParameter("@Language", Language));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieve all instructors, including Cohort information
        /// </summary>
        /// <returns></returns>
        public List<Instructor> GetAllInstructorsWithCohort()
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = $"SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.CohortId, c.Name FROM Instructor i INNER JOIN Cohort c ON i.CohortId = c.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    var instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        var i = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        instructors.Add(i);
                    }

                    reader.Close();
                    return instructors;
                }
            }
        }


        /// <summary>
        /// Add a new instructor to the database
        /// </summary>
        /// <param name="instructor"></param>
        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) VALUES (@FirstName, @LastName, @SlackHandle, @CohortId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle", instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", instructor.CohortId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Assign an existing exercise to an existing student
        /// </summary>
        /// <param name="StudentId"></param>
        /// <param name="ExerciseId"></param>
        public void AssignExerciseToStudent(int StudentId, int ExerciseId)
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (@StudentId, @ExerciseId)";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", StudentId));
                    cmd.Parameters.Add(new SqlParameter("@ExerciseId", ExerciseId));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get exercises assigned to specified student
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        public List<Exercise> GetStudentExercises(int StudentId)
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM StudentExercise se INNER JOIN Exercise e on e.Id = se.ExerciseId WHERE se.StudentId = @StudentId";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", StudentId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    var exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        var exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }


        /// <summary>
        /// Get a list of all students
        /// </summary>
        /// <returns></returns>
        public List<Student> GetStudents()
        {
            using (SqlConnection c = Connection)
            {
                c.Open();
                using (SqlCommand cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name as CohortName FROM Student s INNER JOIN Cohort c ON s.CohortId = c.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    var students = new List<Student>();
                    while(reader.Read())
                    {
                        var exercises = GetStudentExercises(reader.GetInt32(reader.GetOrdinal("Id")));
                        var student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("CohortName"))
                            },
                            Exercises = exercises
                        };

                        students.Add(student);
                    }
                    reader.Close();
                    return students;
                }
            }
        }

    }

}
