using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFunctionClass : MonoBehaviour
{

    public int NumOfTestScripts;
    public int min;
    public int max;
    // Start is called before the first frame update
    void Start()
    {
        Heap2<GenericTestScript> testScripts = new Heap2<GenericTestScript>(NumOfTestScripts);

        for (int i = 0; i < NumOfTestScripts; i++)
        {
            testScripts.Enqueue(new GenericTestScript(Random.Range(min, max)));
        }
        for (int i = 0; i < NumOfTestScripts; i++)
        {
            Debug.Log(testScripts.Dequeue().ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }


}
