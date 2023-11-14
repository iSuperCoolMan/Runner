using UnityEngine;

public class HealingObject : InteractionObject
{
    protected override void Update()
    {
        Move();
    }

    public override void OnInteraction(Player player)
    {
        player.AddHealth(1);
        Spawner.Instance.HeartsPoolHandler.Release(this);
    }
}
