using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace NotDelicious.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public int LinkId { get; set; }
        public virtual Link Links { get; set; }
    }
}