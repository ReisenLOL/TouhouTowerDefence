using UnityEngine;

public class DollTowerSprite : RangedTowerSprite
{
    [System.Serializable]
    public class DollStringPairs
    {
        public LineRenderer stringToAttach;
        public Transform attachPoint;
    }

    public DollStringPairs[] listOfStrings;

    // Update is called once per frame
    void Update()
    {
        foreach (DollStringPairs dollStringPairs in listOfStrings)
        {
            dollStringPairs.stringToAttach.SetPosition(0, new Vector3(dollStringPairs.attachPoint.position.x, 100f, 0));
            dollStringPairs.stringToAttach.SetPosition(1, dollStringPairs.attachPoint.position);
        }
    }
}
