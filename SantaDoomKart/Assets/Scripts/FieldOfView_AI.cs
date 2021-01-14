using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView_AI : MonoBehaviour
{

    enum bulletKind2
    {
    snowball,
    normal,
    stalker,
    }


    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask buffMask;
    public LayerMask weaponMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> visibleWeapons = new List<Transform>();
    public List<Transform> visibleBuffs = new List<Transform>();

    [SerializeField] private NavMeshAgent thisAgent;

    [SerializeField] private Vector3 randomPos; // the destination of this agent which changes over time

    [SerializeField] private int health; //temporarily SerializeField to test things out
    [SerializeField] private int score;
    [SerializeField] private int bulletAmount;
    [SerializeField] private GameObject bulletKind;

    [SerializeField] private Transform[] respawnPoints;


    private bool gotBuff;
    private bool gotWeapon;
    private bool inSightAction;

    private GameObject weaponCarried;

    private void Start()
    {
        StartCoroutine(ChangeDestination());
        StartCoroutine("findTargetsWithDelay", 0.2f);
    }

    IEnumerator findTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
            FindVisibleBuffs();
            FindVisibleWeapons();
        }
    }

    void FindVisibleTarget()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToYarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToYarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(dstToTarget > 1)
                { 
                    if(!Physics.Raycast(transform.position, dirToYarget,dstToTarget,obstacleMask))
                    {
                        visibleTargets.Add(target);
                        if(visibleTargets.Count > 0)
                        {
                            if(gotWeapon == true)
                            {
                                inSightAction = true;
                                transform.LookAt(visibleTargets[0]);

                                if(bulletAmount > 0)
                                {
                                    shoot();
                                }
                            }
                            if (gotWeapon == false)
                            {
                                flee();
                            }
                        }
                        if(visibleBuffs.Count < 1 && visibleTargets.Count < 1 && visibleWeapons.Count < 1 && inSightAction == true)
                        {
                            inSightAction = false;
                            StartCoroutine(ChangeDestination());
                        }
                    }
                }
            }
        }
    }

    void FindVisibleBuffs()
    {
        visibleBuffs.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, buffMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform buff = targetsInViewRadius[i].transform;
            Vector3 dirToYarget = (buff.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToYarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, buff.position);
                if (!Physics.Raycast(transform.position, dirToYarget, dstToTarget, obstacleMask))
                {
                    visibleBuffs.Add(buff);
                    if(gotBuff == true)
                    {
                        flee();
                    }
                    if(gotBuff == false)
                    {
                        takeItem();
                    }

                }
            }
        }
    }

    void FindVisibleWeapons()
    {
        visibleWeapons.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, weaponMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform weapon = targetsInViewRadius[i].transform;
            Vector3 dirToYarget = (weapon.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToYarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, weapon.position);
                if (!Physics.Raycast(transform.position, dirToYarget, dstToTarget, obstacleMask))
                {
                    visibleWeapons.Add(weapon);

                    if(gotWeapon == false)
                    {
                        inSightAction = true;
                        thisAgent.SetDestination(visibleWeapons[0].transform.position);
                    }

                    if(gotWeapon == true)
                    {
                        takeItem();
                    }

                }
            }
        }
    }

    public Vector3 DirfromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    void takeItem()
    {
        
    }

    void flee()
    {

    }

    void shoot()
    {
        bulletAmount--;
        if(bulletAmount < 1)
        {
            bulletKind = null;
        }
    }

    IEnumerator ChangeDestination()
    {
        yield return new WaitForSeconds(4);
        
        if(inSightAction == false)
        {
            randomPos = Random.insideUnitSphere * 2 + new Vector3(0, 0, 0);
            thisAgent.SetDestination(randomPos);
            StartCoroutine(ChangeDestination());
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            ReceiveDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "explosion")
        {
            ReceiveDamage();
        }
    }

    void ReceiveDamage()
    {
        health--;

        if (health < 1)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = respawnPoints[Random.Range(0, 6)].position;
        this.score -= 50;
        health = 3;
    }



}
