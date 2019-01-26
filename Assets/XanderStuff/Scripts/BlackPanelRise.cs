using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPanelRise : MonoBehaviour {

    [SerializeField]
    private float riseSpeed;

	public static BlackPanelRise Instance { get; private set; }

    private bool rising;



    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (rising)
            transform.position += new Vector3(0f, Time.deltaTime * riseSpeed);
    }

    public void Rise()
    {
        rising = true;
    }

}
