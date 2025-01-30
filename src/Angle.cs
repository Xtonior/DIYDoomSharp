namespace DiyDoomSharp.src
{
    public class Angle
    {
        protected float m_Angle = 0.0f;

        public Angle(float angle)
        {
            m_Angle = angle;
            Normalize360();
        }

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.m_Angle + b.m_Angle);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.m_Angle - b.m_Angle);
        }

        public static Angle operator -(Angle a)
        {
            return new Angle(360.0f - a.m_Angle);
        }

        public static bool operator <(Angle a, Angle b)
        {
            return a.m_Angle < b.m_Angle;
        }

        public static bool operator <(Angle a, float b)
        {
            return a.m_Angle < b;
        }

        public static bool operator <=(Angle a, Angle b)
        {
            return a.m_Angle <= b.m_Angle;
        }

        public static bool operator <=(Angle a, float b)
        {
            return a.m_Angle <= b;
        }

        public static bool operator >(Angle a, Angle b)
        {
            return a.m_Angle > b.m_Angle;
        }

        public static bool operator >(Angle a, float b)
        {
            return a.m_Angle > b;
        }

        public static bool operator >=(Angle a, Angle b)
        {
            return a.m_Angle >= b.m_Angle;
        }

        public static bool operator >=(Angle a, float b)
        {
            return a.m_Angle >= b;
        }

        public float GetValue()
        {
            return m_Angle;
        }

        public float GetCosValue()
        {
            return MathF.Cos(m_Angle * MathF.PI / 180.0f);
        }

        public float GetSinValue()
        {
            return MathF.Sin(m_Angle * MathF.PI / 180.0f);
        }

        public float GetTanValue()
        {
            return MathF.Tan(m_Angle * MathF.PI / 180.0f);
        }

        public float GetSignedValue()
        {
            if (m_Angle > 180)
            {
                return m_Angle - 360;
            }

            return m_Angle;
        }

        private void Normalize360()
        {
            m_Angle %= 360.0f;

            if (m_Angle < 0.0f)
                m_Angle += 360.0f;
        }
    }
}
