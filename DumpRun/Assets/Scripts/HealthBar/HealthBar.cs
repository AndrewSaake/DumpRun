using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    [SerializeField ] string healthBarName;
    // Start is called before the first frame update
    void Start()
    {
       bar = transform.Find("Bar");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSize(float sizeNorm)
    {
        bar.localScale = new Vector3(sizeNorm, 1f);
    }

}