using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApiDEMO.Repository;

namespace WebApiDEMO.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Login
        public HttpResponseMessage Get()
        {
            try
            {
                var headers = Request.Headers;
                if (headers.Contains("Authorization"))
                {
                    //decode from Base 64
                    string token = headers.GetValues("Authorization").First();
                    string[] tokens = token.Split(':');
                    if (tokens.Length == 2)
                    {
                        string userName = tokens[0];
                        string password = tokens[1];
                        bool isValidLogin = UserDBContext.ValidateLogin(userName, password);
                        if (isValidLogin)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.OK, "Successful Login");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Login Credentials");
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex);
            }
        }
    }
}
