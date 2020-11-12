using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    void Start()
    {
        if (MapManager.Instance != null)
        {
            Text txt = GetComponent<Text>();
            if (txt != null)
            {
                txt.text = MapManager.Instance.Level.ToString();
            }
            TextUsedImage txtImage = GetComponent<TextUsedImage>();
            if (txtImage != null)
            {
                txtImage.text = MapManager.Instance.Level.ToString();
            }
        }
    }
}
