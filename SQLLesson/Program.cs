using System;
using SqlLibrary;

namespace SQLLesson {
    class Program {
        static void Main(string[] args) {
            var sqllib = new BcConnection();
            sqllib.Connect(@"localhost\sqlexpress", "EdDb", "trusted_connection=true"); //puts variables into connection string
            StudentController.bcConnection = sqllib;
            MajorController.bcConnection = sqllib;

            var majors = MajorController.GetAllMajors();
            foreach(var m in majors) {
                Console.WriteLine(m);
            }
            var newMajor = new Major {
                Id = 1000,
                Description = "Computer Science",
                MinSat = 1300
            };
            Console.WriteLine();
            
            var success = MajorController.InsertMajor(newMajor);
            majors = MajorController.GetAllMajors();
            foreach (var m in majors) {
                Console.WriteLine(m);
            }
            Console.WriteLine();
            
            newMajor.MinSat = 1200;
            newMajor.Description = "Computer Engineering";
            success = MajorController.UpdateMajor(newMajor);
            majors = MajorController.GetAllMajors();
            foreach (var m in majors) {
                Console.WriteLine(m);
            }

            Console.WriteLine();
            success = MajorController.DeleteMajor(newMajor);
            majors = MajorController.GetAllMajors();
            foreach (var m in majors) {
                Console.WriteLine(m);
            }


            //var major = MajorController.GetMajorByPk(1);
            //Console.WriteLine(major);
            //var newStudent = new Student {
            //    Id = 248,
            //    Firstname = "Crazy",
            //    Lastname = "Eights",
            //    SAT = 600,
            //    GPA = 0.00,
            //    MajorId = null
            //};
            //var success = StudentController.InsertStudent(newStudent);
            //var studentToDelete = new Student { Id = 824 };
            //var success = StudentController.DeleteStudent(999);

            //var student = StudentController.GetStudentByPk(777);
            //student.Firstname = "Crazy";
            //student.Lastname = "Eights";
            //if(student == null) {
            //    Console.WriteLine("Student not found");
            //} else { Console.WriteLine(student); }

            //student.Firstname = "Charlie";
            //student.Lastname = "Chan";
            //var success = StudentController.UpdateStudent(student);


            //var students = StudentController.GetAllStudents();
            //foreach (var s in students) {
            //    Console.WriteLine(s);
            //}

            sqllib.Disconnect();

          
        }
    }
}
