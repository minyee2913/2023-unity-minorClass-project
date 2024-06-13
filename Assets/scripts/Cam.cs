using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float camSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance != null) {
            transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position + new Vector3(0, 7.85f, -3), camSpeed * Time.deltaTime);
        }
    }
}
