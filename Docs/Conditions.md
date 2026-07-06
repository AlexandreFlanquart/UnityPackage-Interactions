# Creating Conditions

Conditions are checks that must be satisfied before an interaction occurs.


## Properties

- **`requiredForEffects`** : 
  - `true` = Condition required for **EnterEffect** (block EnterEffect if false)
  - `false` = Required for **InteractEffect** only (EnterEffect triggers even if false)

- **`shouldBeTrue`** : Inverts the logic (useful for "NOT" conditions)

- **`isReady`** : Current state of the condition

## Notify Changes

When the state changes, call `OnConditionMet(bool)` :



## Example: Inventory Condition

```csharp
public class InventoryCondition : ACondition
{
    [SerializeField] private string objectName;
    [SerializeField] private int requiredQuantity = 1;
    
    private Inventory inventory;
    
    void OnEnable()
    {
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
            inventory.onInventoryChanged += CheckInventory;
    }
    
    void OnDisable()
    {
        if (inventory != null)
            inventory.onInventoryChanged -= CheckInventory;

        // Reset state and notify listeners so the interactable doesn't keep
        // a stale "condition met" state while this component can't update it
        ResetReadyState();
    }
    
    protected override bool EvaluateCondition()
    {
        if (inventory == null) return false;
        return inventory.HasObject(objectName, requiredQuantity);
    }
    
    private void CheckInventory()
    {
        bool satisfied = CheckCondition();
        if (isReady != satisfied)
        {
            isReady = satisfied;
            OnConditionMet(isReady);
        }
    }
}
```

## Best Practices

✅ Notify only when the state **actually changes**  
✅ Check for null references  
✅ Unsubscribe from events in `OnDisable()`  
✅ Call `ResetReadyState()` in `OnDisable()` — it resets `isReady` and notifies listeners only if the condition was met (no spurious notify)  
✅ Avoid expensive calculations in `EvaluateCondition()`
