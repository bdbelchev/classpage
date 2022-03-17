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
    public class ClassServiceTests
    {
        private readonly DbConnection dbConnection;
        private readonly DbContextOptions<SchooldbContext> dbContextOptions;
        private SchooldbContext context;
        private ClassService classService;

        public ClassServiceTests()
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

            classService = new ClassService(context);
        }

        [TearDown]
        public void Dispose()
        {
            dbConnection.Dispose();
        }

        [Test]
        public void TestGetById()
        {
            Class dbClass = context.Classes.FirstOrDefault(s => s.Id == 1);
            ClassDTO classDto = classService.GetById(1);

            Assert.AreEqual(dbClass.ClassName, classDto.ClassName);
        }

        [Test]
        public void TestGetAll()
        {
            List<Class> dbClasses = context.Classes.ToList();
            List<ClassDTO> classDtos = classService.GetAll();

            Assert.AreEqual(dbClasses.First().ClassName, classDtos.First().ClassName);
        }

        [Test]
        public void TestAdd()
        {
            ClassDTO classDto = new ClassDTO() { ClassName = "Class3", MainTeacherId = 3 };
            classService.Add(classDto);

            Class dbClass = context.Classes.FirstOrDefault(s => s.Id == 3);
            Assert.AreEqual(classDto.ClassName, dbClass.ClassName);
        }

        [Test]
        public void TestEdit()
        {
            ClassDTO classDto = new ClassDTO() { ClassName = "EditClass2", MainTeacherId = 2 };
            classService.Edit(2, classDto);

            Class dbClass = context.Classes.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(classDto.ClassName, dbClass.ClassName);
        }

        [Test]
        public void TestDelete()
        {
            Class dbClass = context.Classes.FirstOrDefault(s => s.Id == 2);
            classService.Delete(2);

            CollectionAssert.DoesNotContain(context.Classes.ToList(), dbClass);
        }
    }
}
