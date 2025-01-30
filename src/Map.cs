using DiyDoomSharp.src.DataTypes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public class Map
    {
        protected string m_Name = "";

        protected List<Vertex>? m_Vertexes;
        protected List<Sector>? m_Sectors;
        protected List<Sidedef>? m_Sidedefs;
        protected List<Linedef>? m_Linedefs;
        protected List<Seg>? m_Segs;
        protected List<Subsector>? m_Subsectors;
        protected List<Node>? m_Nodes;

        protected List<WADSector>? m_WADSectors;
        protected List<WADSidedef>? m_WADSidedefs;
        protected List<WADLinedef>? m_WADLinedefs;
        protected List<WADSeg>? m_WADSegs;

        protected int m_XMin;
        protected int m_XMax;
        protected int m_YMin;
        protected int m_YMax;
        protected int m_iLumpIndex;

        protected Player? m_Player;
        protected List<Thing>? m_Things;
        protected ViewRenderer? m_ViewRenderer;

        private void BuildSectors()
        {
        
        }

        private void BuildSidedefs()
        {
        
        }

        private void BuildLinedef()
        {
        
        }

        private void BuildSeg()
        { 
        
        }

        private void RenderBSPNodes()
        {
        
        }

        private void RenderBSPNodes(int iNodeID)
        {
        
        }

        private void RenderSubsector(int iSubsectorID)
        {
        
        }

        private bool IsPointOnLeftSide(int XPosition, int YPosition, int iNodeID)
        {

        }
    }
}
