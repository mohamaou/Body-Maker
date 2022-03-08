using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Scrollbar scrollBar;
    private float scrollPos;
    private float[] pos;

    private Transform buttonHold;
    private Image activeButton;
    
    
    private void Update()
    { 
        ButtonMovement();
        ButtonScale();
        if(buttonHold != null) MoveButtonWithMouse();
    }
    private void MoveButtonWithMouse()
    {
        buttonHold.position = Vector3.Lerp(buttonHold.position,Input.mousePosition,6 * Time.deltaTime);
        buttonHold.localScale = Vector3.one * MouseHeight(0.5f);
        if (Input.GetMouseButtonUp(0)) RemoveButton();
    }
    public static float MouseHeight(float offset)
    {
        var height = (Input.mousePosition.y / Screen.height) * 5;
        height = ( height - offset) *2;
        height = Mathf.Clamp(height, 0, 1);
        return  1 - height;
    }
    private void ButtonMovement()
    {
        pos = new float[transform.childCount];
        var distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if(scrollPos <= pos[i] + (distance / 2) && scrollPos >= pos[i] - (distance / 2))
            {
                scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 6 * Time.deltaTime);
            }
        }
    }
    private void ButtonScale()
    {
        var distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            Vector3 targetScale;
            var activeButton = scrollPos <= pos[i] + (distance / 2) && scrollPos >= pos[i] - (distance / 2);
            targetScale =activeButton ? Vector3.one : Vector3.one * 0.7f;
           
            transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, targetScale, 6 * Time.deltaTime);
        }
        if(scrollBar.value <= 0) transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.one,6 * Time.deltaTime);
        if(scrollBar.value >= 1) transform.GetChild(transform.childCount -1).localScale = Vector3.Lerp(transform.GetChild(transform.childCount -1).localScale, Vector3.one,6 * Time.deltaTime);
    }
    public void Next()
    {
        if (scrollPos - 0.01f > 1 - (1f / (pos.Length - 1f))) return;
        scrollPos +=  1f / (pos.Length - 1f);
    }
    public void Previous()
    {
        if (scrollPos - (1f / (pos.Length - 1f)) < -0.01) return;
        scrollPos -=  1f / (pos.Length - 1f);
    }
    public void HoldButton(Transform button)
    {
        activeButton = button.GetComponent<Image>();
        buttonHold = Instantiate(button,canvas);
        buttonHold.position = button.position;
        activeButton.color = Color.gray;
        activeButton.raycastTarget = false;
    }
    private void RemoveButton()
    {
        Destroy(buttonHold.gameObject);
        buttonHold = null;
        if(BodyManager.IsInBody)return;
        activeButton.color = Color.white;
        activeButton.raycastTarget = true;
        activeButton = null;
    }
    
}
