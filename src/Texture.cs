using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class Texture
    {
        private int m_iWidth;
        private int m_iHeight;
        private int m_iOverLapSize;

        private bool m_IsComposed;

        private string m_Name;

        private List<int> m_ColumnPatchCount;
        private List<int> m_ColumnIndex;
        private List<int> m_ColumnPatch;

        private List<WADTexturePatch> m_TexturePatches;
        private List<nuint> m_OverlapColumnData;

        public Texture(WADTextureData textureData)
        {
            m_Name = textureData.TextureName;
            m_iWidth = textureData.Width;
            m_iHeight = textureData.Height;

            m_TexturePatches = new List<WADTexturePatch>();

            for (int i = 0; i < textureData.PatchCount; i++)
            {
                m_TexturePatches.Add(textureData.TexturePatches[i]);
            }

            m_ColumnPatchCount = new List<int>();
            m_ColumnIndex = new List<int>();
            m_ColumnPatch = new List<int>();
            m_OverlapColumnData = new List<nuint>();
        }

        public bool IsComposed() => m_IsComposed;

        public bool Initialize()
        {
            for (int i = 0; i < m_TexturePatches.Count; i++)
            {
                Patch patch = AssetsManager.GetPatch(AssetsManager.GetName(m_TexturePatches[i].PNameIndex));

                int iXStart = m_TexturePatches[i].XOffset;
                int iMaxWidth = iXStart + patch.GetWidth();

                int iXIndex = iXStart;

                if (iXStart < 0)
                {
                    iXIndex = 0;
                }

                //Does this patch extend outside the Texture?
                if (iMaxWidth > m_iWidth)
                {
                    iMaxWidth = m_iWidth;
                }

                while (iXIndex < iMaxWidth)
                {
                    m_ColumnPatchCount[iXIndex]++;
                    m_ColumnPatch[iXIndex] = i/*pPatch*/;
                    m_ColumnIndex[iXIndex] = patch.GetColumnDataIndex(iXIndex - iXStart);
                    iXIndex++;
                }
            }

            return true;
        }

        public bool Compose()
        {
            Initialize();

            for (int i = 0; i < m_TexturePatches.Count; i++)
            {
                Patch patch = AssetsManager.GetPatch(AssetsManager.GetName(m_TexturePatches[i].PNameIndex));

                int iXStart = m_TexturePatches[i].XOffset;
                int iMaxWidth = iXStart + patch.GetWidth();


                int iXIndex = iXStart;

                if (iXStart < 0)
                {
                    iXIndex = 0;
                }

                //Does this patch extend outside the Texture?
                if (iMaxWidth > m_iWidth)
                {
                    iMaxWidth = m_iWidth;
                }

                while (iXIndex < iMaxWidth)
                {
                    // Does this column have more than one patch?
                    // if yes compose it, else skip it
                    if (m_ColumnPatch[iXIndex] < 0)
                    {
                        int iPatchColumnIndex = patch.GetColumnDataIndex(iXIndex - iXStart);
                        patch.ComposeColumn(m_OverlapColumnData.ToArray(), m_iHeight, iPatchColumnIndex, m_ColumnIndex[iXIndex], m_TexturePatches[i].YOffset);
                    }

                    iXIndex++;
                }
            }

            m_IsComposed = true;
            return m_IsComposed;
        }

        public void Render(nuint[] screenBuffer, int iBufferPitch, int iXScreenLocation, int iYScreenLocation)
        {
            for (int iCurrentColumnIndex = 0; iCurrentColumnIndex < m_iWidth; iCurrentColumnIndex++)
            {
                RenderColumn(screenBuffer, iBufferPitch, iXScreenLocation + iCurrentColumnIndex, iYScreenLocation, iCurrentColumnIndex);
            }
        }

        public void RenderColumn(nuint[] screenBuffer, int iBufferPitch, int iXScreenLocation, int iYScreenLocation, int iCurrentColumnIndex)
        {
            if (m_ColumnPatch[iCurrentColumnIndex] > -1)
            {
                Patch patch = AssetsManager.GetPatch(AssetsManager.GetName(m_TexturePatches[m_ColumnPatch[iCurrentColumnIndex]].PNameIndex));

                patch.RenderColumn(screenBuffer, iBufferPitch, m_ColumnIndex[iCurrentColumnIndex], iXScreenLocation, iYScreenLocation, m_iHeight, m_TexturePatches[m_ColumnPatch[iCurrentColumnIndex]].YOffset);
            }
            else
            {
                for (int iYIndex = 0; iYIndex < m_iHeight; ++iYIndex)
                {
                    screenBuffer[iBufferPitch * (iYScreenLocation + iYIndex) + iXScreenLocation] = m_OverlapColumnData[m_ColumnIndex[iCurrentColumnIndex] + iYIndex];
                }
            }
        }
    }
}