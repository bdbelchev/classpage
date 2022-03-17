using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Tests
{
    [TestFixture]
    public class GradeServiceTests
    {
        private readonly DbConnection dbConnection;
        private readonly DbContextOptions<SchooldbContext> dbContextOptions;
        private SchooldbContext context;
        private GradeService gradeService;

        public GradeServiceTests()
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

            context.Subjects.AddRange(new Subject
            {
                Id = 1,
                SubjectName = "Subject1"
            });

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
                Id = 1,
                FirstName = "Grade",
                LastName = "One",
                Email = "s1@ma.il",
                ClassId = 1
            });

            context.Grades.AddRange(new Grade
            {
                Value = 6,
                SubjectId = 1,
                TeacherId = 1,
                StudentId = 1,
                DateAdded = DateTime.Now
            }, new Grade
            {
                Value = 2,
                SubjectId = 1,
                TeacherId = 1,
                StudentId = 1,
                DateAdded = DateTime.Now
            });

            context.SaveChanges();

            gradeService = new GradeService(context);
        }

        [TearDown]
        public void Dispose()
        {
            dbConnection.Dispose();
        }

        [Test]
        public void TestGetById()
        {
            Grade dbGrade = context.Grades.FirstOrDefault(s => s.Id == 1);
            GradeDTO gradeDto = gradeService.GetById(1);

            Assert.AreEqual(dbGrade.Value, gradeDto.Value);
        }

        [Test]
        public void TestGetAll()
        {
            List<Grade> dbGrades = context.Grades.ToList();
            List<GradeDTO> gradeDtos = gradeService.GetAll();

            Assert.AreEqual(dbGrades.First().Value, gradeDtos.First().Value);
        }

        [Test]
        public void TestAdd()
        {
            GradeDTO gradeDto = new GradeDTO { Value = 5.50, SubjectId = 1, TeacherId = 1, StudentId = 1 };
            gradeService.Add(gradeDto);

            Grade dbGrade = context.Grades.FirstOrDefault(s => s.Id == 3);
            Assert.AreEqual(gradeDto.Value, dbGrade.Value);
        }

        [Test]
        public void TestEdit()
        {
            GradeDTO gradeDto = new GradeDTO() { Value = 4.49, SubjectId = 1, TeacherId = 1, StudentId = 1 };
            gradeService.Edit(2, gradeDto);

            Grade dbGrade = context.Grades.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(gradeDto.Value, dbGrade.Value);
        }

        [Test]
        public void TestDelete()
        {
            Grade dbGrade = context.Grades.FirstOrDefault(s => s.Id == 2);
            gradeService.Delete(2);

            CollectionAssert.DoesNotContain(context.Grades.ToList(), dbGrade);
        }
    }
}
