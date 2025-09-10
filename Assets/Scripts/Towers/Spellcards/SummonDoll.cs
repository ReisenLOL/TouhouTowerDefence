using UnityEngine;

public class SummonDoll : Spellcard
{
    private TowerPlacement towerPlacement;
    public string DollTowerID;
    protected override void Start()
    {
        base.Start();
        towerPlacement = FindFirstObjectByType<TowerPlacement>();
    }
    protected override void SpellCardEffects()
    {
        towerPlacement.AddTower(DollTowerID);
        towerPlacement.RebuildTowerSelection();
    }
}
