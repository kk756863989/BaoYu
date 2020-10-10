using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CC
{
    public enum DrawType
    {
        LINE,
        TRIANGLES
    }

    public class GraphicSystem
    {
        private static CCPool glGrapherPool, lrGrapherPool;

        private static CCPool GetGlGrapherPool()
        {
            if (glGrapherPool != null) return glGrapherPool;

            var grapher = new GameObject("Grapher").AddComponent<GlGrapher>();

            glGrapherPool = new CCPool(grapher, 1);
            CachSystem.Add("GraphicPool", glGrapherPool);

            return glGrapherPool;
        }

        private static CCPool GetLrGrapherPool()
        {
            if (lrGrapherPool != null) return lrGrapherPool;

            var grapher = new GameObject("Grapher").AddComponent<LrGrapher>();
            var lrCamera = new GameObject("LRCamera").AddComponent<Camera>();
            var renderTexture = new RenderTexture(Screen.width, Screen.height, (int) RenderTextureFormat.ARGB2101010);

            lrCamera.clearFlags = CameraClearFlags.SolidColor;
            lrCamera.transform.position = new Vector3(0, 0, -100);
            lrCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            lrCamera.cullingMask = LayerMask.GetMask("Default");
            lrCamera.orthographic = true;
            lrCamera.orthographicSize = 5;
            lrCamera.backgroundColor = new Color(0, 0, 0, 0);
            lrCamera.targetTexture = renderTexture;

            var rm = new GameObject("GrapherRawImage").AddComponent<RawImage>();
            var canvas = rm.gameObject.AddComponent<Canvas>();

            rm.texture = renderTexture;
            rm.gameObject.layer = LayerMask.NameToLayer("UI");
            rm.transform.SetParent(GuiSystem.GetGuiCanvas().transform);
            rm.transform.SetPosition(0, 0, 0);

            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;

            var rt = rm.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);

            rt.SetScale(1, 1, 1);

            lrGrapherPool = new CCPool(grapher);
            CachSystem.Add("GraphicPool", lrGrapherPool);

            return lrGrapherPool;
        }

        public static GlGrapher Add(Color color, Material mat = null, params Vector3[] vectors)
        {
            var grapher = GetGlGrapherPool().Add(PoolScaleType.RECYCLE, Vector3.zero) as GlGrapher;

            if (grapher == null) return null;

            if (mat != null) grapher.mat = mat;
            grapher.color = color;
            grapher.drawType = DrawType.LINE;
            grapher.vectors = new List<Vector3>(vectors);
            grapher.actived = true;

            return grapher;
        }

        public static LrGrapher Add(Color color, Color endColor, float startWidth, float endWidth, int layerId = 0,
            Material mat = null, params Vector3[] vectors)
        {
            var grapher = GetLrGrapherPool().Add(PoolScaleType.RECYCLE, Vector3.zero) as LrGrapher;

            // grapher.transform.SetParent(GuiSystem.GetGUICanvas().transform);
            // grapher.gameObject.layer = layerId;

            if (grapher == null) return null;
            if (mat != null) grapher.mat = mat;

            grapher.color = color;
            grapher.endColor = color;
            grapher.startWidth = startWidth;
            grapher.endWidth = endWidth;
            grapher.drawType = DrawType.LINE;
            grapher.vectors = new List<Vector3>(vectors);
            grapher.actived = true;
            grapher.DrawLine();

            return grapher;
        }

        public static void Remove(GlGrapher grapher)
        {
            GetGlGrapherPool().Put(grapher);
        }

        public static void Remove(LrGrapher grapher)
        {
            GetLrGrapherPool().Put(grapher);
        }

        public static void Clear()
        {
            GetLrGrapherPool().Clear();
            GetGlGrapherPool().Clear();
        }

        public abstract class Grapher : MonoBehaviour, IReuseObject
        {
            public bool HasUsed { get; set; }
            public string EntityId { get; set; }
            public Material mat;
            public Color color;
            public bool actived;
            public List<Vector3> vectors;
            public DrawType drawType;

            public abstract void DrawLine();

            public virtual void Init()
            {
                if (!HasUsed)
                {
                    OnFirstUse();
                    HasUsed = true;
                }
                else
                {
                    OnReuse();
                }
            }

            public abstract void OnFirstUse();
            public abstract void OnReuse();
            public abstract void OnRecycle();

            public abstract void OnRelease();

            // 创建一个材质球
            protected void CreateMaterial()
            {
                var shader = Shader.Find("Hidden/Internal-Colored");

                mat = new Material(shader) {hideFlags = HideFlags.HideAndDontSave};
                //设置参数
                mat.SetInt("_SrcBlend", (int) BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
                //设置参数
                mat.SetInt("_Cull", (int) CullMode.Off);
                //设置参数
                mat.SetInt("_ZWrite", 0);
            }
        }

        //绘图组件
        public class GlGrapher : Grapher
        {
            void OnRenderObject()
            {
                if (!actived) return;

                //如果材质球不存在
                if (!mat) CreateMaterial();

                switch (drawType)
                {
                    default:
                        DrawLine();
                        break;
                }
            }

            public override void DrawLine()
            {
                //刷新当前材质
                mat.SetPass(0);
                //渲染入栈  在Push——Pop之间写GL代码
                GL.PushMatrix();
                //矩阵相乘，将物体坐标转化为世界坐标
                GL.LoadPixelMatrix();
                //辅助函数用来做一个正交投影变换，调用这个函数后，视野的锥型区域从(0,0,-1) to (1,1,100)。这个函数用来绘制2D图形。
                //GL.LoadOrtho();
                //设置颜色
                GL.Color(color);
                //开始画线
                GL.Begin(GL.LINES);

                for (int i = 0, l = vectors.Count - 1; i < l; i++)
                {
                    GL.Vertex3(vectors[i].x, vectors[i].y, vectors[i].z);
                    GL.Vertex3(vectors[i + 1].x, vectors[i + 1].y, vectors[i + 1].z);
                }

                //结束画线
                GL.End();
                //渲染出栈
                GL.PopMatrix();
            }

            public void AddPoint(Vector3 vec)
            {
                vectors.Add(vec);
            }

            public override void OnFirstUse()
            {
                actived = false;
            }

            public override void OnReuse()
            {
            }

            public override void OnRecycle()
            {
                actived = false;
                vectors = new List<Vector3>();
            }

            public override void OnRelease()
            {
                Destroy(gameObject);
            }
        }

        public class LrGrapher : Grapher
        {
            public LineRenderer lineRender;
            public Color endColor;
            public float startWidth = 1, endWidth = 1;

            public override void DrawLine()
            {
                //如果材质球不存在
                if (!mat) CreateMaterial();

                lineRender.sortingLayerID = gameObject.layer;
                lineRender.material = mat;
                lineRender.startColor = color;
                lineRender.endColor = endColor;
                lineRender.startWidth = startWidth;
                lineRender.endWidth = endWidth;
                lineRender.positionCount = vectors.Count;

                for (int i = 0, l = vectors.Count; i < l; i++)
                {
                    lineRender.SetPosition(i, vectors[i]);
                }
            }

            public override void OnFirstUse()
            {
                vectors = new List<Vector3>();
                lineRender = gameObject.AddComponent<LineRenderer>();
            }

            public override void OnReuse()
            {
                lineRender.positionCount = 0;
                vectors = new List<Vector3>();
            }

            public override void OnRecycle()
            {
                lineRender.positionCount = 0;
                vectors = new List<Vector3>();
            }

            public override void OnRelease()
            {
                Destroy(gameObject);
            }
        }
    }
}