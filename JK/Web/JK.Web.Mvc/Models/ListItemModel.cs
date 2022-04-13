using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Web.Mvc.Models
{
    public class ListItemModel
    {
        public string Seq{get;set;}
        public string Title{get;set;}
        public DateTime RegDate{get;set;}
        public DateTime ModifyDate {get;set;}
    }
}
