using System;
using StudentExercises.Models;
using StudentExercises.Data;
using System.Collections.Generic;
using System.Linq;

namespace StudentExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new Repository();

            //Query the database for all the Exercises.
            List<Exercise> allExercises = repo.GetAllExercises();
            Console.WriteLine("All Exercises");
            foreach (var e in allExercises)
            {
                Console.WriteLine(e.Name);
            }
            Pause();

            //Find all the exercises in the database where the language is Python.
            List<Exercise> pythonExercises = repo.GetAllExercisesByLanguage("Python");

            Console.WriteLine("Python Exercises:");
            foreach (var e in pythonExercises)
            {
                Console.WriteLine(e.Name);
            }
            Pause();

            //Insert a new exercise into the database.
            repo.AddNewExercise("Student Exercises", "C#");
            List<Exercise> allExercisesWithNew = repo.GetAllExercises();
            Console.WriteLine("All Exercises with new one:");
            foreach (var e in allExercisesWithNew)
            {
                Console.WriteLine(e.Name);
            }
            Pause();

            //Find all instructors in the database. Include each instructor's cohort.
            List<Instructor> allInstructors = repo.GetAllInstructorsWithCohort();
            Console.WriteLine("All Instructors:");
            foreach (var i in allInstructors)
            {
                Console.WriteLine($"{i.FirstName} {i.LastName}, Cohort {i.Cohort.Name}");
            }
            Pause();

            //Insert a new instructor into the database.Assign the instructor to an existing cohort.

            var newInstructor = new Instructor
            {
                FirstName = "Kimmy",
                LastName = "Bird",
                SlackHandle = "pandadance",
                CohortId = 1
            };

            repo.AddInstructor(newInstructor);
            List<Instructor> allInstructorsUpdated = repo.GetAllInstructorsWithCohort();
            Console.WriteLine("New Instructor added");
            Console.WriteLine("All Instructors:");
            foreach (var i in allInstructorsUpdated)
            {
                Console.WriteLine($"{i.FirstName} {i.LastName}, Cohort {i.Cohort.Name}");
            }
            Pause();

            //Assign an existing exercise to an existing student.
            repo.AssignExerciseToStudent(1, 4);
            Student student = repo.GetStudents().First(x => x.Id == 1);
            var studentExercises = repo.GetStudentExercises(1);

            Console.WriteLine($"New exercise assigned to {student.FirstName}");
            Console.WriteLine($"{student.FirstName}'s exercises:");
            foreach (var e in studentExercises)
            {
                Console.WriteLine($"{e.Name}, {e.Language}");
            }


        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
