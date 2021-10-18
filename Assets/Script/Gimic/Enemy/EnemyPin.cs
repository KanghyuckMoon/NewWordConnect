using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPin : EnemyBased
{
	[SerializeField]
	private bool updown;
	[SerializeField]
	private bool upon;
	private bool attack;

	[SerializeField]
	private bool right;


    protected override void Start()
    {
        base.Start();
		player = FindObjectOfType<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		MoveF();

	}

    public override void Settingvalue()
    {
        base.Settingvalue();
		rigid.gravityScale = 0;
    }

    private void MoveF()
	{
		if (!attack)
		{
			if (updown)
			{
				if (upon)
				{
					if (player.transform.position.x < transform.position.x + 1f && player.transform.position.x > transform.position.x - 1f && player.transform.position.y > transform.position.y)
					{
						attack = true;
						return;
					}
				}
				else if (player.transform.position.x < transform.position.x + 1f && player.transform.position.x > transform.position.x - 1f && player.transform.position.y < transform.position.y)
				{
					attack = true;
					return;
				}
			}
			else if (right)
			{
				if (player.transform.position.y < transform.position.y + 1f && player.transform.position.y > transform.position.y - 1f && player.transform.position.x > transform.position.y)
				{
					attack = true;
					return;
				}
			}
			else if (player.transform.position.y < transform.position.y + 1f && player.transform.position.y > transform.position.y - 1f && player.transform.position.x < transform.position.y)
			{
				attack = true;
			}
			return;
		}
		if (updown)
		{
			if (upon)
			{
				rigid.AddForce(Vector2.up * speedset * enemymoveSpeed, ForceMode2D.Impulse);
				return;
			}
			rigid.AddForce(Vector2.down * speedset * enemymoveSpeed, ForceMode2D.Impulse);
			return;
		}
		else
		{
			if (right)
			{
				rigid.AddForce(Vector2.right * speedset * enemymoveSpeed, ForceMode2D.Impulse);
				return;
			}
			rigid.AddForce(Vector2.left * speedset * enemymoveSpeed, ForceMode2D.Impulse);
			return;
		}
	}

	protected override void OnCollisionEnter2D(Collision2D collision)
	{ 
		if (attack)
		{
			Died();
		}
	}
}
