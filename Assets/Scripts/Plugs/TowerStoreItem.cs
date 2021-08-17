using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerStoreItem : MonoBehaviour
{
    public GameObject towerPrefab;
    public float price;
    public UnityEvent<GameObject, float> OnClickEvent = new UnityEvent<GameObject, float>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnClickEvent?.Invoke(towerPrefab, price));
        transform.GetChild(1).GetComponent<Text>().text = "$" + price.ToString();
    }

}
