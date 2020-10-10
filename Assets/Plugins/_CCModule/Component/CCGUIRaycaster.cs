using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    public class CCGUIRaycaster : GraphicRaycaster
    {
        public void DefaultSetOfGUI()
        {
            SetIgnoreReversedGraphics(true);
            SetBlockingObjects(BlockingObjects.None);
            SetBlockingMask(-1);
        }

        public void SetIgnoreReversedGraphics(bool isIgnore)
        {
            ignoreReversedGraphics = isIgnore; //是否过滤反转图片
        }

        public void SetBlockingObjects(BlockingObjects blockingObjects)
        {
            this.blockingObjects = blockingObjects;
        }

        public void SetBlockingMask(LayerMask layerMask)
        {
            m_BlockingMask = layerMask;
        }
    }
}