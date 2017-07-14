using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDEMO.Models;
using WebApiDEMO.Repository;

namespace WebApiDEMO.Controllers
{
    [UserAuthentication]
    public class UserTasksController : ApiController
    {
        // GET: api/UserTasks
        public HttpResponseMessage Get(int userid)
        {
            try
            {
                var userTasks = UserTaskDBContext.GetAllUserTasks(userid);
                if (userTasks != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, userTasks);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No User Tasks found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/UserTasks
        public HttpResponseMessage Post([FromBody]UserTask userTask)
        {
            try
            {
                int result = UserTaskDBContext.Add(userTask);

                if (result < 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to create new User Task");
                }

                //result should contain the Id of the user that was just POSTed
                userTask.Id = result;
                var message = Request.CreateResponse(HttpStatusCode.Created, userTask);
                message.Headers.Location = new Uri(Request.RequestUri + userTask.Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/UserTasks/5
        public HttpResponseMessage Put([FromBody]int id, bool isComplete)
        {
            try
            {
                int result = UserTaskDBContext.Update(id, isComplete);

                if (result == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to update User Task data for Id = " + id.ToString());
                }

                var message = Request.CreateResponse(HttpStatusCode.OK, id);
                message.Headers.Location = new Uri(Request.RequestUri + id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/UserTasks/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                int result = UserTaskDBContext.Remove(id);
                if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to remove User Task with Id = " + id.ToString());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
