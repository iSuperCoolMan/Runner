using UnityEngine;

public class Coin : InteractionObject
{
    protected override void Update()
    {
        Move();
    }

    public override void OnInteraction(Player player)
    {
        player.AddCoins(1);
        Spawner.Instance.CoinsPoolHandler.Release(this);
    }
}
