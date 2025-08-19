using Core.Extensions;
using UnityEngine;

public class MasterSpark : Spellcard
{
    //add cool funny effect.
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
        rangeToCheck = thisTower.GetComponentInChildren<TowerRangeCollider>().transform;
        Transform newBeam = Instantiate(masterSparkBeam, rangeToCheck.transform);
        newBeam.position = thisTower.transform.position;
        newBeam.localScale = new Vector3(100f, sparkSize.y, 1);
        newBeam.gameObject.SetActive(false);
        newBeam.Translate(Vector2.right * (50f + 0.5f));
        createdBeam = newBeam.gameObject;
    }
    protected override void SpellCardEffects()
    {
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
                currentSparkTime = 0;
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