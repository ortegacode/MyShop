using MyShop.Core.Contracts; //added 
using MyShop.Core.Models;  // added
using System;
using System.Collections.Generic;
using System.Data.Entity;  // added
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T>/* genericsplaceholder <T>*/ : IRepository<T> where T : BaseEntity // had to implement IRepository<T> where T calls on BaseEntity- error in  IRepository rt click - implement interface
    {
        internal DataContext context; // calls out to the DataContext class
        internal DbSet<T> dbSet;       // calls out the list of tables in DataContest under DbSet

        public SQLRepository(DataContext context) // passed in DataContext to our constructor
        {
            this.context = context; // this.context = context we are passing in (context)
            this.dbSet = context.Set<T>(); // set the underlying table (this.dbSet)by referencing the context and calling the set (.SET)command passing in the model we want to work against (<T>)
        }

        public IQueryable<T> Collection()
        {
            //throw new NotImplementedException(); // deleted
            return dbSet;  
        }

        public void Commit()
        {
            //throw new NotImplementedException(); //deleted
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            //throw new NotImplementedException(); // deleted
            var t = Find(Id); // find the object based on its id using the Find()
            if (context.Entry(t).State == EntityState.Detached) // checks the state of the entry
                dbSet.Attach(t); // attaches object(t)

            dbSet.Remove(t); //removes object(t)
           
        }

        public T Find(string Id)
        {
            //throw new NotImplementedException(); // deleted
            return dbSet.Find(Id); // passes thru the id to the underlying context using dbset.Find()
        }

        public void Insert(T t)
        {
            //throw new NotImplementedException(); //deleted
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            //throw new NotImplementedException(); // deleted
            dbSet.Attach(t); // need to attach the object(t) because EF caches data and doesnt immediately write it to the db so we need to explicitly tell it to do it
            context.Entry(t).State = EntityState.Modified; // set that entry(t) to a state of modified. tells EF that when we call the save changes () to look for this object (t) and save it
        }
    }
}
