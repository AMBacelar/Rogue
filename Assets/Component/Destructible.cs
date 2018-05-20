using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{

	public int StartingHP;
	int currentHP;
	public int CurrentHP { get { return currentHP; } }

	void Start()
	{
		currentHP = StartingHP;
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
		GetComponent<BoardPosition>().Unregister();
		GetComponent<Actor>().Unregister();
		if (GetComponent<PlayerActor>())
		{
			GameObject.Find("Game").GetComponent<LevelManager>().LoadScene("03_Lose");
		}
		Destroy(gameObject);
	}
}