using SchoolManager.Application.Services;
using SchoolManager.Domain.Models;
using SchoolManager.Infrastructure.Json;
using System;

class Program
{
    static void Main()
    {
        // ---------- Wiring ----------
        var studentRepo = new JsonStudentRepository(
            "Infrastructure/Json/Data/students.json");
        var teacherRepo = new JsonTeacherRepository(
            "Infrastructure/Json/Data/teachers.json");
        var subjectRepo = new JsonSubjectRepository(
            "Infrastructure/Json/Data/subjects.json");
        var classRepo = new JsonClassroomRepository(
            "Infrastructure/Json/Data/classes.json");
        var gradeRepo = new JsonGradeRepository(
            "Infrastructure/Json/Data/grades.json");

        var studentService = new StudentService(studentRepo, classRepo);
        var teacherService = new TeacherService(teacherRepo, subjectRepo);
        var subjectService = new SubjectService(subjectRepo);
        var classroomService = new ClassroomService(classRepo, studentRepo);
        var reportService = new ReportService(gradeRepo, subjectRepo);

        // ---------- Menu loop ----------
        while (true)
        {
            Console.Clear();
            PrintMainMenu();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StudentMenu(studentService, studentRepo);
                    break;

                case "2":
                    TeacherMenu(teacherService, teacherRepo);
                    break;

                case "3":
                    SubjectMenu(subjectService, subjectRepo, teacherRepo);
                    break;

                case "4":
                    ClassroomMenu(classroomService, classRepo, studentRepo);
                    break;

                case "5":
                    GradeMenu(gradeRepo, studentRepo, subjectRepo);
                    break;

                case "6":
                    GenerateReport(reportService);
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    // ---------- Main Menu ----------
    static void PrintMainMenu()
    {
        Console.WriteLine("=== SCHOOL MANAGEMENT SYSTEM ===");
        Console.WriteLine("1. Student Management");
        Console.WriteLine("2. Teacher Management");
        Console.WriteLine("3. Subject Management");
        Console.WriteLine("4. Classroom Management");
        Console.WriteLine("5. Grade Management");
        Console.WriteLine("6. Student Academic Report");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
    }

    // ========== STUDENT MANAGEMENT ==========
    static void StudentMenu(StudentService service, JsonStudentRepository repo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENT MANAGEMENT ===");
            Console.WriteLine("1. List all students");
            Console.WriteLine("2. View student by ID");
            Console.WriteLine("3. Add student");
            Console.WriteLine("4. Update student");
            Console.WriteLine("5. Delete student");
            Console.WriteLine("6. Change student class");
            Console.WriteLine("7. List students by class");
            Console.WriteLine("0. Back to main menu");
            Console.WriteLine();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListStudents(repo);
                    break;
                case "2":
                    ViewStudentById(repo);
                    break;
                case "3":
                    AddStudent(service);
                    break;
                case "4":
                    UpdateStudent(repo);
                    break;
                case "5":
                    DeleteStudent(repo);
                    break;
                case "6":
                    ChangeStudentClass(service);
                    break;
                case "7":
                    ListStudentsByClass(service);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ListStudents(JsonStudentRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== STUDENT LIST ===");

        var students = repo.GetAll();
        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
        }
        else
        {
            foreach (var s in students)
            {
                Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | ClassID: {s.ClassId} | Status: {s.Status}");
            }
        }

        Pause();
    }

    static void ViewStudentById(JsonStudentRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== VIEW STUDENT ===");

        Console.Write("Student ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var student = repo.GetById(id);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
        }
        else
        {
            Console.WriteLine($"ID: {student.Id}");
            Console.WriteLine($"Name: {student.Name}");
            Console.WriteLine($"ClassID: {student.ClassId}");
            Console.WriteLine($"Status: {student.Status}");
        }

        Pause();
    }

    static void AddStudent(StudentService service)
    {
        Console.Clear();
        Console.WriteLine("=== ADD STUDENT ===");

        Console.Write("ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Name: ");
        var name = Console.ReadLine()!;

        Console.Write("ClassID: ");
        var classId = int.Parse(Console.ReadLine()!);

        Console.Write("Status (1=In, 2=Graduated, 3=Outed): ");
        var status = (StudentStatus)int.Parse(Console.ReadLine()!);

        service.EnrollStudent(new Student
        {
            Id = id,
            Name = name,
            ClassId = classId,
            Status = status
        });

        Console.WriteLine("Student added successfully.");
        Pause();
    }

    static void UpdateStudent(JsonStudentRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== UPDATE STUDENT ===");

        Console.Write("Student ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var student = repo.GetById(id);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current Name: {student.Name}");
        Console.Write("New Name (leave empty to keep current): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            student.Name = name;

        Console.WriteLine($"Current ClassID: {student.ClassId}");
        Console.Write("New ClassID (leave empty to keep current): ");
        var classIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(classIdInput))
            student.ClassId = int.Parse(classIdInput);

        Console.WriteLine($"Current Status: {student.Status}");
        Console.Write("New Status (1=In, 2=Graduated, 3=Outed, leave empty to keep current): ");
        var statusInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(statusInput))
            student.Status = (StudentStatus)int.Parse(statusInput);

        repo.Update(student);
        Console.WriteLine("Student updated successfully.");
        Pause();
    }

    static void DeleteStudent(JsonStudentRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== DELETE STUDENT ===");

        Console.Write("Student ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var student = repo.GetById(id);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
        }
        else
        {
            Console.Write($"Are you sure you want to delete '{student.Name}'? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y")
            {
                repo.Delete(id);
                Console.WriteLine("Student deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        Pause();
    }

    static void ChangeStudentClass(StudentService service)
    {
        Console.Clear();
        Console.WriteLine("=== CHANGE STUDENT CLASS ===");

        Console.Write("Student ID: ");
        var studentId = int.Parse(Console.ReadLine()!);

        Console.Write("New Class ID: ");
        var newClassId = int.Parse(Console.ReadLine()!);

        try
        {
            service.ChangeClass(studentId, newClassId);
            Console.WriteLine("Student class changed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    static void ListStudentsByClass(StudentService service)
    {
        Console.Clear();
        Console.WriteLine("=== STUDENTS BY CLASS ===");

        Console.Write("Class ID: ");
        var classId = int.Parse(Console.ReadLine()!);

        var students = service.GetStudentsByClass(classId);
        if (students.Count == 0)
        {
            Console.WriteLine("No students found in this class.");
        }
        else
        {
            foreach (var s in students)
            {
                Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | Status: {s.Status}");
            }
        }

        Pause();
    }

    // ========== TEACHER MANAGEMENT ==========
    static void TeacherMenu(TeacherService service, JsonTeacherRepository repo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== TEACHER MANAGEMENT ===");
            Console.WriteLine("1. List all teachers");
            Console.WriteLine("2. View teacher by ID");
            Console.WriteLine("3. Add teacher");
            Console.WriteLine("4. Update teacher");
            Console.WriteLine("5. Delete teacher");
            Console.WriteLine("6. View teacher's subjects");
            Console.WriteLine("0. Back to main menu");
            Console.WriteLine();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListTeachers(repo);
                    break;
                case "2":
                    ViewTeacherById(repo);
                    break;
                case "3":
                    AddTeacher(repo);
                    break;
                case "4":
                    UpdateTeacher(repo);
                    break;
                case "5":
                    DeleteTeacher(repo);
                    break;
                case "6":
                    ViewTeacherSubjects(service);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ListTeachers(JsonTeacherRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== TEACHER LIST ===");

        var teachers = repo.GetAll();
        if (teachers.Count == 0)
        {
            Console.WriteLine("No teachers found.");
        }
        else
        {
            foreach (var t in teachers)
            {
                Console.WriteLine($"ID: {t.Id} | Name: {t.Name}");
            }
        }

        Pause();
    }

    static void ViewTeacherById(JsonTeacherRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== VIEW TEACHER ===");

        Console.Write("Teacher ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var teacher = repo.GetById(id);
        if (teacher == null)
        {
            Console.WriteLine("Teacher not found.");
        }
        else
        {
            Console.WriteLine($"ID: {teacher.Id}");
            Console.WriteLine($"Name: {teacher.Name}");
        }

        Pause();
    }

    static void AddTeacher(JsonTeacherRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== ADD TEACHER ===");

        Console.Write("ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Name: ");
        var name = Console.ReadLine()!;

        repo.Add(new Teacher
        {
            Id = id,
            Name = name
        });

        Console.WriteLine("Teacher added successfully.");
        Pause();
    }

    static void UpdateTeacher(JsonTeacherRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== UPDATE TEACHER ===");

        Console.Write("Teacher ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var teacher = repo.GetById(id);
        if (teacher == null)
        {
            Console.WriteLine("Teacher not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current Name: {teacher.Name}");
        Console.Write("New Name (leave empty to keep current): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            teacher.Name = name;

        repo.Update(teacher);
        Console.WriteLine("Teacher updated successfully.");
        Pause();
    }

    static void DeleteTeacher(JsonTeacherRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== DELETE TEACHER ===");

        Console.Write("Teacher ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var teacher = repo.GetById(id);
        if (teacher == null)
        {
            Console.WriteLine("Teacher not found.");
        }
        else
        {
            Console.Write($"Are you sure you want to delete '{teacher.Name}'? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y")
            {
                repo.Delete(id);
                Console.WriteLine("Teacher deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        Pause();
    }

    static void ViewTeacherSubjects(TeacherService service)
    {
        Console.Clear();
        Console.WriteLine("=== TEACHER'S SUBJECTS ===");

        Console.Write("Teacher ID: ");
        var teacherId = int.Parse(Console.ReadLine()!);

        var subjects = service.GetSubjects(teacherId);
        if (subjects.Count == 0)
        {
            Console.WriteLine("No subjects found for this teacher.");
        }
        else
        {
            foreach (var s in subjects)
            {
                Console.WriteLine($"ID: {s.Id} | Name: {s.Name}");
            }
        }

        Pause();
    }

    // ========== SUBJECT MANAGEMENT ==========
    static void SubjectMenu(SubjectService service, JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SUBJECT MANAGEMENT ===");
            Console.WriteLine("1. List all subjects");
            Console.WriteLine("2. View subject by ID");
            Console.WriteLine("3. Add subject");
            Console.WriteLine("4. Update subject");
            Console.WriteLine("5. Delete subject");
            Console.WriteLine("6. Assign teacher to subject");
            Console.WriteLine("0. Back to main menu");
            Console.WriteLine();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListSubjects(repo, teacherRepo);
                    break;
                case "2":
                    ViewSubjectById(repo, teacherRepo);
                    break;
                case "3":
                    AddSubject(repo);
                    break;
                case "4":
                    UpdateSubject(repo);
                    break;
                case "5":
                    DeleteSubject(repo);
                    break;
                case "6":
                    AssignTeacher(service);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ListSubjects(JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
    {
        Console.Clear();
        Console.WriteLine("=== SUBJECT LIST ===");

        var subjects = repo.GetAll();
        if (subjects.Count == 0)
        {
            Console.WriteLine("No subjects found.");
        }
        else
        {
            foreach (var s in subjects)
            {
                var teacher = teacherRepo.GetById(s.TeacherId);
                var teacherName = teacher?.Name ?? "Not assigned";
                Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | Teacher: {teacherName}");
            }
        }

        Pause();
    }

    static void ViewSubjectById(JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
    {
        Console.Clear();
        Console.WriteLine("=== VIEW SUBJECT ===");

        Console.Write("Subject ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var subject = repo.GetById(id);
        if (subject == null)
        {
            Console.WriteLine("Subject not found.");
        }
        else
        {
            var teacher = teacherRepo.GetById(subject.TeacherId);
            Console.WriteLine($"ID: {subject.Id}");
            Console.WriteLine($"Name: {subject.Name}");
            Console.WriteLine($"Teacher ID: {subject.TeacherId}");
            Console.WriteLine($"Teacher Name: {teacher?.Name ?? "Not assigned"}");
        }

        Pause();
    }

    static void AddSubject(JsonSubjectRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== ADD SUBJECT ===");

        Console.Write("ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Name: ");
        var name = Console.ReadLine()!;

        Console.Write("Teacher ID (0 for none): ");
        var teacherId = int.Parse(Console.ReadLine()!);

        repo.Add(new Subject
        {
            Id = id,
            Name = name,
            TeacherId = teacherId
        });

        Console.WriteLine("Subject added successfully.");
        Pause();
    }

    static void UpdateSubject(JsonSubjectRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== UPDATE SUBJECT ===");

        Console.Write("Subject ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var subject = repo.GetById(id);
        if (subject == null)
        {
            Console.WriteLine("Subject not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current Name: {subject.Name}");
        Console.Write("New Name (leave empty to keep current): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            subject.Name = name;

        Console.WriteLine($"Current Teacher ID: {subject.TeacherId}");
        Console.Write("New Teacher ID (leave empty to keep current): ");
        var teacherIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(teacherIdInput))
            subject.TeacherId = int.Parse(teacherIdInput);

        repo.Update(subject);
        Console.WriteLine("Subject updated successfully.");
        Pause();
    }

    static void DeleteSubject(JsonSubjectRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== DELETE SUBJECT ===");

        Console.Write("Subject ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var subject = repo.GetById(id);
        if (subject == null)
        {
            Console.WriteLine("Subject not found.");
        }
        else
        {
            Console.Write($"Are you sure you want to delete '{subject.Name}'? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y")
            {
                repo.Delete(id);
                Console.WriteLine("Subject deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        Pause();
    }

    static void AssignTeacher(SubjectService service)
    {
        Console.Clear();
        Console.WriteLine("=== ASSIGN TEACHER TO SUBJECT ===");

        Console.Write("Subject ID: ");
        var subjectId = int.Parse(Console.ReadLine()!);

        Console.Write("Teacher ID: ");
        var teacherId = int.Parse(Console.ReadLine()!);

        try
        {
            service.AssignTeacher(subjectId, teacherId);
            Console.WriteLine("Teacher assigned successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    // ========== CLASSROOM MANAGEMENT ==========
    static void ClassroomMenu(ClassroomService service, JsonClassroomRepository repo, JsonStudentRepository studentRepo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CLASSROOM MANAGEMENT ===");
            Console.WriteLine("1. List all classrooms");
            Console.WriteLine("2. View classroom by ID");
            Console.WriteLine("3. Add classroom");
            Console.WriteLine("4. Update classroom");
            Console.WriteLine("5. Delete classroom");
            Console.WriteLine("6. List classrooms by grade");
            Console.WriteLine("0. Back to main menu");
            Console.WriteLine();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListClassrooms(repo, studentRepo);
                    break;
                case "2":
                    ViewClassroomById(repo, studentRepo);
                    break;
                case "3":
                    AddClassroom(repo);
                    break;
                case "4":
                    UpdateClassroom(repo);
                    break;
                case "5":
                    DeleteClassroom(repo);
                    break;
                case "6":
                    ListClassroomsByGrade(repo);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ListClassrooms(JsonClassroomRepository repo, JsonStudentRepository studentRepo)
    {
        Console.Clear();
        Console.WriteLine("=== CLASSROOM LIST ===");

        var classrooms = repo.GetAll();
        if (classrooms.Count == 0)
        {
            Console.WriteLine("No classrooms found.");
        }
        else
        {
            foreach (var c in classrooms)
            {
                var studentCount = studentRepo.GetAll().Count(s => s.ClassId == c.Id);
                Console.WriteLine($"ID: {c.Id} | GradeID: {c.GradeId} | Students: {studentCount}");
            }
        }

        Pause();
    }

    static void ViewClassroomById(JsonClassroomRepository repo, JsonStudentRepository studentRepo)
    {
        Console.Clear();
        Console.WriteLine("=== VIEW CLASSROOM ===");

        Console.Write("Classroom ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var classroom = repo.GetById(id);
        if (classroom == null)
        {
            Console.WriteLine("Classroom not found.");
        }
        else
        {
            var students = studentRepo.GetAll().Where(s => s.ClassId == classroom.Id).ToList();
            Console.WriteLine($"ID: {classroom.Id}");
            Console.WriteLine($"GradeID: {classroom.GradeId}");
            Console.WriteLine($"Number of Students: {students.Count}");

            if (students.Count > 0)
            {
                Console.WriteLine("\nStudents:");
                foreach (var s in students)
                {
                    Console.WriteLine($"  - {s.Name} (ID: {s.Id})");
                }
            }
        }

        Pause();
    }

    static void AddClassroom(JsonClassroomRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== ADD CLASSROOM ===");

        Console.Write("ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Grade ID: ");
        var gradeId = int.Parse(Console.ReadLine()!);

        repo.Add(new Classroom
        {
            Id = id,
            GradeId = gradeId
        });

        Console.WriteLine("Classroom added successfully.");
        Pause();
    }

    static void UpdateClassroom(JsonClassroomRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== UPDATE CLASSROOM ===");

        Console.Write("Classroom ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var classroom = repo.GetById(id);
        if (classroom == null)
        {
            Console.WriteLine("Classroom not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current Grade ID: {classroom.GradeId}");
        Console.Write("New Grade ID (leave empty to keep current): ");
        var gradeIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(gradeIdInput))
            classroom.GradeId = int.Parse(gradeIdInput);

        repo.Update(classroom);
        Console.WriteLine("Classroom updated successfully.");
        Pause();
    }

    static void DeleteClassroom(JsonClassroomRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== DELETE CLASSROOM ===");

        Console.Write("Classroom ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var classroom = repo.GetById(id);
        if (classroom == null)
        {
            Console.WriteLine("Classroom not found.");
        }
        else
        {
            Console.Write($"Are you sure you want to delete classroom {classroom.Id}? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y")
            {
                repo.Delete(id);
                Console.WriteLine("Classroom deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        Pause();
    }

    static void ListClassroomsByGrade(JsonClassroomRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== CLASSROOMS BY GRADE ===");

        Console.Write("Grade ID: ");
        var gradeId = int.Parse(Console.ReadLine()!);

        var classrooms = repo.GetByGradeId(gradeId);
        if (classrooms.Count == 0)
        {
            Console.WriteLine("No classrooms found for this grade.");
        }
        else
        {
            foreach (var c in classrooms)
            {
                Console.WriteLine($"ID: {c.Id} | GradeID: {c.GradeId}");
            }
        }

        Pause();
    }

    // ========== GRADE MANAGEMENT ==========
    static void GradeMenu(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GRADE MANAGEMENT ===");
            Console.WriteLine("1. List all grades");
            Console.WriteLine("2. View grade by ID");
            Console.WriteLine("3. Add grade");
            Console.WriteLine("4. Update grade");
            Console.WriteLine("5. Delete grade");
            Console.WriteLine("6. View grades by student");
            Console.WriteLine("0. Back to main menu");
            Console.WriteLine();

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListGrades(repo, studentRepo, subjectRepo);
                    break;
                case "2":
                    ViewGradeById(repo, studentRepo, subjectRepo);
                    break;
                case "3":
                    AddGrade(repo);
                    break;
                case "4":
                    UpdateGrade(repo);
                    break;
                case "5":
                    DeleteGrade(repo);
                    break;
                case "6":
                    ViewGradesByStudent(repo, subjectRepo);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ListGrades(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
    {
        Console.Clear();
        Console.WriteLine("=== GRADE LIST ===");

        var grades = repo.GetAll();
        if (grades.Count == 0)
        {
            Console.WriteLine("No grades found.");
        }
        else
        {
            foreach (var g in grades)
            {
                var student = studentRepo.GetById(g.StudentID);
                var subject = subjectRepo.GetById(g.SubjectID);
                Console.WriteLine($"ID: {g.Id} | Student: {student?.Name ?? "Unknown"} | Subject: {subject?.Name ?? "Unknown"} | Score: {g.Score}");
            }
        }

        Pause();
    }

    static void ViewGradeById(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
    {
        Console.Clear();
        Console.WriteLine("=== VIEW GRADE ===");

        Console.Write("Grade ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var grade = repo.GetById(id);
        if (grade == null)
        {
            Console.WriteLine("Grade not found.");
        }
        else
        {
            var student = studentRepo.GetById(grade.StudentID);
            var subject = subjectRepo.GetById(grade.SubjectID);
            Console.WriteLine($"ID: {grade.Id}");
            Console.WriteLine($"Student: {student?.Name ?? "Unknown"} (ID: {grade.StudentID})");
            Console.WriteLine($"Subject: {subject?.Name ?? "Unknown"} (ID: {grade.SubjectID})");
            Console.WriteLine($"Score: {grade.Score}");
        }

        Pause();
    }

    static void AddGrade(JsonGradeRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== ADD GRADE ===");

        Console.Write("ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Student ID: ");
        var studentId = int.Parse(Console.ReadLine()!);

        Console.Write("Subject ID: ");
        var subjectId = int.Parse(Console.ReadLine()!);

        Console.Write("Score: ");
        var score = decimal.Parse(Console.ReadLine()!);

        repo.Add(new Grade
        {
            Id = id,
            StudentID = studentId,
            SubjectID = subjectId,
            Score = score
        });

        Console.WriteLine("Grade added successfully.");
        Pause();
    }

    static void UpdateGrade(JsonGradeRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== UPDATE GRADE ===");

        Console.Write("Grade ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var grade = repo.GetById(id);
        if (grade == null)
        {
            Console.WriteLine("Grade not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current Student ID: {grade.StudentID}");
        Console.Write("New Student ID (leave empty to keep current): ");
        var studentIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(studentIdInput))
            grade.StudentID = int.Parse(studentIdInput);

        Console.WriteLine($"Current Subject ID: {grade.SubjectID}");
        Console.Write("New Subject ID (leave empty to keep current): ");
        var subjectIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(subjectIdInput))
            grade.SubjectID = int.Parse(subjectIdInput);

        Console.WriteLine($"Current Score: {grade.Score}");
        Console.Write("New Score (leave empty to keep current): ");
        var scoreInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(scoreInput))
            grade.Score = decimal.Parse(scoreInput);

        repo.Update(grade);
        Console.WriteLine("Grade updated successfully.");
        Pause();
    }

    static void DeleteGrade(JsonGradeRepository repo)
    {
        Console.Clear();
        Console.WriteLine("=== DELETE GRADE ===");

        Console.Write("Grade ID: ");
        var id = int.Parse(Console.ReadLine()!);

        var grade = repo.GetById(id);
        if (grade == null)
        {
            Console.WriteLine("Grade not found.");
        }
        else
        {
            Console.Write($"Are you sure you want to delete grade {grade.Id}? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y")
            {
                repo.Delete(id);
                Console.WriteLine("Grade deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        Pause();
    }

    static void ViewGradesByStudent(JsonGradeRepository repo, JsonSubjectRepository subjectRepo)
    {
        Console.Clear();
        Console.WriteLine("=== STUDENT GRADES ===");

        Console.Write("Student ID: ");
        var studentId = int.Parse(Console.ReadLine()!);

        var grades = repo.GetAll().Where(g => g.StudentID == studentId).ToList();
        if (grades.Count == 0)
        {
            Console.WriteLine("No grades found for this student.");
        }
        else
        {
            foreach (var g in grades)
            {
                var subject = subjectRepo.GetById(g.SubjectID);
                Console.WriteLine($"Subject: {subject?.Name ?? "Unknown"} | Score: {g.Score}");
            }
        }

        Pause();
    }

    // ========== REPORT ==========
    static void GenerateReport(ReportService service)
    {
        Console.Clear();
        Console.WriteLine("=== STUDENT ACADEMIC REPORT ===");

        Console.Write("Student ID: ");
        var id = int.Parse(Console.ReadLine()!);

        try
        {
            var result = service.EvaluateStudent(id);
            Console.WriteLine($"Academic Performance: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }
}