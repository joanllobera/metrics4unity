using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class MeasureHandsDistance : MonoBehaviour
{
    [SerializeField]
    protected Transform[] referenceHands;
    [SerializeField]
    protected Transform[] measuredHands;


    protected int frameCount;

    StringBuilder output;
    string separator;

    public
    string fileName = "./../metrics/handDistance.csv";


    [Tooltip("optional parameter, it will add the name of the trianed model in the .csv saved")]
    public
    Transform agentTransform;

    #region metrics

    public int endFrame = 120;
    public int startFrame = 20;



    metric hands_distance;



    void initMetrics()
    {
        hands_distance.initSampleList();


    }

    void updateMetrics()
    {



        hands_distance.addSample(getHandsDistance());


    }


    void showMetrics()
    {
        hands_distance.updateStats();

        Debug.Log("for metric: difference in hand distance the average is: " + hands_distance.mean + "and the std is: " + hands_distance.std);


    }


    #endregion



    float[] getHandsDistance()
    {

        //Example without fancy linq operations
        //   return new float[2] {
        //   Mathf.Abs( Vector3.Distance(leftHandRagdoll.position, rootRagdoll.position) -  Vector3.Distance(leftHandSource.position, rootSource.position)),            
        //   Mathf.Abs( Vector3.Distance(rightHandRagdoll.position, rootRagdoll.position)-  Vector3.Distance(rightHandSource.position, rootSource.position)) };




        float[] res = referenceHands.Zip(measuredHands, (m, r) => Mathf.Abs(Vector3.Distance(m.transform.position, r.transform.position))).ToArray();
        float[] reswithtime = new float[res.Length + 1];
        reswithtime[0] = frameCount;
        res.CopyTo(reswithtime, 1);
        return reswithtime;
    }





    void Start()
    {

        initMetrics();

        separator = ",";
        output = new StringBuilder();

        output.Append("Frame"+separator);
        
        output.AppendLine(string.Join(separator, referenceHands.Select(r => r.name).ToArray()));
    }


    protected virtual string GetPath4Data(Transform agentTransform = null)
    {

        string path = Application.dataPath + "/" + fileName  + ".csv";
        return path;
    }

    void FixedUpdate()
    {

        frameCount++;
        if (frameCount > startFrame)
        {
            updateMetrics();







        }
        if (frameCount == endFrame)
        {
            showMetrics();

            string samplesintext = hands_distance.getSamplesInStringFormat();
            output.Append(samplesintext);


            string path = GetPath4Data();
            Debug.Log(path);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, output.ToString());
            }
            else
            {
                File.AppendAllText(path, output.ToString());
            }
            // string[] newLine = { " ", pose_angle.mean.ToString(), pose_angle.std.ToString() };

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif

            Application.Quit();

        }
    }
}
