using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public class WADLoader
    {
        protected string? m_WADFilePath;
        protected FileInfo? m_WADFile;       // File?
        protected List<string>? m_WADDirectories;
        protected byte[]? m_WADData;

        protected WADReader m_Reader = new WADReader();

        public void SetWADFilePath(string filePath)
        {
            m_WADFilePath = filePath;
        }

        public bool LoadWADToMemory()
        {
            return OpenAndLoad() || ReadDirectories();
        }

        public bool LoadMapData(Map map)
        {
            Console.WriteLine($"Info: Parsing Map: {map.GetName()}");

            Console.WriteLine("Info: Processing Map Vertexes");
            
            if (!ReadMapVertexes(map))
            {
                Console.WriteLine($"Error: Failed to load map vertexes data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Sectors");

            if (!ReadMapSectors(map))
            {
                Console.WriteLine($"Error: Failed to load map sectors data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Sidedefs");

            if (!ReadMapSidedefs(map))
            {
                Console.WriteLine($"Error: Failed to load map sidedefs data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Linedefs");

            if (!ReadMapLinedefs(map))
            {
                Console.WriteLine($"Error: Failed to load map linedefs data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Segs");

            if (!ReadMapSegs(map))
            {
                Console.WriteLine($"Error: Failed to load map segs data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Things");

            if (!ReadMapThings(map))
            {
                Console.WriteLine($"Error: Failed to load map things data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Nodes");

            if (!ReadMapNodes(map))
            {
                Console.WriteLine($"Error: Failed to load map nodes data MAP: {map.GetName()}");
                return false;
            }

            Console.WriteLine("Info: Processing Map Subsectors");

            if (!ReadMapSubsectors(map))
            {
                Console.WriteLine($"Error: Failed to load map subsectors data MAP: {map.GetName()}");
                return false;
            }
        }

        public bool LoadPalette(DisplayManager displayManager)
        {
            Console.WriteLine("Info: Loading PLAYPAL (Color Palettes)");

            int iPlaypalIndex = FindLumpByName("PLAYPAL");

            if (m_WADDirectories[iPlaypalIndex].LumpName != "PLAYPAL") 
                return false;

            WADPalette palette;

            for (int i = 0; i < 14; i++)
            {
                m_Reader.ReadPalette(m_WADData, m_WADDirectories[iPlaypalIndex].LumpOffset + (i * 3 * 256), out palette);
                displayManager.AddColorPalette(ref palette);
            }

            return true;
        }

        public bool LoadPatch(string patchName)
        {
            int iPatchIndex = FindLumpByName(patchName);

            if (m_WADDirectories[iPatchIndex].LumpName != patchName) 
                return false;

            WADPatchHeader patchHeader;
            m_Reader.ReadPatchHeader(m_WADData, m_WADDirectories[iPatchIndex].LumpOffset, out patchHeader);

            Patch patch = AssetsManager.AddPatch(patchName, ref patchHeader);

            PatchColumnData patchColumnData;

            for (int i = 0; i < patchHeader.Width; ++i)
            {
                int Offset = m_WADDirectories[iPatchIndex].LumpOffset + patchHeader.ColumnOffsets[i];
                patch.AppendColumnStartIndex();
                do
                {
                    Offset = m_Reader.ReadPatchColumn(m_WADData, Offset, out patchColumnData);
                    patch.AppendPatchColumn(patchColumnData);
                } while (patchColumnData.TopDelta != 0xFF);
            }

            return true;
        }

        public bool LoadTextures(string textureName)
        {
            int iTextureIndex = FindLumpByName(textureName);

            if (iTextureIndex < 0)
                return false;

            if (m_WADDirectories[iTextureIndex].LumpName != textureName)
                return false;

            WADTextureHeader textureHeader;
            m_Reader.ReadTextureHeader(m_WADData, m_WADDirectories[iTextureIndex].LumpOffset, out textureHeader);

            WADTextureData textureData;
            for (int i = 0; i < textureHeader.TexturesCount; ++i)
            {
                m_Reader.ReadTextureData(m_WADData, m_WADDirectories[iTextureIndex].LumpOffset + textureHeader.TexturesDataOffset[i], out textureData);
                AssetsManager.AddTexture(ref textureData);

                textureData = default; // clear textureData
            }

            textureHeader = default;
            return true;
        }

        public bool LoadPNames()
        {
            int iPNameIndex = FindLumpByName("PNAMES");
            if (m_WADDirectories[iPNameIndex].LumpName  != "PNAMES")
                return false;

            WADNames pNames;
            m_Reader.ReadName(m_WADData, m_WADDirectories[iPNameIndex].LumpOffset, out pNames);
            
            string name = "";

            for (int i = 0; i < pNames.NameCount; ++i)
            {
                m_Reader.Read8Characters(m_WADData, (int)pNames.NameOffset, name);
                AssetsManager.AddName(name);
                pNames.NameOffset += 8;
            }

            return true;
        }

        private bool OpenAndLoad()
        {
            Console.WriteLine($"Info: Loading WAD file: {m_WADFilePath}");

            if (!File.Exists(m_WADFilePath))
            {
                Console.WriteLine($"Error: Failed to open WAD file {m_WADFilePath}");
                return false;
            }

            try
            {
                m_WADData = File.ReadAllBytes(m_WADFilePath);
                Console.WriteLine("Info: Loading complete.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        private bool ReadDirectories()
        {
            Header header;

            
        }

        private bool ReadMapVertexes(Map map)
        {
        
        }

        private bool ReadMapLinedefs(Map map)
        {
        
        }

        private bool ReadMapThings(Map map)
        {
        
        }

        private bool ReadMapNodes(Map map)
        {
        
        }

        private bool ReadMapSubsectors(Map map)
        {
        
        }

        private bool ReadMapSectors(Map map)
        {
        
        }

        private bool ReadMapSidedefs(Map map)
        {
        
        }

        private bool ReadMapSegs(Map map)
        {
        
        }

        private int FindMapIndex(Map map)
        {
        
        }

        private int FindLumpByName(string lumpName)
        {
        
        }
    }
}
