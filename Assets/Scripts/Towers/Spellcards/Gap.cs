using Unity.VisualScripting;
using UnityEngine;

public class Gap : Spellcard
{
    //supposed to add a new tower that blocks enemies from passing through.
    public Tower gapTower;
    private TowerPlacement placementHandler;

    private void Start()
    {
        placementHandler = FindFirstObjectByType<TowerPlacement>();
    }
    protected override void SpellCardEffects()
    {
        
    }
}
