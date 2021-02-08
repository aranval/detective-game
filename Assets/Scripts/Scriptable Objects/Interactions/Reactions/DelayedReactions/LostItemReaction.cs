public class LostItemReaction : DelayedReaction {
    public Item item;

    private Inventory inventory;

    protected override void Exec() => inventory = FindObjectOfType<Inventory>();
    protected override void InstantReaction() {
        if (inventory != null) inventory.RemoveItem(item);
        else UnityEngine.Debug.Log(item.name + " removed from inventory.");
    }
}