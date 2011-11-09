using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace NotDelicious.Models
{
    public class Link
    {
        public int LinkId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}