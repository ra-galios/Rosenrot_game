using UnityEngine;
using System.Collections;

public class MenuSwipe : MonoBehaviour
{

    private float prevFingerPosY;
    private float deltaPosY;
    private float posY;
    private bool movePanel = false;
    enum PanelState { Up, Down }
    private PanelState currentState = PanelState.Up;

    public float defaultPositionY;
    public float bottomPositionY;

    public bool darkLevelIsOpen;

    void Start()
    {
        defaultPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (darkLevelIsOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                movePanel = false;
                prevFingerPosY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            }
            if (Input.GetMouseButton(0))
            {
                deltaPosY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - prevFingerPosY;

                posY = transform.position.y + deltaPosY;
                posY = Mathf.Clamp(posY, defaultPositionY, bottomPositionY);

                transform.position = new Vector3(transform.position.x, posY, transform.position.z);

                prevFingerPosY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            }
            if (Input.GetMouseButtonUp(0))
            {
                movePanel = true;
            }

            if (movePanel)
            {
                if (currentState == PanelState.Up)
                {
                    if (transform.position.y >= defaultPositionY + (bottomPositionY - defaultPositionY) * 0.2f || deltaPosY > 0.1f)//если панель сдвинута на 20% вниз
                    {                                                                                                              // или ускорение пальца больше 0.1
                        MoveDown();
                    }
                    else
                    {
                        MoveUp();
                    }
                }

                else if (currentState == PanelState.Down)
                {
                    if (transform.position.y <= bottomPositionY - (bottomPositionY - defaultPositionY) * 0.2f || deltaPosY < -0.1f)//если панель сдвинута на 20% вверх
                    {                                                                                                              //или ускорение пальца меньше -0.1
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }
                }
            }
        }
    }

    private void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, defaultPositionY, .3f), transform.position.z);

        if (transform.position.y <= defaultPositionY + 0.01f)
        {
            transform.position = new Vector3(transform.position.x, defaultPositionY, transform.position.z);
            currentState = PanelState.Up;
            movePanel = false;
        }
    }

    private void MoveDown()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, bottomPositionY, .3f), transform.position.z);

        if (transform.position.y >= bottomPositionY - 0.01f)
        {
            transform.position = new Vector3(transform.position.x, bottomPositionY, transform.position.z);
            currentState = PanelState.Down;
            movePanel = false;
        }
    }
}
