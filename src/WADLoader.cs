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
            
        }

        public bool LoadPalette(DisplayManager displayManager)
        {
        
        }

        public bool LoadPatch(string patchName)
        {
        
        }

        public bool LoadTextures(string textureName)
        {
        
        }

        public bool LoadPNames()
        {
        
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
