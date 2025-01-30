using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public class AssetsManager
    {
        protected WADLoader? m_WADLoader = null;
        protected bool m_Initialized = false;

        protected Dictionary<string, Patch> m_PatchesCache = new Dictionary<string, Patch>();
        protected Dictionary<string, Texture> m_TexturesCache = new Dictionary<string, Texture>();

        protected List<string> m_NameLookup = new List<string>();

        public void Init(WADLoader pWADLoader)
        {
            m_WADLoader = pWADLoader;
        }

        public Patch AddPatch(string patchName, WADPatchHeader PatchHeader)
        {
            m_PatchesCache[patchName] = new Patch(patchName);
            Patch p_Patch = m_PatchesCache[patchName];

            return p_Patch;
        }

        public Patch GetPatch(string patchName)
        {
            return m_PatchesCache[patchName];
        }

        public Texture AddTexture(WADTextureData textureData)
        {
            m_TexturesCache[textureData.TextureName] = new Texture(textureData);
            Texture texture = m_TexturesCache[textureData.TextureName];

            return texture;
        }

        public Texture? GetTexture(string name)
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

        public void AddName(string name)
        {
            m_NameLookup.Add(name);
        }

        public string GetName(int index) => m_NameLookup[index];

        private void LoadPatch(string patchName)
        {
            m_WADLoader.
        }
    }
}
