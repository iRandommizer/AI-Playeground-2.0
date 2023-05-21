using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFunctionClass : MonoBehaviour
{

    public int NumOfTestScripts;
    public int min;
    public int max;
    public List<int> listOfNumbers;
    // Start is called before the first frame update
    void Start()
    {
        listOfNumbers = new List<int>();
        Heap2<GenericTestScript> testScripts = new Heap2<GenericTestScript>(NumOfTestScripts);

        for (int i = 0; i < NumOfTestScripts; i++)
        {
            int RandomNum = Random.Range(min, max);
            Debug.Log("Produced Num: " + i + " " + RandomNum);
            testScripts.Enqueue(new GenericTestScript(RandomNum));
        }
        for (int i = 0; i < NumOfTestScripts; i++)
        {
            Debug.Log(testScripts.Dequeue().lifeTotal);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }


}
