using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneFPSChanger : MonoBehaviour
{
	private void Awake()
	{
        Application.targetFrameRate = 165;
        QualitySettings.vSyncCount = 0;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("beans");
    }
}
