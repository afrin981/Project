using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MembersWebApi.Models;

namespace MembersWebApi.Controllers
{
    public class MembersController : ApiController
    {
        List<Member> members = new List<Member>();

        public MembersController() { }

        public MembersController(List<Member> members)
        {
            this.members = members;
        }
        private MemberEntities db = new MemberEntities();

        // GET: api/Members
        [ResponseType(typeof(IEnumerable<Member>))]
        [Route("api/GetMember")]
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // POST: api/Members
        [ResponseType(typeof(Member))]
        [Route("api/PostMember", Name ="PostMember")]
        public IHttpActionResult PostMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Members.Add(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberExists(member.MemberID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("PostMember", new { id = member.MemberID }, member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberID == id) > 0;
        }

        // GET: api/Members/5
        [ResponseType(typeof(Member))]
        [Route("api/GetOldestMember")]
        public Member GetOldestMember()
        {
            Member objOldestMember = db.Members.OrderBy(e => e.DateOfBirth).FirstOrDefault();
            return objOldestMember;
        }
    }
}