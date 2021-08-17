using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public interface IModel : IPlayable
{

}

public class ModelDirector : SceneDirector<IModel>, IModel
{

    public void DefaultModel()
    {
        //SceneDirector<IModel>.Instance.Ensure(nameof(Terrain));
    }

}
