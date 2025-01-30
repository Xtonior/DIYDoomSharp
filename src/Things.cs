using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public class Things
    {
        protected List<Thing> m_Things = new List<Thing>();

        public void AddTins(Thing thing)
        {
            m_Things.Add(thing);
        }

        public Thing GetThingByID(int id)
        {
            //TODO Change this code thing can return uninitialized 
            return m_Things[id];
        }
    }
}
