using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    public void SetupHotKey(KeyCode _myHotKey)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            Debug.Log("«÷≈∞ : " + myHotKey);
        }
    }

}
