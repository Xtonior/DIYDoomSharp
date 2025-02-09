using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class Things
    {
        private List<Thing> m_Things = new List<Thing>();

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
