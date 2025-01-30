using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class Player
    {
        protected int m_iPlayerID;
        protected int m_XPosition;
        protected int m_YPosition;
        protected int m_ZPosition;
        protected int m_EyeLevel;
        protected int m_FOV;
        protected int m_iRotationSpeed;
        protected int m_iMoveSpeed;

        protected Angle m_Angle = new Angle(0.0f);
        protected Angle m_HalfFOV = new Angle(60.0f);
        protected ViewRenderer? m_ViewRenderer;
        protected Weapon m_Weapon;

        public Player(ViewRenderer viewRenderer, int iID)
        {
            m_ZPosition = m_EyeLevel;
            m_Weapon = new Weapon("PISGA0");
        }

        public void Init(Thing thing)
        {
            SetXPosition(thing.XPosition);
            SetYPosition(thing.YPosition);
            SetAngle(thing.Angle);
            m_HalfFOV = new Angle(m_FOV / 2);
        }
        public void SetXPosition(int xPosition)
        {
            m_XPosition = xPosition;
        }

        public void SetYPosition(int yPosition)
        {
            m_YPosition = yPosition;
        }

        public void SetZPosition(int zPosition)
        {
            m_ZPosition = zPosition;
        }

        public void SetAngle(int angle)
        {
            m_Angle = new Angle(angle);
        }

        public void MoveForward()
        {
            m_XPosition += (int)MathF.Round(m_Angle.GetCosValue()) * m_iMoveSpeed;
            m_YPosition += (int)MathF.Round(m_Angle.GetSinValue()) * m_iMoveSpeed;
        }

        public void MoveLeftward()
        {
            m_XPosition -= (int)MathF.Round(m_Angle.GetCosValue()) * m_iMoveSpeed;
            m_YPosition -= (int)MathF.Round(m_Angle.GetSinValue()) * m_iMoveSpeed;
        }
        public void RotateLeft()
        {
            float angle = m_Angle.GetValue() + 0.1875f * m_iRotationSpeed;
            m_Angle = new Angle(angle);
        }
        public void RotateRight()
        {
            float angle = m_Angle.GetValue() - 0.1875f * m_iRotationSpeed;
            m_Angle = new Angle(angle);
        }
        public void Fly()
        {
            m_ZPosition += 1;
        }
        public void Sink()
        {
            m_ZPosition -= 1;
        }
        public void Think(int iSubSectorHieght)
        {
            m_ZPosition = iSubSectorHieght + m_EyeLevel;
        }
        public void Render(byte[] pScreenBuffer, int iBufferPitch)
        {
            m_Weapon.Render(pScreenBuffer, iBufferPitch);
        }

        public int GetID() => m_iPlayerID;
        public int GetXPosition() => m_XPosition;
        public int GetYPosition() => m_YPosition;
        public int GetZPosition() => m_ZPosition;
        public int GetFOV() => m_FOV;
        public Angle GetAngle() => m_Angle;

        public bool ClipVertexesInFOV(Vertex v1, Vertex v2, ref Angle v1Angle, ref Angle v2Angle, ref Angle v1AngleFromPlayer, ref Angle v2AngleFromPlayer)
        {
            v1Angle = AngleToVertex(v1);
            v2Angle = AngleToVertex(v2);

            Angle v1ToV2Span = v1Angle - v2Angle;

            if (v1ToV2Span >= 180)
            {
                return false;
            }

            // Rotate every thing.
            v1AngleFromPlayer = v1Angle - m_Angle;
            v2AngleFromPlayer = v2Angle - m_Angle;

            // Validate and Clip V1
            // shift angles to be between 0 and 90 (now virtualy we shifted FOV to be in that range)
            Angle v1Moved = v1AngleFromPlayer + m_HalfFOV;

            if (v1Moved > m_FOV)
            {
                // now we know that V1, is outside the left side of the FOV
                // But we need to check is Also V2 is outside.
                // Lets find out what is the size of the angle outside the FOV
                Angle V1MovedAngle = new Angle(v1Moved.GetValue() - m_FOV);

                // Are both V1 and V2 outside?
                if (V1MovedAngle >= v1ToV2Span)
                {
                    return false;
                }

                // At this point V2 or part of the line should be in the FOV.
                // We need to clip the V1
                v1AngleFromPlayer = m_HalfFOV;
            }

            // Validate and Clip V2
            Angle V2Moved = m_HalfFOV - v2AngleFromPlayer;

            // Is V2 outside the FOV?
            if (V2Moved > m_FOV)
            {
                v2AngleFromPlayer = -m_HalfFOV;
            }

            float v1ang = v1AngleFromPlayer.GetValue() + 90;
            v1AngleFromPlayer = new Angle(v1ang);

            float v2ang = v2AngleFromPlayer.GetValue() + 90;
            v2AngleFromPlayer = new Angle(v2ang);

            return true;
        }

        // Calulate the distance between the player an the vertex.
        public float DistanceToPoint(Vertex v)
        {
            // We have two points, where the player is and the vertex passed.
            // To calculate the distance just use "The Distance Formula"
            // distance = square root ((X2 - X1)^2 + (y2 - y1)^2)
            return MathF.Sqrt(MathF.Pow(m_XPosition - v.XPosition, 2) + MathF.Pow(m_YPosition - v.YPosition, 2));
        }

        public Angle AngleToVertex(Vertex vertex)
        {
            float Vdx = vertex.XPosition - m_XPosition;
            float Vdy = vertex.YPosition - m_YPosition;

            Angle VertexAngle = new Angle(MathF.Atan2(Vdy, Vdx)* 180.0f / MathF.PI);

            return VertexAngle;
        }
    }
}
