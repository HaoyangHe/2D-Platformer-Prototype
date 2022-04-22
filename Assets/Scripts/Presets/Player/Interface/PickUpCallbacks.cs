public interface PickUpCallbacks
{
    public void AddHealth(int value = 10);
    public void AddCoin(int value = 1);
    public void PickUpKey(InventoryItemData item);
    public void UseKey();
}
