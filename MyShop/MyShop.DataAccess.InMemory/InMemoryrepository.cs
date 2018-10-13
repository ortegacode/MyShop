using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryrepository<T/*or anything u want*/> where T : BaseEntity // whenever we pass in an object it must be of the type BaseEntity or inherit from BaseEntity
    // also since baseEntity has an Id it means whenever we reference the Id our generic class knows what that is
    {
        ObjectCache cache = MemoryCache.Default;
        List<T/*referencing our placeholder*/> items;
        string className; // gives us an easy way to handle how we r gonna store our objects in cache because we need to tell it what the name is gonna be because it would have to be different each time

        public InMemoryrepository()
        {
            className = typeof(T).Name; // reflection command(typeof), pass to it the object we r using (T) and it returns its internal name. it gets our actual name of our class. example - so when it passes thru Product it will be Product.Name - passes thru Category it will be Category.Name

            items = cache[className] as List<T>; // Initialize our internal items class which checks any actions in our cache
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()  // Generic commit funtion that will store our actions in memory
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + " Not Found.");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + " Not Found.");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " Not Found.");
            }

        }
    }
}
