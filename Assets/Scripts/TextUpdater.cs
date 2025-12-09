using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    public TMP_Text textbox1;
    public TMP_Text textbox2;

    // Update is called once per frame
    void Update()
    {
        textbox1.text = GameManager.Instance.gold.ToString(); 
        textbox2.text = GameManager.Instance.keysCollected.ToString();
    }
}
