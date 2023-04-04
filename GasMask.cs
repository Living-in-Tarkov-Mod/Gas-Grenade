using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILogger

public class GasMask : MonoBehaviour
{
    private bool isEquipped;
    private bool isImmuneToGas;
    
    private void Start()
    {
        isEquipped = false;
        isImmuneToGas = false;
    }
    
    private void Update()
    {
        if (isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            isImmuneToGas = !isImmuneToGas;
            
            if (isImmuneToGas)
            {
                Debug.Log("You are now immune to gas damage.");
            }
            else
            {
                Debug.Log("You are no longer immune to gas damage.");
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GasGrenade"))
        {
            if (!isImmuneToGas)
            {
                // Apply gas damage to the player
                Debug.Log("You are taking gas damage!");
            }
            else
            {
                Debug.Log("You are immune to gas damage.");
            }
        }
    }
    
    public void Equip()
    {
        isEquipped = true;
    }
    
    public void Unequip()
    {
        isEquipped = false;
    }
}
