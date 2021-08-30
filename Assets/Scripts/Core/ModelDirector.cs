using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public interface IModel : IPlayable
{
    string modelName { get; }
    void Open(UnityAction done);
    void Close(UnityAction done);
}

public class ModelDirector : SceneDirector<IModel>
{
    List<IModel> models = new List<IModel>();

    public IModel GetModel(IModel model)
    {
        return models.Find((v) => v == model);
    }

    public X GetModel<X>() where X : MonoBehaviour, IModel
    {
        foreach (var v in models)
        {
            X x = v as X;
            if (x != null) { return x; }
        }

        return null;
    }

    public void SetModel(IModel model)
    {
        models.Add(model);
    }

    public void DefaultLoadModels()
    {
        Ensure<HomeModel>();
        Ensure<Terrain>();
    }

}
