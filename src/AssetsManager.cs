using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public static class AssetsManager
    {
        private static WADLoader? m_WADLoader = null;
        private static bool m_Initialized = false;

        private static Dictionary<string, Patch> m_PatchesCache = new Dictionary<string, Patch>();
        private static Dictionary<string, Texture> m_TexturesCache = new Dictionary<string, Texture>();

        private static List<string> m_NameLookup = new List<string>();

        public static void Init(WADLoader pWADLoader)
        {
            m_WADLoader = pWADLoader;
        }

        public static Patch AddPatch(string patchName, ref WADPatchHeader PatchHeader)
        {
            m_PatchesCache[patchName] = new Patch(patchName);
            Patch p_Patch = m_PatchesCache[patchName];

            return p_Patch;
        }

        public static Patch GetPatch(string patchName)
        {
            return m_PatchesCache[patchName];
        }

        public static Texture AddTexture(ref WADTextureData textureData)
        {
            m_TexturesCache[textureData.TextureName] = new Texture(textureData);
            Texture texture = m_TexturesCache[textureData.TextureName];

            return texture;
        }

        public static Texture? GetTexture(string name)
        {
            Texture texture = m_TexturesCache[name];

            if (texture == null)
            {
                return null;
            }

            if (!texture.IsComposed())
            {
                texture.Compose();
            }

            return texture;
        }

        public static void AddName(string name)
        {
            m_NameLookup.Add(name);
        }

        public static string GetName(int index) => m_NameLookup[index];

        public static void LoadPatch(string patchName)
        {
            m_WADLoader.
        }
    }
}
