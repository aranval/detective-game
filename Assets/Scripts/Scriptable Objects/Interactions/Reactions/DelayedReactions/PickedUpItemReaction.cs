public class PickedUpItemReaction : DelayedReaction {
    public Item item;

    private Inventory inventory;

    protected override void Exec() => inventory = FindObjectOfType<Inventory>();
    protected override void InstantReaction() {
        if (inventory != null) inventory.AddItem(item);
        else UnityEngine.Debug.Log(item.name + " added to inventory.");
    }
}