using DiyDoomSharp.src.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiyDoomSharp.src
{
    public class Texture
    {
        protected int m_iWidth;
        protected int m_iHeight;
        protected int m_iOverLapSize;

        protected bool m_IsComposed;

        protected string m_Name;

        protected List<int> m_ColumnPathCount;
        protected List<int> m_ColumnIndex;
        protected List<int> m_ColumnPatch;

        protected List<WADTexturePatch> m_TexturePathces;
        uint m_OverlapColumnData;

        public Texture(WADTextureData textureData)
        {
            m_Name = textureData.TextureName;
            m_iWidth = textureData.Width;
            m_iHeight = textureData.Height;

            for (int i = 0; i < textureData.PatchCount; i++)
            {
                m_TexturePathces.Add(textureData.TexturePatches[i]);
            }
        }


        public bool IsComposed()
        {
            return m_IsComposed;
        }

        public bool Initialize()
        {
            AssetsManager assetsManager = new AssetsManager();

            for (int i = 0; i < m_TexturePathces.Count; i++)
            {
                Patch patch = assetsManager.GetPatch(assetsManager.get)
            }
        }

        public bool Compose()
        {

        }

        public void Render(uint pScreenBuffer, int iBufferPitch, int iXScreenLocation, int iYScreenLocation)
        {

        }

        public void RenderColumn(uint pScreenBuffer, int iBufferPitch, int iXScreenLocation, int iYScreenLocation, int iCurrentColumnIndex)
        {

        }
    }
}
