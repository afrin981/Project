using MembersInformationApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MembersInformationApp.Controllers
{
    public class MembersController : Controller
    {
        private string Base_URL = "http://localhost:58122/";

        public ActionResult Index()
        {
            List<Member> MemberInfo = new List<Member>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/GetMember").Result;
                if (response.IsSuccessStatusCode)
                {
                    var MemberResponse = response.Content.ReadAsStringAsync().Result;
                    MemberInfo = JsonConvert.DeserializeObject<List<Member>>(MemberResponse);
                    response = client.GetAsync("api/GetOldestMember").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var OldestMemberResponse = response.Content.ReadAsStringAsync().Result;
                        Member objMember = JsonConvert.DeserializeObject<Member>(OldestMemberResponse);
                        int age = CalculateAge(objMember.DateOfBirth);
                        ViewBag.OldestMemberInfo = objMember.FirstName + "," + objMember.LastName + "," + age;
                    }                    
                }               
                return View(MemberInfo);
            }
        }

        public ActionResult Create(Member member)
        {
            List<Member> MemberInfo = new List<Member>();
            if (ModelState.IsValid)
            {                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Base_URL);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync("api/PostMember", member).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        response = client.GetAsync("api/GetMember").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var MemberResponse = response.Content.ReadAsStringAsync().Result;
                            MemberInfo = JsonConvert.DeserializeObject<List<Member>>(MemberResponse);
                            response = client.GetAsync("api/GetOldestMember").Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var OldestMemberResponse = response.Content.ReadAsStringAsync().Result;
                                Member objMember = JsonConvert.DeserializeObject<Member>(OldestMemberResponse);
                                int age = CalculateAge(objMember.DateOfBirth);
                                ViewBag.OldestMemberInfo = objMember.FirstName + "," + objMember.LastName + "," + age;
                            }
                        }
                    }
                }
                return View("Index", MemberInfo);
            }
            else
            {                
                return RedirectToAction("Index");
            }               
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }       
}