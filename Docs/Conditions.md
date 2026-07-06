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
✅ Avoid expensive calculations in `CheckCondition()`
