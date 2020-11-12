using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour {
    public List<Sprite> list;
    int lastIndex = -1;
    
	void OnEnable () {
        if(lastIndex==-1){
            BlockTools.Shuffle(list);
        }
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (i != lastIndex)
            {
                gameObject.GetComponent<Image>().sprite = list[i];
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(list[i].rect.width, list[i].rect.height);
                lastIndex = i;
            }
        }
	}
}
