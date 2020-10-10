using UnityEngine;

namespace CC
{
    public interface ISceneObject : IReuseObject
    {
        SceneObjType SceneObjType { get; set; }
        GameObject Node { get; set; }
    }
}