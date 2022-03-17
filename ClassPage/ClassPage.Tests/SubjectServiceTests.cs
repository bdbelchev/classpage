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
    public class SubjectServiceTests
    {
        private readonly DbConnection dbConnection;
        private readonly DbContextOptions<SchooldbContext> dbContextOptions;
        private SchooldbContext context;
        private SubjectService subjectService;

        public SubjectServiceTests()
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
            }, new Subject
            {
                Id = 2,
                SubjectName = "Subject2"
            });

            context.SaveChanges();

            subjectService = new SubjectService(context);
        }

        [TearDown]
        public void Dispose()
        {
            dbConnection.Dispose();
        }

        [Test]
        public void TestGetById()
        {
            Subject dbSubject = context.Subjects.FirstOrDefault(s => s.Id == 1);
            SubjectDTO subjectDto = subjectService.GetById(1);

            Assert.AreEqual(dbSubject.SubjectName, subjectDto.SubjectName);
        }

        [Test]
        public void TestGetAll()
        {
            List<Subject> dbSubjects = context.Subjects.ToList();
            List<SubjectDTO> subjectDtos = subjectService.GetAll();

            Assert.AreEqual(dbSubjects.First().SubjectName, subjectDtos.First().SubjectName);
        }

        [Test]
        public void TestAdd()
        {
            SubjectDTO subjectDto = new SubjectDTO {SubjectName = "Subject3"};
            subjectService.Add(subjectDto);

            Subject dbSubject = context.Subjects.FirstOrDefault(s => s.Id == 3);
            Assert.AreEqual(subjectDto.SubjectName, dbSubject.SubjectName);
        }

        [Test]
        public void TestEdit()
        {
            SubjectDTO subjectDto = new SubjectDTO {SubjectName = "EditSubject2"};
            subjectService.Edit(2, subjectDto);

            Subject dbSubject = context.Subjects.FirstOrDefault(s => s.Id == 2);
            Assert.AreEqual(subjectDto.SubjectName, dbSubject.SubjectName);
        }

        [Test]
        public void TestDelete()
        {
            Subject dbSubject = context.Subjects.FirstOrDefault(s => s.Id == 2);
            subjectService.Delete(2);

            CollectionAssert.DoesNotContain(context.Subjects.ToList(), dbSubject);
        }
    }
}
