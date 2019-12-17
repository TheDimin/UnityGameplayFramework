using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class AchievementManagerBase : ISerializable
{
    protected List<AchievementBase> achievements = new List<AchievementBase>();

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("achievements:", achievements, typeof(List<AchievementBase>));
    }

    public AchievementManagerBase() { }

    protected AchievementManagerBase(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        new BinaryFormatter();
        throw new System.NotImplementedException();
    }
}
