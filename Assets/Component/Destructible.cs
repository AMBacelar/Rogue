using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{

	public int startingHP;
	int currentHP;

	void Start()
	{
		currentHP = startingHP;
		Debug.Log(name + " has woken up, and has " + currentHP + " health");
	}

	public void ApplyDamage(int damage)
	{
		currentHP -= damage;
		Debug.Log(name + " took " + damage.ToString() + " damage. Current Health is now: " + currentHP.ToString());
		if (currentHP <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		gameObject.SendMessage("Unregister");
		Destroy(gameObject);
	}
}