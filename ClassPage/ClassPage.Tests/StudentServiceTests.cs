using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ClassPage.Tests
{
    [TestFixture]
    public class StudentServiceTests
    {
        private readonly DbConnection dbConnection;
        private readonly DbContextOptions<SchooldbContext> dbContextOptions;
        private SchooldbContext context;
        private StudentService studentService;

        public StudentServiceTests()
        {
            dbConnection = new SqliteConnection("Filename=:memory:");
            dbContextOptions = new DbContextOptionsBuilder<SchooldbContext>().UseSqlite(dbConnection).Options;
        }

        [SetUp]
        public void SeedDB()
        {
            dbConnection.Open();
            context = new SchooldbContext(dbContextOptions);

            context.Database.EnsureCreated();

            context.Teachers.AddRange(new Teacher
            {
                Id = 1,
                FirstName = "Teacher",
                LastName = "One",
                Email = "t1@ma.il"
            });

            context.Classes.AddRange(new Class
            {
                Id = 1,
                ClassName = "Class1",
                MainTeacherId = 1
            });

            context.Students.AddRange(new Student
            {
                FirstName = "Student",
                LastName = "One",
                Email = "s1@ma.il",
                ClassId = 1
            }, new Student
            {
                FirstName = "Student",
                LastName = "Two",
                Email = "s2@ma.il",
                ClassId = 1
            });

            context.SaveChanges();

            studentService = new StudentService(context);
        }

        [TearDown]
        public void Dispose()
        {
            dbConnection.Dispose();
        }

        [Test]
        public void TestGetById()
        {
            Student dbStudent = context.Students.FirstOrDefault(s => s.Id == 1);
            StudentDTO studentDto = studentService.GetById(1);

            Assert.AreEqual(dbStudent.FirstName, studentDto.FirstName);
        }

        [Test]
        public void TestGetAll()
        {
            List<Student> dbStudents = context.Students.ToList();
            List<StudentDTO> studentDtos = studentService.GetAll();

            Assert.AreEqual(dbStudents.First().FirstName, studentDtos.First().FirstName);
        }

        [Test]
        public void TestAdd()
        {
            StudentDTO studentDto = new StudentDTO { FirstName = "Student", LastName = "Three", ClassId = 1, Email = "s3@ma.il" };
            studentService.Add(studentDto);

            Student dbStudent = context.Students.FirstOrDefault(s => s.Id == 3);
            Assert.AreEqual(studentDto.FirstName, dbStudent.FirstName);
        }

        [Test]
        public void TestEdit()
        {
            StudentDTO studentDto = new StudentDTO { FirstName = "EditStudent", LastName = "Two", Email = "s2@ma.il", ClassId = 1 };
            studentService.Edit(2, studentDto);

            Student dbStudent = context.Students.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(studentDto.FirstName, dbStudent.FirstName);
        }

        [Test]
        public void TestDelete()
        {
            Student dbStudent = context.Students.FirstOrDefault(s => s.Id == 2);
            studentService.Delete(2);

            CollectionAssert.DoesNotContain(context.Students.ToList(), dbStudent);
        }
    }
}
