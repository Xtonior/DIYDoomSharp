using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class Map
    {
        protected string m_Name = "";

        protected List<Vertex> m_Vertexes = new List<Vertex>();
        protected List<Sector> m_Sectors = new List<Sector>();
        protected List<Sidedef> m_Sidedefs = new List<Sidedef>();
        protected List<Linedef> m_Linedefs = new List<Linedef>();
        protected List<Seg> m_Segs = new List<Seg>();
        protected List<Subsector> m_Subsectors = new List<Subsector>();
        protected List<Node> m_Nodes = new List<Node>();

        protected List<WADSector> m_WADSectors;
        protected List<WADSidedef> m_WADSidedefs;
        protected List<WADLinedef> m_WADLinedefs;
        protected List<WADSeg> m_WADSegs;

        protected int m_XMin;
        protected int m_XMax;
        protected int m_YMin;
        protected int m_YMax;
        protected int m_iLumpIndex;

        protected Player m_Player;
        protected Things m_Things;
        protected ViewRenderer m_ViewRenderer = new ViewRenderer();

        public Map()
        {
            m_WADSectors = new List<WADSector>();
            m_WADSidedefs = new List<WADSidedef>();
            m_WADLinedefs = new List<WADLinedef>();
            m_WADSegs = new List<WADSeg>();

            m_Player = new Player(m_ViewRenderer, 0);
            m_Things = new Things();
        }

        public void Init()
        {
            BuildSectors();
            BuildSidedefs();
            BuildLinedef();
            BuildSeg();
        }

        public void AddVertex(Vertex v)
        {
            m_Vertexes.Add(v);

            if (m_XMin > v.XPosition)
            {
                m_XMin = v.XPosition;
            }
            else if (m_XMax < v.XPosition)
            {
                m_XMax = v.XPosition;
            }

            if (m_YMin > v.YPosition)
            {
                m_YMin = v.YPosition;
            }
            else if (m_YMax < v.YPosition)
            {
                m_YMax = v.YPosition;
            }
        }

        public void AddLinedef(WADLinedef l)
        {
            m_WADLinedefs.Add(l);
        }

        public void AddNode(Node node)
        {
            m_Nodes.Add(node);
        }

        public void AddSubsector(Subsector subsector)
        {
            m_Subsectors.Add(subsector);
        }

        public void AddSeg(WADSeg seg)
        {
            m_WADSegs.Add(seg);
        }

        public void AddSidedef(WADSidedef sidedef)
        {
            m_WADSidedefs.Add(sidedef);
        }

        public void AddSector(WADSector sector)
        {
            m_WADSectors.Add(sector);
        }

        public void Render3DView()
        {
            RenderBSPNodes();
        }

        public void SetLumpIndex(int iIndex)
        {
            m_iLumpIndex = iIndex;
        }

        public int GetPlayerSubSectorHieght()
        {
            int iSubsectorID = m_Nodes.Count - 1;

            while ((iSubsectorID & (Identifiers.SUBSECTORIDENTIFIER)) == 0)
            {
                bool isOnLeft = IsPointOnLeftSide(m_Player.GetXPosition(), m_Player.GetYPosition(), iSubsectorID);

                if (isOnLeft)
                {
                    iSubsectorID = m_Nodes[iSubsectorID].LeftChildID;
                }
                else
                {
                    iSubsectorID = m_Nodes[iSubsectorID].RightChildID;
                }
            }
            
            Subsector subsector = m_Subsectors[iSubsectorID & ~Identifiers.SUBSECTORIDENTIFIER];
            Seg seg = m_Segs[subsector.FirstSegID];
            return seg.RightSector.FloorHeight;
        }

        public int GetXMin() => m_XMin;

        public int GetXMax() => m_XMax;

        public int GetYMin() => m_YMin;

        public int GetYMax() => m_YMax;

        public int GetLumpIndex() => m_iLumpIndex;

        public string GetName() => m_Name;

        public Things GetThings()
        {
            return m_Things;
        }

        private void BuildSectors()
        {
            WADSector wadsector;
            Sector sector = new Sector();

            for (int i = 0; i < m_WADSectors?.Count; i++)
            {
                wadsector = m_WADSectors[i];

                sector.FloorHeight = wadsector.FloorHeight;
                sector.CeilingHeight = wadsector.CeilingHeight;
                sector.FloorTexture = wadsector.FloorTexture;
                sector.CeilingTexture = wadsector.CeilingTexture;
                sector.Lightlevel = wadsector.Lightlevel;
                sector.Type = wadsector.Type;
                sector.Tag = wadsector.Tag;

                m_Sectors.Add(sector);
            }
        }

        private void BuildSidedefs()
        {
            WADSidedef wadsidedef;
            Sidedef sidedef = new Sidedef();

            for (int i = 0; i < m_WADSidedefs?.Count; i++)
            {
                wadsidedef = m_WADSidedefs[i];

                sidedef.XOffset = wadsidedef.XOffset;
                sidedef.YOffset = wadsidedef.YOffset;
                sidedef.UpperTexture = wadsidedef.UpperTexture;
                sidedef.LowerTexture = wadsidedef.LowerTexture;
                sidedef.MiddleTexture = wadsidedef.MiddleTexture;
                sidedef.TargetSector = m_Sectors[wadsidedef.SectorID];

                m_Sidedefs.Add(sidedef);
            }
        }

        private void BuildLinedef()
        {
            WADLinedef wadlinedef;
            Linedef linedef = new Linedef();

            for (int i = 0; i < m_WADLinedefs?.Count; i++)
            {
                wadlinedef = m_WADLinedefs[i];

                linedef.StartVertex = m_Vertexes[wadlinedef.StartVertexID];
                linedef.EndVertex = m_Vertexes[wadlinedef.EndVertexID];
                linedef.Flags = wadlinedef.Flags;
                linedef.LineType = wadlinedef.LineType;
                linedef.SectorTag = wadlinedef.SectorTag;

                if (wadlinedef.RightSidedef == 0xFFFF)
                {
                    //linedef.RightSidedef = null;
                }
                else
                {
                    linedef.RightSidedef = m_Sidedefs[wadlinedef.RightSidedef];
                }

                if (wadlinedef.LeftSidedef == 0xFFFF)
                {
                    //linedef.LeftSidedef = null;
                }
                else
                {
                    linedef.LeftSidedef = m_Sidedefs[wadlinedef.LeftSidedef];
                }

                m_Linedefs.Add(linedef);
            }
        }

        private void BuildSeg()
        {
            WADSeg wadseg;
            Seg seg = new Seg();

            for (int i = 0; i < m_WADSegs?.Count; ++i)
            {
                wadseg = m_WADSegs[i];

                seg.StartVertex = m_Vertexes[wadseg.StartVertexID];
                seg.EndVertex = m_Vertexes[wadseg.EndVertexID];

                seg.SlopeAngle = new Angle(wadseg.SlopeAngle);
                seg.TargetLinedef = m_Linedefs[wadseg.LinedefID];
                seg.Direction = wadseg.Direction;
                seg.Offset = (ushort)((float)(wadseg.Offset << 16) / (1 << 16));

                Sidedef? rightSidedef;
                Sidedef? leftSidedef;

                if (seg.Direction != 0)
                {
                    rightSidedef = seg.TargetLinedef.LeftSidedef;
                    leftSidedef = seg.TargetLinedef.RightSidedef;
                }
                else
                {
                    rightSidedef = seg.TargetLinedef.RightSidedef;
                    leftSidedef = seg.TargetLinedef.LeftSidedef;
                }

                if (rightSidedef != null)
                {
                    seg.RightSector = rightSidedef.Value.TargetSector;
                }
                else
                {
                    //seg.RightSector = null;
                }

                if (leftSidedef != null)
                {
                    seg.LeftSector = leftSidedef.Value.TargetSector;
                }
                else
                {
                    //seg.LeftSector = null;
                }

                m_Segs.Add(seg);
            }
        }

        private void RenderBSPNodes()
        {
            RenderBSPNodes(m_Nodes.Count - 1);
        }

        private void RenderBSPNodes(int iNodeID)
        {
            // Masking all the bits exipt the last one
            // to check if this is a subsector
            if ((iNodeID & Identifiers.SUBSECTORIDENTIFIER) != 0)
            {
                RenderSubsector(iNodeID & (~Identifiers.SUBSECTORIDENTIFIER));
                return;
            }

            bool isOnLeft = IsPointOnLeftSide(m_Player.GetXPosition(), m_Player.GetYPosition(), iNodeID);

            if (isOnLeft)
            {
                RenderBSPNodes(m_Nodes[iNodeID].LeftChildID);
                RenderBSPNodes(m_Nodes[iNodeID].RightChildID);
            }
            else
            {
                RenderBSPNodes(m_Nodes[iNodeID].RightChildID);
                RenderBSPNodes(m_Nodes[iNodeID].LeftChildID);
            }
        }

        private void RenderSubsector(int iSubsectorID)
        {
            Subsector subsector = m_Subsectors[iSubsectorID];

            for (int i = 0; i < subsector.SegCount; i++)
            {
                Seg seg = m_Segs[subsector.FirstSegID + i];

                Angle v1Angle = new Angle(0.0f);
                Angle v2Angle = new Angle(0.0f);
                Angle v1AngleFromPlayer = new Angle(0.0f);
                Angle v2AngleFromPlayer = new Angle(0.0f);

                if (m_Player.ClipVertexesInFOV(seg.StartVertex, seg.EndVertex, ref v1Angle, ref v2Angle, ref v1AngleFromPlayer, ref v2AngleFromPlayer))
                {
                    m_ViewRenderer.AddWallInFOV(seg, V1Angle, V2Angle, V1AngleFromPlayer, V2AngleFromPlayer);
                }
            }
        }

        private bool IsPointOnLeftSide(int XPosition, int YPosition, int iNodeID)
        {
            int dx = XPosition - m_Nodes[iNodeID].XPartition;
            int dy = YPosition - m_Nodes[iNodeID].YPartition;

            return (((dx * m_Nodes[iNodeID].ChangeYPartition) - (dy * m_Nodes[iNodeID].ChangeXPartition)) <= 0);
        }
    }
}
