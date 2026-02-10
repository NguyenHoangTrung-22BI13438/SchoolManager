using SchoolManager.Application.Interfaces;
using SchoolManager.Application.Services;
using SchoolManager.Domain.Rules;
using SchoolManager.Infrastructure.Json;

namespace SchoolManager
{
    public class SchoolAppContext
    {
        public StudentService Students { get; }
        public TeacherService Teachers { get; }
        public SubjectService Subjects { get; }
        public ClassroomService Classrooms { get; }
        public GradeService Grades { get; }
        public ReportService Reports { get; }

        public IStudentRepository StudentRepo { get; }
        public ITeacherRepository TeacherRepo { get; }
        public ISubjectRepository SubjectRepo { get; }
        public IClassroomRepository ClassroomRepo { get; }
        public IGradeRepository GradeRepo { get; }
        public IClassroomSubjectRepository ClassroomSubjectRepo { get; }

        public SchoolAppContext(
            StudentService students,
            TeacherService teachers,
            SubjectService subjects,
            ClassroomService classrooms,
            GradeService grades,
            ReportService reports,
            IStudentRepository studentRepo,
            ITeacherRepository teacherRepo,
            ISubjectRepository subjectRepo,
            IClassroomRepository classroomRepo,
            IGradeRepository gradeRepo,
            IClassroomSubjectRepository classroomSubjectRepo)
        {
            Students = students;
            Teachers = teachers;
            Subjects = subjects;
            Classrooms = classrooms;
            Grades = grades;
            Reports = reports;

            StudentRepo = studentRepo;
            TeacherRepo = teacherRepo;
            SubjectRepo = subjectRepo;
            ClassroomRepo = classroomRepo;
            GradeRepo = gradeRepo;
            ClassroomSubjectRepo = classroomSubjectRepo;
        }
    }

    public static class AppBootstrapper
    {
        public static SchoolAppContext Build()
        {
            // Repositories
            IStudentRepository studentRepo = new JsonStudentRepository("Infrastructure/Json/Data/students.json");
            ITeacherRepository teacherRepo = new JsonTeacherRepository("Infrastructure/Json/Data/teachers.json");
            ISubjectRepository subjectRepo = new JsonSubjectRepository("Infrastructure/Json/Data/subjects.json");
            IClassroomRepository classroomRepo = new JsonClassroomRepository("Infrastructure/Json/Data/classrooms.json");
            IGradeRepository gradeRepo = new JsonGradeRepository("Infrastructure/Json/Data/grades.json");
            IClassroomSubjectRepository classroomSubjectRepo = new JsonClassroomSubjectRepository("Infrastructure/Json/Data/classroomsubjects.json");

            // Domain Rules
            IAcademicRules academicRules = new StandardAcademicRules();

            // Services
            var studentService = new StudentService(studentRepo, classroomRepo);
            var teacherService = new TeacherService(teacherRepo, classroomSubjectRepo);
            var subjectService = new SubjectService(subjectRepo, classroomSubjectRepo);
            var classroomService = new ClassroomService(classroomRepo, studentRepo, classroomSubjectRepo);
            var gradeService = new GradeService(gradeRepo, studentRepo, subjectRepo);
            var reportService = new ReportService(gradeRepo, subjectRepo, academicRules);

            return new SchoolAppContext(
                studentService,
                teacherService,
                subjectService,
                classroomService,
                gradeService,
                reportService,
                studentRepo,
                teacherRepo,
                subjectRepo,
                classroomRepo,
                gradeRepo,
                classroomSubjectRepo
            );
        }
    }
}