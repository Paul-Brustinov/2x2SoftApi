using _2x2SoftApi.Area.Common.Store;
using _2x2SoftApi.Area.Crm;
using _2x2SoftApi.Infrastructure;
using A2v10.Data.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace _2x2SoftApi.Mvc.Controllers
{
    [RoutePrefix("api/WorkTime")]
    public class WorkTimeController : ApiController
    {
        private readonly IDbContext _dbContext;
        private readonly PersonStore _storePerson;
        private readonly WorkTimeStore _storeWorkTime;
        private readonly WorkTimeDetailStore _storeWorkTimeDetail;
        //private readonly string _email;

        public WorkTimeController()
        {
            _dbContext = ServiceLocator.Current.GetService<IDbContext>();
            _storePerson = new PersonStore(_dbContext, "Person", "Crm");
            _storeWorkTime = new WorkTimeStore(_dbContext, "WorkTime", "Crm");
            _storeWorkTimeDetail = new WorkTimeDetailStore(_dbContext, "WorkTimeDetail", "Crm");

            //System.Web.HttpContext.Current.User.Identity.Name;
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("MyWorkTimes")]
        [HttpGet]
        [Authorize]
        public async Task<ExpandoObject> MyWorkTimes()
        {
            dynamic model = new ExpandoObject();

            model.Email = User.Identity.GetUserName();
            Person _suppPerson = await _storePerson.FindByEmailAsync(model.Email);

            IEnumerable<Person> persons = await _storePerson.GetAll();
            List<Entity> entities = new List<Entity>();
            //To restrict information sending to client, send only Id and Name
            persons.ForEach(p => { entities.Add(new Entity() { Id = p.Id, Name = p.Name }); });
            model.Persons = entities;

            IEnumerable<WorkTime> myWorkTimes = await _storeWorkTime.WorkTimesGetBySupp(_suppPerson.Id);

            myWorkTimes = myWorkTimes.OrderByDescending(x => x.OpDate);
            foreach (var myWorkTime in myWorkTimes)
            {
                myWorkTime.Client = persons.FirstOrDefault(p => p.Id == myWorkTime.ClientId);
                myWorkTime.Supporter = persons.FirstOrDefault(p => p.Id == myWorkTime.SuppId);
            }

            model.WorkTimes = myWorkTimes;
            model.Status = HttpStatusCode.OK;

            return model;
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("AddOrUpdate")]
        [HttpGet]
        [Authorize]
        public async Task<HttpResponseMessage> AddOrUpdate()
        {
            //Int64 id = 0

            Int64 id = 58645;

            //if (!User.Identity.IsAuthenticated)
            //    return Json("Need to login");
            WorkTime workTime       = await _storeWorkTime.FindByIdAsync(id); ;
            workTime.Client         = await _storePerson.FindByIdAsync(workTime.ClientId);
            workTime.Supporter      = await _storePerson.FindByIdAsync(workTime.SuppId);
            workTime.WorkTimeDetails= await _storeWorkTime.GetWorkTimeDetails(workTime.Id);
            //return workTime;
            return Request.CreateResponse(HttpStatusCode.OK, workTime);
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Delete")]
        [HttpGet]
        [Authorize]
        public async Task Delete(Int64 id = 0)
        {
            await _storeWorkTime.DeleteAsync(id);
        }


    }
}
