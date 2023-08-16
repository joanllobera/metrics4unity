using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;


public class MeasurePoseDifference : MonoBehaviour
{

    [SerializeField]
    Transform agentTransform;


    [SerializeField]
    protected Transform referenceRoot;

    [SerializeField]
    protected Transform measuredRoot;


    Transform[] _referenceTransforms;
    Transform[] _measuredTransforms;



    protected int frameCount;

    StringBuilder output;
    string separator;

    #region metrics

    public  int endFrame = 120;
    public  int startFrame = 20;
    public string fileName = "./../metrics/poseDifference.csv";


    metric pose_angle;



    void initMetrics()
    {
        pose_angle.initSampleList();

        Transform[] referenceCandidates = referenceRoot.GetComponentsInChildren<Transform>();
        Transform[] trackedCandidates = measuredRoot.GetComponentsInChildren<Transform>();

        List<Transform> trackedTransforms = new List<Transform>();
        List<Transform> referencesFound = new List<Transform>();
        foreach (Transform transform in referenceCandidates)
        {

            string name2find = transform.name.Replace("mixamorig:", "");

            Transform outcome = trackedCandidates.FirstOrDefault<Transform>(p => p.name.Contains(name2find));
            if (outcome == null)
                Debug.Log("nothing to track that matches " + name2find);
            else
            {
                trackedTransforms.Add(outcome);
                referencesFound.Add(transform);
            }
        }

        _referenceTransforms = referencesFound.ToArray();
        _measuredTransforms = trackedTransforms.ToArray();
    }

    void updateMetrics()
    {



        pose_angle.addSample(getPoseDifference());


    }


    void showMetrics()
    {
        pose_angle.updateStats();

        Debug.Log("for metric: difference in pose the average is: " + pose_angle.mean[1] + "and the std is: " + pose_angle.std[1]);

       // string[] newLine = { " ", pose_angle.mean.ToString(), pose_angle.std.ToString() };

    
        //output.AppendLine(string.Join(separator, newLine));

    }


    #endregion



    float[] getPoseDifference()
    {

        //Example without fancy linq operations
        //   return new float[2] {
        //   Mathf.Abs( Vector3.Distance(leftHandRagdoll.position, rootRagdoll.position) -  Vector3.Distance(leftHandSource.position, rootSource.position)),            
        //   Mathf.Abs( Vector3.Distance(rightHandRagdoll.position, rootRagdoll.position)-  Vector3.Distance(rightHandSource.position, rootSource.position)) };


        float res = _referenceTransforms.Zip(_measuredTransforms, (m, r) => Mathf.Abs(Quaternion.Angle(m.transform.rotation, r.transform.rotation))).Sum() / _referenceTransforms.Length;



        return new float[2] { frameCount, res };


    }


    protected string GetPath4Data()
    {
        //string modelName = GetNNModelName.GetModelName(agentTransform);

        string path = Application.dataPath + "/" + fileName + ".csv";
        return path;
    }



    void Start()
    {

        initMetrics();

        separator = ",";
        output = new StringBuilder();
        string[] headings = { "Time", "Sample"};
        output.AppendLine(string.Join(separator, headings));
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

            string samplesintext = pose_angle.getSamplesInStringFormat();
            output.Append(samplesintext);

            
            string path =GetPath4Data();
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
