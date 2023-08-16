
using UnityEngine;
using Unity.MLAgents.Policies;


public class MeasureHandsDistance4TrainedAgent: MeasureHandsDistance

{

    protected override string GetPath4Data(Transform agentTransform = null)
    {
        string modelName = GetModelName(agentTransform);

        string path = Application.dataPath + "/" + fileName +  "/" + modelName +".csv";
        return path;
    }


    public static string GetModelName(Transform agentTransform)
    {

        BehaviorParameters bp = agentTransform.GetComponent<BehaviorParameters>();
        if (bp != null)
            return bp.Model.name;
        else
            return "";



    }

}
