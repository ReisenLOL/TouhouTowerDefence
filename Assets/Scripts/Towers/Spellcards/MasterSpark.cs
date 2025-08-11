using UnityEngine;

public class MasterSpark : Spellcard
{
    //add cool funny effect.
    private Transform towerSprite;
    public float sparkDuration;
    private float currentSparkTime;
    private bool sparkActive;
    public Vector2 sparkSize;
    public LayerMask enemyLayer;
    private Transform rangeToCheck;
    public float damageDebounce;
    private float damageDebounceTime;
    public float damageDealtPerTick;
    public Transform masterSparkBeam;
    private GameObject createdBeam;
    protected override void Start()
    {
        base.Start();
        towerSprite = thisTower.transform.Find("Sprite").transform;
        rangeToCheck = thisTower.GetComponentInChildren<TowerRangeCollider>().transform;
        Transform newBeam = Instantiate(masterSparkBeam, thisTower.transform);
        newBeam.position = thisTower.transform.position;
        newBeam.rotation = rangeToCheck.rotation;
        newBeam.localScale = new Vector3(100f, sparkSize.y, 1);
        newBeam.gameObject.SetActive(false);
        newBeam.Translate(rangeToCheck.transform.right * 50f);
        createdBeam = newBeam.gameObject;
    }
    protected override void SpellCardEffects()
    {
        //do an overlapbox where the tower faces, foreach enemy inside during this, damage them.
        //or ray cast?
        sparkActive = true;
        createdBeam.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        if (sparkActive)
        {
            
            currentSparkTime += Time.deltaTime;
            if (currentSparkTime >= sparkDuration)
            {
                sparkActive = false;
                createdBeam.gameObject.SetActive(false);
            }
            damageDebounceTime += Time.deltaTime;
            if (damageDebounceTime > damageDebounce)
            {
                damageDebounceTime = 0;
                //i do not like that i am constantly running this raycast and getcomponent
                RaycastHit2D[] hit = Physics2D.BoxCastAll(thisTower.transform.position, sparkSize,
                    rangeToCheck.eulerAngles.z, rangeToCheck.transform.right, 100f, enemyLayer);
                foreach (RaycastHit2D foundEnemy in hit)
                {
                    foundEnemy.transform.gameObject.GetComponent<Enemy>().TakeDamage(damageDealtPerTick);
                }
            }

        }
    }
}