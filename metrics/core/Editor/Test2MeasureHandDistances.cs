using System.Collections;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using System.Linq;

public class Test2MeasureHandDistances

{
  

    public static float tolerance4comparisons = 0.05f;


 

    public class MyMonoBehaviourTest : MeasureHandsDistance, IMonoBehaviourTest
    {

        public bool IsTestFinished
        {
            get { return frameCount > endFrame; }
        }





    }



  
  [UnityTest]

    public IEnumerator TestHandDistances()
    {

        Debug.Log("the initialization of the scene needs to be done from a prefab");


        yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
    }

  



}
