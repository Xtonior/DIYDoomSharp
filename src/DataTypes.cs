using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL3.SDL;

namespace DiyDoomSharp.src.DataTypes
{
    enum Identifiers : int
    {
        // Subsector Identifier is the 16th bit which
        // indicate if the node ID is a subsector.
        // The node ID is stored as uint16
        // 0x8000 in binary 1000000000000000
        SUBSECTORIDENTIFIER = 0x8000
    }

    public enum EMAPLUMPSINDEX
    {
        eName,
        eTHINGS,
        eLINEDEFS,
        eSIDEDDEFS,
        eVERTEXES,
        eSEAGS,
        eSSECTORS,
        eNODES,
        eSECTORS,
        eREJECT,
        eBLOCKMAP,
        eCOUNT
    }

    public enum ELINEDEFFLAGS
    {
        eBLOCKING = 0,
        eBLOCKMONSTERS = 1,
        eTWOSIDED = 2,
        eDONTPEGTOP = 4,
        eDONTPEGBOTTOM = 8,
        eSECRET = 16,
        eSOUNDBLOCK = 32,
        eDONTDRAW = 64,
        eDRAW = 128
    }

    public struct Header
    {
        public string WADType;
        public uint DirectoryCount;
        public uint DirectoryOffset;
    }

    public struct Directory
    {
        public UInt32 LumpOffset;
        public UInt32 LumpSize;
        public string LumpName;
    }

    public struct Thing
    {
        public Int16 XPosition;
        public Int16 YPosition;
        public UInt16 Angle;
        public UInt16 Type;
        public UInt16 Flags;
    }

    public struct Vertex
    {
        public UInt16 XPosition;
        public UInt16 YPosition;
    }

    public struct WADSector
    {
        public UInt16 FloorHeight;
        public UInt16 CeilingHeight;
        public string FloorTexture;
        public string CeilingTexture;
        public UInt16 Lightlevel;
        public UInt16 Type;
        public UInt16 Tag;
    }

    public struct Sector
    {
        public UInt16 FloorHeight;
        public UInt16 CeilingHeight;
        public string FloorTexture;
        public string CeilingTexture;
        public UInt16 Lightlevel;
        public UInt16 Type;
        public UInt16 Tag;
    }

    public struct WADSidedef
    {
        public UInt16 XOffset;
        public UInt16 YOffset;
        public string UpperTexture;
        public string LowerTexture;
        public string MiddleTexture;
        public UInt16 SectorID;
    }

    public struct Sidedef
    {
        public UInt16 XOffset;
        public UInt16 YOffset;
        public string UpperTexture;
        public string LowerTexture;
        public string MiddleTexture;
        Sector pSector;
    }

    public struct WADLinedef
    {
        public UInt16 StartVertexID;
        public UInt16 EndVertexID;
        public UInt16 Flags;
        public UInt16 LineType;
        public UInt16 SectorTag;
        public UInt16 RightSidedef; //0xFFFF means there is no sidedef
        public UInt16 LeftSidedef;  //0xFFFF means there is no sidedef
    }

    public struct Linedef
    {
        public Vertex StartVertex;
        public Vertex EndVertex;
        public UInt16 Flags;
        public UInt16 LineType;
        public UInt16 SectorTag;
        public Sidedef RightSidedef;
        public Sidedef LeftSidedef;
    }

    public struct WADPatchHeader
    {
        public Int16 Width;
        public Int16 Height;
        public Int16 LeftOffset;
        public Int16 TopOffset;
        public Int32[] ColumnOffsets;
    }

    public struct PatchColumnData
    {
        public byte TopDelta;
        public byte Length;
        public byte PaddingPre;
        public byte[] ColumnData;
        public byte PaddingPost;
    }

    public struct WADNames
    {
        public UInt32 NameCount;
        public UInt32 NameOffset;
    }

    public struct WADTextureHeader
    {
        public UInt32 TexturesCount;
        public UInt32 TexturesOffset;
        public UInt32[] TexturesDataOffset;
    }

    public struct WADTexturePatch
    {
        public Int16 XOffset;
        public Int16 YOffset;
        public UInt16 PNameIndex;
        public UInt16 StepDir; // Unused value.
        public UInt16 ColorMap; // Unused value.
    }

    public struct WADTextureData
    {
        public string TextureName;
        public Int32 Flags;
        public Int16 Width;
        public Int16 Height;
        public Int32 ColumnDirectory; // Unused value.
        public Int16 PatchCount;
        public WADTexturePatch[] TexturePatches;
    }

    public struct WADSeg
    {
        public UInt16 StartVertexID;
        public UInt16 EndVertexID;
        public UInt16 SlopeAngle;
        public UInt16 LinedefID;
        public UInt16 Direction; // 0 same as linedef, 1 opposite of linedef
        public UInt16 Offset; // distance along linedef to start of seg
    }

    public struct Seg
    {
        public Vertex pStartVertex;
        public Vertex pEndVertex;
        public Angle SlopeAngle;
        public Linedef pLinedef;
        public UInt16 Direction; // 0 same as linedef, 1 opposite of linedef
        public UInt16 Offset; // distance along linedef to start of seg    
        public Sector pRightSector;
        public Sector pLeftSector;
    }

    public struct Subsector
    {
        public UInt16 SegCount;
        public UInt16 FirstSegID;
    }

    public struct Node
    {
        public Int16 XPartition;
        public Int16 YPartition;
        public Int16 ChangeXPartition;
        public Int16 ChangeYPartition;

        public Int16 RightBoxTop;
        public Int16 RightBoxBottom;
        public Int16 RightBoxLeft;
        public Int16 RightBoxRight;

        public Int16 LeftBoxTop;
        public Int16 LeftBoxBottom;
        public Int16 LeftBoxLeft;
        public Int16 LeftBoxRight;

        public UInt16 RightChildID;
        public UInt16 LeftChildID;
    }

    public struct WADPalette
    {
        public List<SDL_Color> Colors;
    }
}
