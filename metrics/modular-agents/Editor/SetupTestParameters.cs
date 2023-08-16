using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorUpdate;
public class SetupTestParameters : MonoBehaviour
{

    public  float PhysicsFPS = 150;
  
    public  float KP = 10000;


    public bool uniqueKP = true;

    private void OnEnable()
    {

        Time.fixedDeltaTime = 1 /PhysicsFPS;
        Debug.Log("fixed delta time: " + Time.fixedDeltaTime);
        if (uniqueKP)
        {

            Muscles[] m4t = FindObjectsOfType<Muscles>();
         
        }


    }

   
}
