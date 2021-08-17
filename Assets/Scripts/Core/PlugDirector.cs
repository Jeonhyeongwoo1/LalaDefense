using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlug : IPlayable
{

}

public class PlugDirector : SceneDirector<IPlug>
{

}
