using DiyDoomSharp.src.DataTypes;

namespace DiyDoomSharp.src
{
    public class Patch
    {
        private int m_iHeight = 0;
        private int m_iWidth = 0;
        private int m_iXOffset = 0;
        private int m_iYOffset = 0;

        private string m_Name = "";

        private List<PatchColumnData> m_PatchData = new List<PatchColumnData>();
        private List<int> m_ColumnIndex = new List<int>();

        public Patch(string name)
        {
            m_Name = name;
        }

        public void Init(WADPatchHeader patchHeader)
        {
            m_iWidth = patchHeader.Width;
            m_iHeight = patchHeader.Height;
            m_iXOffset = patchHeader.LeftOffset;
            m_iYOffset = patchHeader.TopOffset;
        }

        public void AppendPatchColumn(PatchColumnData patchColumn)
        {
            m_PatchData.Add(patchColumn);
        }

        public void Render(UIntPtr[] screenBuffer, int iBufferPitch, int iXScreenLocation, int iYScreenLocation)
        {
            int iXIndex = 0;

            for (int iPatchColumnIndex = 0; iPatchColumnIndex  < m_PatchData.Count; iPatchColumnIndex ++)
            {
                if (m_PatchData[iPatchColumnIndex].TopDelta == 0xFF)
                {
                    iXIndex++;
                    continue;
                }

                for (int iYIndex = 0; iYIndex < m_PatchData[iPatchColumnIndex].Length; iYIndex++)
                {
                    screenBuffer[iBufferPitch * (iYScreenLocation + m_PatchData[iPatchColumnIndex].TopDelta + iYIndex) + (iXScreenLocation + iXIndex)] = m_PatchData[iPatchColumnIndex].ColumnData[iYIndex];
                }
            }
        }

        public void RenderColumn(UIntPtr[] screenBuffer, int iBufferPitch, int iColumn, int iXScreenLocation, int iYScreenLocation, int iMaxHeight, int iYOffset)
        {
            int iTotalHeight = 0;
            int iYIndex = 0;

            if (iYOffset < 0)
            {
                iYIndex = iYOffset * -1;
            }

            while (m_PatchData[iColumn].TopDelta != 0xFF && iTotalHeight < iMaxHeight)
            {
                while (iYIndex < m_PatchData[iColumn].Length && iTotalHeight < iMaxHeight)
                {
                    screenBuffer[iBufferPitch * (iYScreenLocation + m_PatchData[iColumn].TopDelta + iYIndex + iYOffset) + iXScreenLocation] = m_PatchData[iColumn].ColumnData[iYIndex];
                    iTotalHeight++;
                    iYIndex++;
                }

                iColumn++;
                iYIndex = 0;
            }
        }

        public void AppendColumnStartIndex()
        {
            m_ColumnIndex.Add(m_PatchData.Count);
        }

        public void ComposeColumn(UIntPtr[] overLapColumnData, int iHeight, int iPatchColumnIndex, int iColumnOffsetIndex, int iYOrigin)
        {
            while (m_PatchData[iPatchColumnIndex].TopDelta != 0xFF)
            {
                int iYPosition = iYOrigin + m_PatchData[iPatchColumnIndex].TopDelta;
                int iMaxRun = m_PatchData[iPatchColumnIndex].Length;

                if (iYPosition < 0)
                {
                    iMaxRun += iYPosition;
                    iYPosition = 0;
                }

                if (iYPosition + iMaxRun > iHeight)
                {
                    iMaxRun = iHeight - iYPosition;
                }

                for (int iYIndex = 0; iYIndex < iMaxRun; ++iYIndex)
                {
                    overLapColumnData[iColumnOffsetIndex + iYPosition + iYIndex] = m_PatchData[iPatchColumnIndex].ColumnData[iYIndex];
                }
                ++iPatchColumnIndex;
            }
        }

        public int GetHeight()
        {
            return m_iHeight;
        }

        public int GetWidth()
        {
            return m_iWidth;
        }

        public int GetXOffset()
        {
            return m_iXOffset;
        }

        public int GetYOffset()
        {
            return m_iYOffset;
        }

        public int GetColumnDataIndex(int iIndex)
        {
            return m_ColumnIndex[iIndex];
        }
    }
}
