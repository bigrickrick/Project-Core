using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    public List<Spell> SpellList = new List<Spell>();
    private List<Spell> SpellListAlternate = new List<Spell>();
    public bool HasSpellInleftHand;
    public bool HasSpellInRightHand;
    public Spell currentSpell;
    public Spell currentSpell2;
    public int spellNumber;
    public int spellNumberAlternate;
    public Transform LeftHand;
    public Transform RightHand;

    private void Start()
    {
        SpellListAlternate = SpellList;
    }

    public void AddSpellToSpellLists(Spell spell)
    {
        SpellList.Add(spell);
        SpellListAlternate.Add(spell);
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
                Destroy(currentSpell.gameObject);
            }

            currentSpell = InstantiateSpell(SpellList[spellNumber]);

            if (currentSpell != null)
            {
                // Set the spell for the left hand
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
                Destroy(currentSpell2.gameObject);
            }

            currentSpell2 = InstantiateSpell(SpellListAlternate[spellNumberAlternate]);

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

    private Spell InstantiateSpell(Spell spellPrefab)
    {
        return Instantiate(spellPrefab);
    }
}
