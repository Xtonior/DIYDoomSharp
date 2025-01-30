using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiyDoomSharp.src
{
    public class WADReader
    {
        public void ReadHeaderData(byte[] WADData, int offset, Header header)
        {
            //0x00 to 0x03
            string c;

            for (int i = 0; i < 4; i++)
            {
                c = WADData[offset + i].ToString();
                header.WADType += c;
            }

            //0x04 to 0x07
            header.DirectoryCount = Read4Bytes(WADData, offset + 4);

            //0x08 to 0x0b
            header.DirectoryCount = Read4Bytes(WADData, offset + 8);
        }

        public void ReadDirectoryData(byte[] WADData, int offset, DataTypes.Directory directory)
        {
            //0x00 to 0x03
            directory.LumpOffset = Read4Bytes(WADData, offset);

            //0x04 to 0x07
            directory.LumpSize = Read4Bytes(WADData, offset + 4);

            //0x08 to 0x0F
            string c;

            for (int i = 0; i < 8; i++)
            {
                directory.LumpName += WADData[offset + 8 + i].ToString();
            }
        }

        public void ReadVertexData(byte[] WADData, int offset, Vertex vertex)
        {
            //0x00 to 0x01
            vertex.XPosition = Read2Bytes(WADData, offset);

            //0x02 to 0x03
            vertex.YPosition = Read2Bytes(WADData, offset + 2);
        }

        public void ReadSectorData(byte[] WADData, int offset, WADSector sector)
        {
            sector.FloorHeight = Read2Bytes(WADData, offset);
            sector.CeilingHeight = Read2Bytes(WADData, offset + 2);

            string c;

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + 4 + i].ToString();
                sector.FloorTexture += c;
            }

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + 12 + i].ToString();
                sector.CeilingTexture += c;
            }

            sector.Lightlevel = Read2Bytes(WADData, offset + 20);
            sector.Type = Read2Bytes(WADData, offset + 22);
            sector.Tag = Read2Bytes(WADData, offset + 24);
        }

        public void ReadSidedefData(byte[] WADData, int offset, WADSidedef sidedef)
        {
            sidedef.XOffset = Read2Bytes(WADData, offset);
            sidedef.YOffset = Read2Bytes(WADData, offset + 2);

            string c;

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + 4 + i].ToString();
                sidedef.UpperTexture += c;
            }

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + 12 + i].ToString();
                sidedef.LowerTexture += c;
            }

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + 20 + i].ToString();
                sidedef.MiddleTexture += c;
            }

            sidedef.SectorID = Read2Bytes(WADData, offset + 28);
        }

        public void ReadLinedefData(byte[] WADData, int offset, WADLinedef linedef)
        {
            linedef.StartVertexID = Read2Bytes(WADData, offset);
            linedef.EndVertexID = Read2Bytes(WADData, offset + 2);
            linedef.Flags = Read2Bytes(WADData, offset + 4);
            linedef.LineType = Read2Bytes(WADData, offset + 6);
            linedef.SectorTag = Read2Bytes(WADData, offset + 8);
            linedef.RightSidedef = Read2Bytes(WADData, offset + 10);
            linedef.LeftSidedef = Read2Bytes(WADData, offset + 12);
        }

        public void ReadThingData(byte[] WADData, int offset, Thing thing)
        {
            thing.XPosition = (short)Read2Bytes(WADData, offset);
            thing.YPosition = (short)Read2Bytes(WADData, offset + 2);
            thing.Angle = Read2Bytes(WADData, offset + 4);
            thing.Type = Read2Bytes(WADData, offset + 6);
            thing.Flags = Read2Bytes(WADData, offset + 8);
        }

        public void ReadNodeData(byte[] WADData, int offset, Node node)
        {
            node.XPartition = (short)Read2Bytes(WADData, offset);
            node.YPartition = (short)Read2Bytes(WADData, offset + 2);
            node.ChangeXPartition = (short)Read2Bytes(WADData, offset + 4);
            node.ChangeYPartition = (short)Read2Bytes(WADData, offset + 6);

            node.RightBoxTop = (short)Read2Bytes(WADData, offset + 8);
            node.RightBoxBottom = (short)Read2Bytes(WADData, offset + 10);
            node.RightBoxLeft = (short)Read2Bytes(WADData, offset + 12);
            node.RightBoxRight = (short)Read2Bytes(WADData, offset + 14);

            node.LeftBoxTop = (short)Read2Bytes(WADData, offset + 16);
            node.LeftBoxBottom = (short)Read2Bytes(WADData, offset + 18);
            node.LeftBoxLeft = (short)Read2Bytes(WADData, offset + 20);
            node.LeftBoxRight = (short)Read2Bytes(WADData, offset + 22);

            node.RightChildID = Read2Bytes(WADData, offset + 24);
            node.LeftChildID = Read2Bytes(WADData, offset + 26);
        }

        public void ReadSubsectorData(byte[] WADData, int offset, Subsector subsector)
        {
            subsector.SegCount = Read2Bytes(WADData, offset);
            subsector.FirstSegID = Read2Bytes(WADData, offset + 2);
        }

        public void ReadSegData(byte[] WADData, int offset, WADSeg seg)
        {
            seg.StartVertexID = Read2Bytes(WADData, offset);
            seg.EndVertexID = Read2Bytes(WADData, offset + 2);
            seg.SlopeAngle = Read2Bytes(WADData, offset + 4);
            seg.LinedefID = Read2Bytes(WADData, offset + 6);
            seg.Direction = Read2Bytes(WADData, offset + 8);
            seg.Offset = Read2Bytes(WADData, offset + 10);
        }

        public void ReadPalette(byte[] WADData, int offset, WADPalette palette)
        {
            palette.Colors = new List<SDL3.SDL.SDL_Color>();
            SDL3.SDL.SDL_Color col = new SDL3.SDL.SDL_Color();

            for (int i = 0; i < 256; i++)
            {
                col.r = WADData[offset + 1];
                col.g = WADData[offset + 1];
                col.b = WADData[offset + 1];
                col.a = 255;

                palette.Colors.Add(col);
            }
        }

        public void ReadPatchHeader(byte[] WADData, int offset, WADPatchHeader patchHeader)
        {
            //0x00 to 0x01
            patchHeader.Width = (short)Read2Bytes(WADData, offset);

            //0x02 to 0x03
            patchHeader.Height = (short)Read2Bytes(WADData, offset + 2);

            patchHeader.LeftOffset = (short)Read2Bytes(WADData, offset + 4);
            patchHeader.TopOffset = (short)Read2Bytes(WADData, offset + 6);

            patchHeader.ColumnOffsets = new Int32[patchHeader.Width];

            offset = offset + 8;

            for (int i = 0; i < patchHeader.Width; ++i)
            {
                patchHeader.ColumnOffsets[i] = (int)Read4Bytes(WADData, offset);
                offset += 4;
            }
        }

        public void ReadName(byte[] WADData, int offset, WADNames names)
        {
            names.NameCount = Read4Bytes(WADData, offset);
            names.NameOffset = (uint)(offset + 4);
        }

        public void ReadTextureHeader(byte[] WADData, int offset, WADTextureHeader header)
        {
            header.TexturesCount = Read4Bytes(WADData, offset);
            header.TexturesOffset = Read4Bytes(WADData, offset + 4);
            header.TexturesDataOffset = new UInt32[header.TexturesCount];

            offset = offset + 4;

            for (int i = 0; i < header.TexturesCount; ++i)
            {
                header.TexturesDataOffset[i] = Read4Bytes(WADData, offset);
                offset += 4;
            }
        }

        public void ReadTextureData(byte[] WADData, int offset, WADTextureData texture)
        {
            string c;

            for (int i = 0; i < 8; i++)
            {
                c = WADData[offset + i].ToString();
                texture.TextureName += c;
            }

            texture.Flags = (int)Read4Bytes(WADData, offset + 8);
            texture.Width = (short)Read2Bytes(WADData, offset + 12);
            texture.Height = (short)Read2Bytes(WADData, offset + 14);
            texture.ColumnDirectory = (int)Read4Bytes(WADData, offset + 16);
            texture.PatchCount = (short)Read2Bytes(WADData, offset + 20);
            texture.TexturePatches = new WADTexturePatch[texture.PatchCount];
        }

        public void ReadTexturePatch(byte[] WADData, int offset, WADTexturePatch texturePatch)
        {
            texturePatch.XOffset = (short)Read2Bytes(WADData, offset);
            texturePatch.YOffset = (short)Read2Bytes(WADData, offset + 2);
            texturePatch.PNameIndex = Read2Bytes(WADData, offset + 4);
            texturePatch.StepDir = Read2Bytes(WADData, offset + 6);
            texturePatch.ColorMap = Read2Bytes(WADData, offset + 8);
        }

        public void Read8Characters(byte[] WADData, int offset, string name)
        {
            name += WADData[offset++];  // [0]
            name += WADData[offset++];  // [1]
            name += WADData[offset++];  // [2]
            name += WADData[offset++];  // [3]
            name += WADData[offset++];  // [4]
            name += WADData[offset++];  // [5]
            name += WADData[offset++];  // [6]
            name += WADData[offset];    // [7]
        }

        public int ReadPatchColumn(byte[] WADData, int offset, PatchColumnData patch)
        {
            patch.TopDelta = WADData[offset++];

            if (patch.TopDelta != 0xFF)
            {
                patch.Length = WADData[offset++];
                patch.PaddingPre = WADData[offset++];

                patch.ColumnData = new byte[patch.Length];

                for (int i = 0; i < patch.Length; ++i)
                {
                    patch.ColumnData[i] = WADData[offset++];
                }

                patch.PaddingPost = WADData[offset++];
            }

            return offset;
        }

        private UInt16 Read2Bytes(byte[] WADData, int offset)
        {
            return BitConverter.ToUInt16(WADData, offset);
        }

        private UInt32 Read4Bytes(byte[] WADData, int offset)
        {
            return BitConverter.ToUInt32(WADData, offset);
        }
    }
}
