using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    Transform Cam;
    public RectTransform HPImg;
    public Monster Owner;
    void Start()
    {
        Cam = Camera.main.transform;
        Owner = Owner.GetComponentInParent<Monster>();
    }

    void Update()
    {
        transform.LookAt(transform.position + Cam.rotation * Vector3.forward
            , Cam.rotation * Vector3.up);
        HPImg.localScale = new Vector3(Owner.CurHealth/ Owner.MaxHealth, 1, 1);
    }
}
