using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace NotDelicious.Models
{ 
    public class LinkRepository : ILinkRepository
    {
        NotDeliciousContext context = new NotDeliciousContext();

        public IQueryable<Link> All
        {
            get
            {
                return context.Links;
            }
        }

        public IQueryable<Link> AllIncluding(params Expression<Func<Link, object>>[] includeProperties)
        {
            IQueryable<Link> query = context.Links;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Link Find(int id)
        {
            return context.Links.Find(id);
        }

        public void InsertOrUpdate(Link link)
        {
            if (link.LinkId == default(int)) {
                // New entity
                context.Links.Add(link);
            } else {
                // Existing entity
                context.Entry(link).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var link = context.Links.Find(id);
            context.Links.Remove(link);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface ILinkRepository
    {
        IQueryable<Link> All { get; }
        IQueryable<Link> AllIncluding(params Expression<Func<Link, object>>[] includeProperties);
        Link Find(int id);
        void InsertOrUpdate(Link link);
        void Delete(int id);
        void Save();
    }
}