﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Cache : DbContext
    {
        public Cache()
            : base("name=Cache")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Data> Data { get; set; }
    
        public virtual int Add(string search, string response)
        {
            var searchParameter = search != null ?
                new ObjectParameter("search", search) :
                new ObjectParameter("search", typeof(string));
    
            var responseParameter = response != null ?
                new ObjectParameter("response", response) :
                new ObjectParameter("response", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Add", searchParameter, responseParameter);
        }
    }
}
