using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebApiDEMO.Models;
using WebApiDEMO.Repository;

namespace WebApiDEMO.Controllers
{
    //Make sure the current user is authorized for access
    [UserAuthentication]
    public class UsersController : ApiController
    {
        // GET: api/Users
        public HttpResponseMessage Get()
        {
            try
            {
                var users = UserDBContext.GetAllUsers();
                if (users != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, users);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Users found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Users/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var user = UserDBContext.GetUser(id);
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Id = " + id.ToString() + " NOT found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/Users
        public HttpResponseMessage Post([FromBody]User user)
        {
            try
            {
                int result = UserDBContext.Add(user);

                if (result < 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to create new user");
                }

                //result should contain the Id of the user that was just POSTed
                user.Id = result;
                var message = Request.CreateResponse(HttpStatusCode.Created, user);
                message.Headers.Location = new Uri(Request.RequestUri + user.Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Users/5
        public HttpResponseMessage Put([FromBody]User user)
        {
            try
            {
                int result = UserDBContext.Update(user);

                if (result == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to update user data for Id = " + user.Id.ToString());
                }

                var message = Request.CreateResponse(HttpStatusCode.OK, user);
                message.Headers.Location = new Uri(Request.RequestUri + user.Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/Users/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                int result = UserDBContext.Remove(id);
                if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to remove User with Id = " + id.ToString());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
