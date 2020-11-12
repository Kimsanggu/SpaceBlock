using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour {
	void Start () {
        StartCoroutine("Flow");
	}
    IEnumerator Flow()
    {
        while (gameObject.activeInHierarchy)
        {
            if (transform.childCount > 0)
            {
                BlockTools.Destroy(transform.GetChild(0).gameObject);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return null;
            }
        }
    }
}
