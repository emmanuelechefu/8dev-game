using UnityEngine;

//I dont know if ts works lmaooo. When I try test I randomly get gold so idk what's up with that

public class EnemyDrop : MonoBehaviour
{
    public int goldDrop = 1;
    public int rubyDrop = 0;
    public int diamondDrop = 0;

    private void OnDestroy()
    {
        // if destroyed during quit / scene unload this might be noisy; optional guard
        if (!Application.isPlaying) return;

        GameManager.Instance.AddResource(ResourceType.Gold, goldDrop);
        GameManager.Instance.AddResource(ResourceType.Ruby, rubyDrop);
        GameManager.Instance.AddResource(ResourceType.Diamond, diamondDrop);
    }
}
