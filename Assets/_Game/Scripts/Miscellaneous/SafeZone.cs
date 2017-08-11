using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			Damageable playerDamageable = other.GetComponent<Damageable>();
			if (playerDamageable.TimeSinceLastRegenaration > playerDamageable.RegenarationTick && playerDamageable.Hp != playerDamageable.maxHP)
			{
				playerDamageable.Hp = playerDamageable.Hp + playerDamageable.RegenarationValue >= playerDamageable.maxHP ? playerDamageable.maxHP : playerDamageable.Hp + playerDamageable.RegenarationValue;
				playerDamageable.TimeSinceLastRegenaration = 0f;
			}
		}
	}
}
