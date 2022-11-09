using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Sets;
using TMPro;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    private int enemyCount;

    public GameObjectSet enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = Mathf.Max(enemyCount, enemies.Items.Count);
        GetComponent<TextMeshProUGUI>().text = $"{enemies.Items.Count} / {enemyCount} left";
    }
}
