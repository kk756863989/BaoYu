using System;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public enum SceneObjType
    {
        BALL,
        CHOCOLATE,
        BISCUITS,
        FRUIT,
        TASKBALL3,
        TASKBALL4,
        BLOCK,
    }

    public enum LayerName
    {
        BACK,
        MIDDLE,
        BFORE,
        BALL,
        FORE,
        EFFECT,
        MASK
    }

    public class CCScene : CCDynamicObject
    {
        private int counter = 0;

        private Dictionary<string, Dictionary<string, ISceneObject>> objMap =
            new Dictionary<string, Dictionary<string, ISceneObject>>();

        private Dictionary<string, ISceneObject> playerMap, monsterMap, roadMap, blockMap;
        private Dictionary<string, Transform> rootMap = new Dictionary<string, Transform>();
        public readonly GameObject node;
        public CCPool ballPool, chocolatePool, fruitPool;
        public Vector3 ballOriginPoint = new Vector3(388, 200, 0);

        public CCScene(CCMap data, GameObject levelManagerObj) : base(data)
        {
            node = levelManagerObj;
        }

        void InitRoot()
        {
            foreach (int p in Enum.GetValues(typeof(SceneObjType)))
            {
                objMap.Add(p.IntToEnum<SceneObjType>().EnumToString(), new Dictionary<string, ISceneObject>());
            }

            RectTransform layerRoot;
            string layerName;
            Canvas canvas;
            Transform container = node.transform.Find("Container");

            foreach (int valueOfLayer in Enum.GetValues(typeof(LayerName)))
            {
                layerName = valueOfLayer.IntToEnum<LayerName>().EnumToString();

                if (container.Find(layerName) == null)
                {
                    layerRoot = new GameObject(layerName).GetOrAddComponent<RectTransform>();
                    layerRoot.SetParent(container);

                    layerRoot.localScale = new Vector3(1, 1, 1);
                    layerRoot.anchorMin = new Vector2(0, 0);
                    layerRoot.anchorMax = new Vector2(1, 1);
                    layerRoot.offsetMin = new Vector2(0, 0);
                    layerRoot.offsetMax = new Vector2(0, 0);
                }
                else
                {
                    layerRoot = container.Find(layerName).GetOrAddComponent<RectTransform>();
                }

                canvas = layerRoot.GetOrAddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = valueOfLayer;

                rootMap.Add(layerName, layerRoot);
            }
        }

        public override void OnFirstUse()
        {
            InitRoot();
            InitPool();
        }

        private void InitPool()
        {
            //var model = new Ball(2);

            //ballPool = new CCPool(model, 30);

            //var chocolate = new Chocolate();

            //chocolatePool = new CCPool(chocolate);

            //var fruit = new Fruit();

            //fruitPool = new CCPool(fruit, 10);
        }

        public Transform GetRootByLayerName(LayerName layerName)
        {
            var key = layerName.EnumToString();

            return !rootMap.ContainsKey(key) ? null : rootMap[key];
        }

        public Transform GetLayerRoot(SceneObjType sceneObjType)
        {
            var result = node.transform;

            switch (sceneObjType)
            {
                case SceneObjType.BALL:
                    result = rootMap[LayerName.BALL.EnumToString()];
                    break;
            }

            return result;
        }

        /// <summary>
        /// 添加物体
        /// </summary>
        public ISceneObject AddObj(SceneObjType sceneObjType, Vector3 pos, /*Ball.BallType ballType,*/ int ballNumber)
        {
            var entityId = GenerateUid(sceneObjType);
            ISceneObject obj;

            switch (sceneObjType)
            {
                default:
                    obj = ballPool.Add(PoolScaleType.RECYCLE, pos) as ISceneObject;

                    //if (obj is Ball addBall)
                    //{
                    //    addBall.number = ballNumber;
                    //    addBall.ballType = ballType;
                    //    addBall.Node.transform.localPosition = pos;
                    //}

                    break;
                case SceneObjType.CHOCOLATE:
                    obj = chocolatePool.Add(PoolScaleType.SCALEUP, pos) as ISceneObject;

                    //if (obj is Chocolate addObj)
                    //{
                    //    addObj.number = ballNumber;
                    //    addObj.Node.transform.localPosition = pos;
                    //}

                    break;
                case SceneObjType.BISCUITS:
                    obj = ballPool.Add(PoolScaleType.RECYCLE, pos) as ISceneObject;

                    //if (obj is Biscuits addBiscuits)
                    //{
                    //    addBiscuits.number = ballNumber;
                    //    addBiscuits.Node.transform.localPosition = pos;
                    //}

                    break;
                case SceneObjType.FRUIT:
                    obj = fruitPool.Add(PoolScaleType.SCALEUP, pos) as ISceneObject;

                    //if (obj is Fruit addFruit)
                    //{
                    //    addFruit.number = ballNumber;
                    //    addFruit.Node.transform.localPosition = pos;
                    //}

                    break;
            }

            if (obj == null) return null;

            obj.EntityId = entityId;
            obj.SceneObjType = sceneObjType;
            var objs = GetObjsOfType(sceneObjType);

            objs.Add(entityId, obj);
            obj.Init();

            return obj;
        }

        /// <summary>
        /// 移除物体
        /// </summary>
        public void RemoveObj(ISceneObject obj)
        {
            Dictionary<string, ISceneObject> objs = GetObjsOfType(obj.SceneObjType);

            if (objs == null || !objs.ContainsKey(obj.EntityId)) return;

            switch (obj.SceneObjType)
            {
                case SceneObjType.BALL:
                    ballPool.Put(obj);
                    break;
                case SceneObjType.CHOCOLATE:
                    chocolatePool.Put(obj);
                    break;
                case SceneObjType.FRUIT:
                    fruitPool.Put(obj);
                    break;
            }

            objs.Remove(obj.EntityId);
        }

        /// <summary>
        /// 获取物体
        /// </summary>
        /// <param name="sceneObjType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ISceneObject GetObj(SceneObjType sceneObjType, string entityId)
        {
            var objs = GetObjsOfType(sceneObjType);

            if (objs == null || !objs.ContainsKey(entityId)) return null;

            return objs[entityId];
        }

        /// <summary>
        /// 获取某类型的所有物体
        /// </summary>
        public Dictionary<string, ISceneObject> GetObjsOfType(SceneObjType sceneObjType)
        {
            var key = sceneObjType.EnumToString();

            return !objMap.ContainsKey(key) ? null : objMap[key];
        }

        /// <summary>
        /// 清空某类型的所有物体
        /// </summary>
        private void ClearObjsOfType(SceneObjType sceneObjType)
        {
            var objs = GetObjsOfType(sceneObjType);

            foreach (var pair in objs.Keys)
            {
                var obj = objs[pair];

                obj.OnRecycle();
                objs.Remove(pair);
            }
        }

        /// <summary>
        /// 清空所有物体
        /// </summary>
        void ClearAllObjs()
        {
            foreach (int p in Enum.GetValues(typeof(SceneObjType)))
            {
                var type = p.IntToEnum<SceneObjType>();
                ClearObjsOfType(type);
            }
        }

        /// <summary>
        /// 生成场景物体唯一ID
        /// </summary>
        string GenerateUid(SceneObjType sceneObjType)
        {
            counter++;
            return sceneObjType.EnumToString() + counter;
        }
    }
}