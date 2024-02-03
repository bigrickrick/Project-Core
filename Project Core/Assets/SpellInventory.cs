using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    public List<Spell> SpellList = new List<Spell>();
    public bool HasSpellInleftHand;
    public bool HasSpellInRightHand;
    public Spell currentSpell;
    public Spell currentSpell2;
    public int spellNumber;

    private void Update()
    {
        if (SpellList.Count > 0)
        {
            if (!HasSpellInleftHand)
            {
                MakeSpellAppearInPlayerHand();
                HasSpellInleftHand = true;
            }
        }
    }

    public void SpellSwitch(int number)
    {
        spellNumber += number;

        HasSpellInleftHand = false;
        if (spellNumber >= SpellList.Count)
        {
            spellNumber = 0;
        }
        else if (spellNumber < 0)
        {
            spellNumber = SpellList.Count - 1;
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
                currentSpell.transform.SetParent(transform);
                currentSpell.transform.localPosition = Vector3.zero;
                currentSpell.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellList is null or empty.");
        }
        if (SpellList != null && SpellList.Count > 0)
        {
            if (currentSpell2 != null)
            {
                Destroy(currentSpell2.gameObject);
            }

            currentSpell2 = InstantiateSpell(SpellList[spellNumber]);

            if (currentSpell2 != null)
            {
                currentSpell2.transform.SetParent(transform);
                currentSpell2.transform.localPosition = Vector3.zero;
                currentSpell2.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellList is null or empty.");
        }
    }

    private Spell InstantiateSpell(Spell spellPrefab)
    {
        
        return Instantiate(spellPrefab);
    }
}
