using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]

public class JumpPoint
{
    public GameObject pusher;//собсно пушер
    public float TimeCreated_s;//время его создания (0 - в порядке очереди) - пока не задействовано
}