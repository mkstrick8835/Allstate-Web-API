using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDEMO.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CompleteDate { get; set; }
        public int UserID { get; set; }
        public bool TaskComplete { get; set; }
        public DateTime InsertDate { get; set; }
    }
}