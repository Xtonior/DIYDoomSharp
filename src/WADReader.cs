using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class WADReader
    {
        public void ReadHeaderData(byte[] WADData, int offset, out Header header)
        {
            //0x00 to 0x03
            string c;

            header = new Header();

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

        public void ReadDirectoryData(byte[] WADData, long offset, WADDirectory directory)
        {
            //0x00 to 0x03
            directory.LumpOffset = Read4Bytes(WADData, offset);

            //0x04 to 0x07
            directory.LumpSize = Read4Bytes(WADData, offset + 4);

            //0x08 to 0x0F
            for (int i = 0; i < 8; i++)
            {
                directory.LumpName += WADData[offset + 8 + i].ToString();
            }
        }

        public void ReadVertexData(byte[] WADData, long offset, out Vertex vertex)
        {
            //0x00 to 0x01
            vertex.XPosition = Read2Bytes(WADData, offset);

            //0x02 to 0x03
            vertex.YPosition = Read2Bytes(WADData, offset + 2);
        }

        public void ReadSectorData(byte[] WADData, long offset, out WADSector sector)
        {
            sector = new WADSector();

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

        public void ReadSidedefData(byte[] WADData, long offset, out WADSidedef sidedef)
        {
            sidedef = new WADSidedef();

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

        public void ReadLinedefData(byte[] WADData, long offset, out WADLinedef linedef)
        {
            linedef.StartVertexID = Read2Bytes(WADData, offset);
            linedef.EndVertexID = Read2Bytes(WADData, offset + 2);
            linedef.Flags = Read2Bytes(WADData, offset + 4);
            linedef.LineType = Read2Bytes(WADData, offset + 6);
            linedef.SectorTag = Read2Bytes(WADData, offset + 8);
            linedef.RightSidedef = Read2Bytes(WADData, offset + 10);
            linedef.LeftSidedef = Read2Bytes(WADData, offset + 12);
        }

        public void ReadThingData(byte[] WADData, long offset, out Thing thing)
        {
            thing.XPosition = (short)Read2Bytes(WADData, offset);
            thing.YPosition = (short)Read2Bytes(WADData, offset + 2);
            thing.Angle = Read2Bytes(WADData, offset + 4);
            thing.Type = Read2Bytes(WADData, offset + 6);
            thing.Flags = Read2Bytes(WADData, offset + 8);
        }

        public void ReadNodeData(byte[] WADData, long offset, out Node node)
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

        public void ReadSubsectorData(byte[] WADData, long offset, out Subsector subsector)
        {
            subsector.SegCount = Read2Bytes(WADData, offset);
            subsector.FirstSegID = Read2Bytes(WADData, offset + 2);
        }

        public void ReadSegData(byte[] WADData, long offset, out WADSeg seg)
        {
            seg.StartVertexID = Read2Bytes(WADData, offset);
            seg.EndVertexID = Read2Bytes(WADData, offset + 2);
            seg.SlopeAngle = Read2Bytes(WADData, offset + 4);
            seg.LinedefID = Read2Bytes(WADData, offset + 6);
            seg.Direction = Read2Bytes(WADData, offset + 8);
            seg.Offset = Read2Bytes(WADData, offset + 10);
        }

        public void ReadPalette(byte[] WADData, long offset, out WADPalette palette)
        {
            palette.Colors = new List<SDL2.SDL.SDL_Color>();
            SDL2.SDL.SDL_Color col = new SDL2.SDL.SDL_Color();

            for (int i = 0; i < 256; i++)
            {
                col.r = WADData[offset + 1];
                col.g = WADData[offset + 1];
                col.b = WADData[offset + 1];
                col.a = 255;

                palette.Colors.Add(col);
            }
        }

        public void ReadPatchHeader(byte[] WADData, long offset, out WADPatchHeader patchHeader)
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

        public void ReadName(byte[] WADData, long offset, out WADNames names)
        {
            names.NameCount = Read4Bytes(WADData, offset);
            names.NameOffset = (uint)(offset + 4);
        }

        public void ReadTextureHeader(byte[] WADData, long offset, out WADTextureHeader header)
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

        public void ReadTextureData(byte[] WADData, long offset, out WADTextureData texture)
        {
            texture = new WADTextureData();

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

        public void ReadTexturePatch(byte[] WADData, long offset, out WADTexturePatch texturePatch)
        {
            texturePatch.XOffset = (short)Read2Bytes(WADData, offset);
            texturePatch.YOffset = (short)Read2Bytes(WADData, offset + 2);
            texturePatch.PNameIndex = Read2Bytes(WADData, offset + 4);
            texturePatch.StepDir = Read2Bytes(WADData, offset + 6);
            texturePatch.ColorMap = Read2Bytes(WADData, offset + 8);
        }

        public void Read8Characters(byte[] WADData, long offset, string name)
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

        public long ReadPatchColumn(byte[] WADData, long offset, out PatchColumnData patch)
        {
            patch = new PatchColumnData();
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

        private UInt16 Read2Bytes(byte[] WADData, long offset)
        {
            return BitConverter.ToUInt16(WADData, (int)offset);
        }

        private UInt32 Read4Bytes(byte[] WADData, long offset)
        {
            return BitConverter.ToUInt32(WADData, (int)offset);
        }
    }
}
