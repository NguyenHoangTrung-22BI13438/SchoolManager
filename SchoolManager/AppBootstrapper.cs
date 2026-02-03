using SchoolManager.Application.Services;
using SchoolManager.Infrastructure.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager
{
    public class SchoolAppContext
    {
        public StudentService Students { get; }
        public TeacherService Teachers { get; }
        public SubjectService Subjects { get; }
        public ClassroomService Classrooms { get; }
        public ReportService Reports { get; }

        public JsonStudentRepository StudentRepo { get; }
        public JsonTeacherRepository TeacherRepo { get; }
        public JsonSubjectRepository SubjectRepo { get; }
        public JsonClassroomRepository ClassroomRepo { get; }
        public JsonGradeRepository GradeRepo { get; }

        public SchoolAppContext(
            StudentService students,
            TeacherService teachers,
            SubjectService subjects,
            ClassroomService classrooms,
            ReportService reports,
            JsonStudentRepository studentRepo,
            JsonTeacherRepository teacherRepo,
            JsonSubjectRepository subjectRepo,
            JsonClassroomRepository classroomRepo,
            JsonGradeRepository gradeRepo)
        {
            Students = students;
            Teachers = teachers;
            Subjects = subjects;
            Classrooms = classrooms;
            Reports = reports;

            StudentRepo = studentRepo;
            TeacherRepo = teacherRepo;
            SubjectRepo = subjectRepo;
            ClassroomRepo = classroomRepo;
            GradeRepo = gradeRepo;
        }
    }

    public static class AppBootstrapper
    {
        public static SchoolAppContext Build()
        {
            var studentRepo = new JsonStudentRepository("Infrastructure/Json/Data/students.json");
            var teacherRepo = new JsonTeacherRepository("Infrastructure/Json/Data/teachers.json");
            var subjectRepo = new JsonSubjectRepository("Infrastructure/Json/Data/subjects.json");
            var classroomRepo = new JsonClassroomRepository("Infrastructure/Json/Data/classrooms.json");
            var gradeRepo = new JsonGradeRepository("Infrastructure/Json/Data/grades.json");

            return new SchoolAppContext(
                new StudentService(studentRepo, classroomRepo),
                new TeacherService(teacherRepo, subjectRepo),
                new SubjectService(subjectRepo),
                new ClassroomService(classroomRepo, studentRepo),
                new ReportService(gradeRepo, subjectRepo),
                studentRepo,
                teacherRepo,
                subjectRepo,
                classroomRepo,
                gradeRepo
            );
        }
    }
}
