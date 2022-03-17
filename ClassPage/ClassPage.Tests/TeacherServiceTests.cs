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
    public class TeacherServiceTests
    {
        private readonly DbConnection dbConnection;
        private readonly DbContextOptions<SchooldbContext> dbContextOptions;
        private SchooldbContext context;
        private TeacherService teacherService;

        public TeacherServiceTests()
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
            }, new Teacher
            {
                Id = 2,
                FirstName = "Teacher",
                LastName = "Two",
                Email = "t2@ma.il"
            }, new Teacher
            {
                Id = 3,
                FirstName = "Teacher",
                LastName = "Three",
                Email = "t3@ma.il"
            });

            context.Subjects.AddRange(new Subject
            {
                Id = 1,
                SubjectName = "Subject1"
            }, new Subject
            {
                Id = 2,
                SubjectName = "Subject2"
            });

            context.Classes.AddRange(new Class
            {
                Id = 1,
                ClassName = "Class1",
                MainTeacherId = 1
            }, new Class
            {
                Id = 2,
                ClassName = "Class2",
                MainTeacherId = 2
            });

            context.SaveChanges();

            teacherService = new TeacherService(context);
        }

        [TearDown]
        public void Dispose()
        {
            dbConnection.Dispose();
        }

        [Test]
        public void TestGetById()
        {
            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 1);
            TeacherDTO teacherDto = teacherService.GetById(1);

            Assert.AreEqual(dbTeacher.FirstName, teacherDto.FirstName);
        }

        [Test]
        public void TestGetAll()
        {
            List<Teacher> dbTeachers = context.Teachers.ToList();
            List<TeacherDTO> teacherDtos = teacherService.GetAll();

            Assert.AreEqual(dbTeachers.First().FirstName, teacherDtos.First().FirstName);
        }

        [Test]
        public void TestAddTeacherWithNoSubjectsAndClasses()
        {
            TeacherDTO teacherDto = new TeacherDTO { FirstName = "Teacher", LastName = "Four", Email = "t4@ma.il" };
            teacherService.Add(teacherDto);

            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 4);

            Assert.AreEqual(teacherDto.FirstName, dbTeacher.FirstName);
        }

        [Test]
        public void TestAddTeacherWithSubjectsAndClasses()
        {
            TeacherDTO teacherDto = new TeacherDTO { FirstName = "Teacher", LastName = "Four", Email = "t4@ma.il", ClassIds = new List<int> { 1, 2 }, SubjectIds = new List<int> { 1, 2 } };
            teacherService.Add(teacherDto);

            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 4);

            Assert.AreEqual(teacherDto.FirstName, dbTeacher.FirstName);
        }

        [Test]
        public void TestEditWithoutSubjectsAndClasses()
        {
            TeacherDTO teacherDto = new TeacherDTO { FirstName = "EditTeacher", LastName = "Two", Email = "t2@ma.il" };
            teacherService.Edit(2, teacherDto);

            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(teacherDto.FirstName, dbTeacher.FirstName);
        }

        [Test]
        public void TestEditWithSubjectsAndClasses()
        {
            TeacherDTO teacherDto = new TeacherDTO { FirstName = "EditTeacher", LastName = "Two", Email = "t2@ma.il", ClassIds = new List<int> { 1, 2 }, SubjectIds = new List<int> { 1, 2 } };
            teacherService.Edit(2, teacherDto);

            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(teacherDto.FirstName, dbTeacher.FirstName);
        }

        [Test]
        public void TestDelete()
        {
            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 3);
            teacherService.Delete(3);

            CollectionAssert.DoesNotContain(context.Teachers.ToList(), dbTeacher);
        }

        [Test]
        public void TestAddAndGetTeacherWithSubjectsAndClasses()
        {
            TeacherDTO addTeacherDto = new TeacherDTO { FirstName = "Teacher", LastName = "Four", Email = "t4@ma.il", ClassIds = new List<int> { 1, 2 }, SubjectIds = new List<int> { 1, 2 } };
            teacherService.Add(addTeacherDto);

            TeacherDTO getTeacherDto = teacherService.GetById(4);
            Teacher dbTeacher = context.Teachers.FirstOrDefault(s => s.Id == 4);

            Assert.AreEqual(dbTeacher.ClassesTeachers.First().ClassId, getTeacherDto.ClassIds.First());
        }
    }
}
