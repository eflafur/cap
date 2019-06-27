using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap
{
    public class EntityFactory
    {
        public static T Create<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
