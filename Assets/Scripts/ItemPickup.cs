using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private int itemScore = 1;
    [SerializeField] private bool shouldPlayerEvolve = false;
    [SerializeField] private bool isWinningItem = false;

    private bool addedToScore = false;

    public void ProcessItemPickup()
    {
        if (!addedToScore)
        {
            addedToScore = true;

            GameSession gameSession = FindObjectOfType<GameSession>();
            if (gameSession == null)
            {
                Debug.LogError("GameSession is null, add GameSession to the scene.");
                return;
            }
            gameSession.AddToScore(itemScore);

            if (isWinningItem)
            {
                gameSession.Win();
            }
        }

        Destroy(gameObject);
    }

    public bool ShouldPlayerEvolve()
    {
        return shouldPlayerEvolve;
    }
}
