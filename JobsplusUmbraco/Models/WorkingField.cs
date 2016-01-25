using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Models;

namespace JobsplusUmbraco.Models
{
    public class WorkingField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}