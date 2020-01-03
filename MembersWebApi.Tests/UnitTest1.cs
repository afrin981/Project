using System;
using System.Collections.Generic;
using MembersWebApi.Controllers;
using MembersWebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MembersWebApi.Tests
{
    [TestClass]
    public class MembersUnitTest
    {
        [TestMethod]
        public void GetAllMembers()
        {
            var testMember = GetTestMembers();
            var controller = new MembersController(testMember);
            var result = controller.GetMembers() as List<Member>;
            Assert.AreEqual(testMember.Count, result.Count);
        }
        private List<Member> GetTestMembers()
        {
            var testMember = new List<Member>();
            testMember.Add(new Member { FirstName = "Demo1", LastName = "Demo2", DateOfBirth = Convert.ToDateTime("01/01/2001"), Email = "afrin.aman@infosys.com" });
            testMember.Add(new Member { FirstName = "Demo3", LastName = "Demo4", DateOfBirth = Convert.ToDateTime("01/10/2000"), Email = "afrin.aman1@infosys.com" });
            return testMember;
        }

    }
}
