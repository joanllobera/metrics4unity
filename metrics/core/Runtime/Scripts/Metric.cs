using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct metric
{

    //a simple struct to generate metrics from arrays of samples.
    //each sample can also be an array of values.


    public float[] mean;
    public float[] std;

    public struct sample
    {
        public float[] values;

    }


    List<sample> samples;

    //sample currentSample;

    public static sample emptySample(int sizeSingleSample)
    {

        sample currentSample;

        currentSample.values = new float[sizeSingleSample];

        return currentSample;


    }

    public void initSampleList()
    {
        samples = new List<sample>();

    }
    public void addSample(float[] val) {

        sample a;
        a.values = val;
        samples.Add(a);
    
    }

    public void addSample(float val)
    {

        sample a;
        a.values = new float[1];
        
        a.values[0] = val;
        samples.Add(a);

    }

    public string getSamplesInStringFormat()
    {
        string result = "";
        
        foreach (var sample in samples)
        {
            string newline = "";
            for(int i= 0; i < sample.values.Length; i++)
            {
                float f= sample.values[i];
                newline += ( f.ToString());
                if (i < sample.values.Length - 1)
                    newline += ",";

            }
                
            result += newline + "\n";

        }
        return result;
    }


    public void updateStats()
    {
        mean = new float[samples[0].values.Length];
        for (int j = 0; j < samples[0].values.Length; j++)
        {
         
            mean[j] = 0;
           for (int i = 0; i < samples.Count; i++)
           { 
                    mean[j] += samples[i].values[j];
           }
           mean[j] = mean[j] / (samples.Count );
        }


        std = new float[samples[0].values.Length];
        for (int j = 0; j < samples[0].values.Length; j++)
        {
            std[j] = 0;
            for (int i = 0; i < samples.Count; i++)

            {
                float temp = (samples[i].values[j] - mean[j]);
                std[j] +=(temp*temp);
            }

            std[j] = Mathf.Sqrt(std[j] / (samples.Count ));
        }



    }


}


