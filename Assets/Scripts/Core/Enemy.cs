using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private enum TypeEnemy
    {
        Rock,
        RockCrack,
        Ghost,
        Bees
    }

    [SerializeField]
    private TypeEnemy m_TypeEnemy;

    private float speed;

    void Start()
    {
        if (transform.childCount > 0)
            this.m_TypeEnemy = TypeEnemy.RockCrack;
    }
    
    void Update()
    {
        if (LevelGenerator.Instance.IsRunLevel)
        {
            move();
        }
    }

    private void move()
    {
        speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    public void DestroyEnemy()
    {
        if (m_TypeEnemy == TypeEnemy.RockCrack)
        {
            Destroy(this.gameObject);
        }
    }

}
