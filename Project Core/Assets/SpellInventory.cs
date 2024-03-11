using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    public List<Spell> SpellList = new List<Spell>();
    public List<Spell> SpellListAlternate = new List<Spell>();
    public bool HasSpellInleftHand;
    public bool HasSpellInRightHand;
    public Spell currentSpell;
    public Spell currentSpell2;
    public int spellNumber;
    public int spellNumberAlternate;
    public Transform LeftHand;
    public Transform RightHand;
    public Projectile spellProjectile;
    

    public void AddSpellToSpellLists(Spell spell,Spell spell2)
    {
        SpellList.Add(spell);
        SpellListAlternate.Add(spell2);
    }

    private void Update()
    {
        if (SpellList.Count > 0)
        {
            if (!HasSpellInleftHand || !HasSpellInRightHand)
            {
                MakeSpellAppearInPlayerHand();
                HasSpellInleftHand = true;
                HasSpellInRightHand = true; // Added to ensure both hands are set to true
            }
        }
    }

    public void SpellSwitch(int number)
    {
        if (number == 0)
        {
            spellNumber++;
            HasSpellInleftHand = false;
            HasSpellInRightHand = false;
            if (spellNumber >= SpellList.Count)
            {
                spellNumber = 0;
            }
            else if (spellNumber < 0)
            {
                spellNumber = SpellList.Count - 1;
            }
        }
        else if (number == 1)
        {
            spellNumberAlternate++;
            HasSpellInleftHand = false;
            HasSpellInRightHand = false;
            if (spellNumberAlternate >= SpellListAlternate.Count)
            {
                spellNumberAlternate = 0;
            }
            else if (spellNumberAlternate < 0)
            {
                spellNumberAlternate = SpellListAlternate.Count - 1;
            }
        }
    }

    private void MakeSpellAppearInPlayerHand()
    {
        if (SpellList != null && SpellList.Count > 0)
        {
            if (currentSpell != null)
            {
                currentSpell.gameObject.SetActive(false);
                currentSpell = null;
            }

            currentSpell = SpellList[spellNumber];
            currentSpell.gameObject.SetActive(true);
            if (currentSpell != null)
            {
                
                currentSpell.transform.SetParent(LeftHand);
                currentSpell.transform.localPosition = Vector3.zero;
                currentSpell.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellList is null or empty.");
        }

        if (SpellListAlternate != null && SpellListAlternate.Count > 0)
        {
            if (currentSpell2 != null)
            {
                currentSpell2.gameObject.SetActive(false);
                
            }

            currentSpell2 = SpellListAlternate[spellNumberAlternate];
            currentSpell2.gameObject.SetActive(true);
            if (currentSpell2 != null)
            {
                // Set the spell for the right hand
                currentSpell2.transform.SetParent(RightHand);
                currentSpell2.transform.localPosition = Vector3.zero;
                currentSpell2.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellListAlternate is null or empty.");
        }
    }
    private void Shootspell(Transform firepoint)
    {
        if(currentSpell.element == Spell.SpellElement.Fire && currentSpell2.element == Spell.SpellElement.Fire)
        {
            Fireball(firepoint);
        }
    }
    private void Fireball(Transform firepoint)
    {
        Vector3 destination;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hits;

        if (Physics.Raycast(ray, out hits))
        {
            destination = hits.point;
        }
        else
        {
            destination = ray.GetPoint(10000);
        }

        GameObject projectile = Instantiate(spellProjectile.gameObject, firepoint.position, firepoint.rotation);
        spellProjectile.GetComponent<Projectile>().currentVelocity = (spellProjectile.GetComponent<Projectile>().ProjectileSpeed) * Player.Instance.SprintSpeed;
        projectile.GetComponent<Rigidbody>().velocity = (destination - firepoint.position).normalized * spellProjectile.GetComponent<Projectile>().currentVelocity;
    }
    private Spell InstantiateSpell(Spell spellPrefab)
    {
        return Instantiate(spellPrefab);
    }
}
