using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour {

    private int energy = 0;

    // Use this for initialization
    void Start () {
        GameLoop.instance.RegisterActor (this);
    }

    abstract public Action GetAction ();

    public void Unregister () {
        GameLoop.instance.UnregisterActor (this);
    }

    public bool HasEnergyToActivate (int activationCost) {
        return energy >= activationCost;
    }

    public void SpendEnergyForActivation (int activationCost) {
        energy -= activationCost;
    }

    public void GainEnergy (int amount) {
        energy += amount;
    }

    public Action GetRestAction () {
        return GetComponent<RestAction> ();
    }
}