namespace StudentManagementSystem
{
    public class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public Instructor(int id, string name, string specialization)
        {
            InstructorId = id;
            Name = name;
            Specialization = specialization;
        }
        public string PrintDetails()
        {
            return $"Instructor [Id: {InstructorId}, Name: {Name}, Specialization: {Specialization}]";
        }
    }
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public Course(int id, string title, Instructor instructor)
        {
            CourseId = id;
            Title = title;
            Instructor = instructor;
        }
        public string PrintDetails()
        {
            return $"Course [Id: {CourseId}, Title: {Title}, Instructor: {Instructor.Name}]";
        }
    }
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; }

        public Student(int id, string name, int age)
        {
            StudentId = id;
            Name = name;
            Age = age;
            Courses = new List<Course>(); 
        }
        public bool Enroll(Course course)
        {
            if (course == null || Courses.Any(c => c.CourseId == course.CourseId))
                return false; 
            Courses.Add(course);
            return true;
        }
        public string PrintDetails()
        {
            string courses = Courses.Count == 0
                ? "None"
                : string.Join(", ", Courses.Select(c => c.Title));
            return $"Student [Id: {StudentId}, Name: {Name}, Age: {Age}, Courses: {courses}]";
        }
    }
    public class StudentManager
    {
        public List<Student> Students { get; private set; } = new List<Student>();
        public List<Course> Courses { get; private set; } = new List<Course>();
        public List<Instructor> Instructors { get; private set; } = new List<Instructor>();
        public bool AddStudent(Student student)
        {
            if (student == null || FindStudent(student.StudentId) != null) return false;
            Students.Add(student);
            return true;
        }

        public bool AddCourse(Course course)
        {
            if (course == null || FindCourse(course.CourseId) != null) return false;
            Courses.Add(course);
            return true;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (instructor == null || FindInstructor(instructor.InstructorId) != null) return false;
            Instructors.Add(instructor);
            return true;
        }
        public Student FindStudent(int studentId) =>
            Students.FirstOrDefault(s => s.StudentId == studentId);

        public Student FindStudent(string name) =>
            Students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Course FindCourse(int courseId) =>
            Courses.FirstOrDefault(c => c.CourseId == courseId);

        public Course FindCourse(string title) =>
            Courses.FirstOrDefault(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        public Instructor FindInstructor(int instructorId) =>
            Instructors.FirstOrDefault(i => i.InstructorId == instructorId);
        public bool UpdateStudent(int studentId, string newName, int newAge)
        {
            Student s = FindStudent(studentId);
            if (s == null) return false;
            s.Name = newName;
            s.Age = newAge;
            return true;
        }

        public bool DeleteStudent(int studentId)
        {
            Student s = FindStudent(studentId);
            if (s == null) return false;
            return Students.Remove(s);
        }
        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            Student student = FindStudent(studentId);
            Course course = FindCourse(courseId);
            if (student == null || course == null) return false;
            return student.Enroll(course);
        }

        // Bonus 
        public bool IsStudentEnrolledInCourse(int studentId, int courseId)
        {
            Student s = FindStudent(studentId);
            return s != null && s.Courses.Any(c => c.CourseId == courseId);
        }
        public string GetInstructorNameByCourseName(string courseName)
        {
            Course c = FindCourse(courseName);
            return c?.Instructor.Name;
        }
    }
    class Program
    {
        static StudentManager manager = new StudentManager();

        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": AddInstructor(); break;
                    case "3": AddCourse(); break;
                    case "4": EnrollStudent(); break;
                    case "5": ShowAllStudents(); break;
                    case "6": ShowAllCourses(); break;
                    case "7": ShowAllInstructors(); break;
                    case "8": FindStudent(); break;
                    case "9": FindCourse(); break;
                    case "10": Console.WriteLine("Goodbye!"); return;
                    case "11": CheckEnrollment(); break;
                    case "12": GetInstructorByCourseName(); break;
                    case "13": UpdateStudent(); break;
                    case "14": DeleteStudent(); break;
                    default: Console.WriteLine("Invalid choice, try again."); break;
                }
                Console.WriteLine();
            }
        }
        static void ShowMenu()
        {
            Console.WriteLine("========== Student Management System ==========");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Add Instructor");
            Console.WriteLine("3. Add Course");
            Console.WriteLine("4. Enroll Student in Course");
            Console.WriteLine("5. Show All Students");
            Console.WriteLine("6. Show All Courses");
            Console.WriteLine("7. Show All Instructors");
            Console.WriteLine("8. Find the student by id or name");
            Console.WriteLine("9. Find the course by id or name");
            Console.WriteLine("10. Exit");
            Console.WriteLine("--- Bonus ---");
            Console.WriteLine("11. Check if the student enrolled in specific course");
            Console.WriteLine("12. Return the instructor name by course name");
            Console.WriteLine("--- Extra (Update / Delete) ---");
            Console.WriteLine("13. Update Student");
            Console.WriteLine("14. Delete Student");
            Console.Write("Choose: ");
        }
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value)) return value;
                Console.WriteLine("Please enter a valid number.");
            }
        }
        static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(s)) return s;
                Console.WriteLine("Value cannot be empty.");
            }
        }
        static void AddStudent()
        {
            int id = ReadInt("Student Id: ");
            string name = ReadString("Name: ");
            int age = ReadInt("Age: ");
            Console.WriteLine(manager.AddStudent(new Student(id, name, age))
                ? "Student added successfully."
                : "Failed: a student with this Id already exists.");
        }
        static void AddInstructor()
        {
            int id = ReadInt("Instructor Id: ");
            string name = ReadString("Name: ");
            string spec = ReadString("Specialization: ");
            Console.WriteLine(manager.AddInstructor(new Instructor(id, name, spec))
                ? "Instructor added successfully."
                : "Failed: an instructor with this Id already exists.");
        }
        static void AddCourse()
        {
            if (manager.Instructors.Count == 0)
            {
                Console.WriteLine("Add an instructor first.");
                return;
            }
            int id = ReadInt("Course Id: ");
            string title = ReadString("Title: ");

            Console.WriteLine("Available instructors:");
            foreach (var i in manager.Instructors) Console.WriteLine("  " + i.PrintDetails());

            int instructorId = ReadInt("Select instructor by Id: ");
            Instructor instructor = manager.FindInstructor(instructorId);
            if (instructor == null)
            {
                Console.WriteLine("Instructor not found.");
                return;
            }
            Console.WriteLine(manager.AddCourse(new Course(id, title, instructor))
                ? "Course added successfully."
                : "Failed: a course with this Id already exists.");
        }
        static void EnrollStudent()
        {
            int studentId = ReadInt("Student Id: ");
            int courseId = ReadInt("Course Id: ");
            if (manager.FindStudent(studentId) == null) { Console.WriteLine("Student not found."); return; }
            if (manager.FindCourse(courseId) == null) { Console.WriteLine("Course not found."); return; }
            Console.WriteLine(manager.EnrollStudentInCourse(studentId, courseId)
                ? "Student enrolled successfully."
                : "Student is already enrolled in this course.");
        }
        static void ShowAllStudents()
        {
            if (manager.Students.Count == 0) { Console.WriteLine("No students found."); return; }
            foreach (var s in manager.Students) Console.WriteLine(s.PrintDetails());
        }

        static void ShowAllCourses()
        {
            if (manager.Courses.Count == 0) { Console.WriteLine("No courses found."); return; }
            foreach (var c in manager.Courses) Console.WriteLine(c.PrintDetails());
        }

        static void ShowAllInstructors()
        {
            if (manager.Instructors.Count == 0) { Console.WriteLine("No instructors found."); return; }
            foreach (var i in manager.Instructors) Console.WriteLine(i.PrintDetails());
        }
        static void FindStudent()
        {
            string input = ReadString("Enter student id or name: ");
            Student s = int.TryParse(input, out int id) ? manager.FindStudent(id) : manager.FindStudent(input);
            Console.WriteLine(s != null ? s.PrintDetails() : "Student not found.");
        }
        static void FindCourse()
        {
            string input = ReadString("Enter course id or name: ");
            Course c = int.TryParse(input, out int id) ? manager.FindCourse(id) : manager.FindCourse(input);
            Console.WriteLine(c != null ? c.PrintDetails() : "Course not found.");
        }
        static void CheckEnrollment()
        {
            int studentId = ReadInt("Student Id: ");
            int courseId = ReadInt("Course Id: ");
            if (manager.FindStudent(studentId) == null) { Console.WriteLine("Student not found."); return; }
            if (manager.FindCourse(courseId) == null) { Console.WriteLine("Course not found."); return; }
            Console.WriteLine(manager.IsStudentEnrolledInCourse(studentId, courseId)
                ? "Yes, the student is enrolled in this course."
                : "No, the student is NOT enrolled in this course.");
        }

        static void GetInstructorByCourseName()
        {
            string courseName = ReadString("Course name: ");
            string instructor = manager.GetInstructorNameByCourseName(courseName);
            Console.WriteLine(instructor != null
                ? $"Instructor: {instructor}"
                : "Course not found.");
        }
        static void UpdateStudent()
        {
            int id = ReadInt("Student Id to update: ");
            if (manager.FindStudent(id) == null) { Console.WriteLine("Student not found."); return; }
            string name = ReadString("New name: ");
            int age = ReadInt("New age: ");
            Console.WriteLine(manager.UpdateStudent(id, name, age)
                ? "Student updated successfully."
                : "Update failed.");
        }
        static void DeleteStudent()
        {
            int id = ReadInt("Student Id to delete: ");
            Console.WriteLine(manager.DeleteStudent(id)
                ? "Student deleted successfully."
                : "Student not found.");
        }
    }
}
