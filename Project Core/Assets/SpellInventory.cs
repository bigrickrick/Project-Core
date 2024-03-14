using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    public List<Spell> SpellList = new List<Spell>();
    public Spell currentspell;
    public List<Element> ElementListlefthand = new List<Element>();
    public List<Element> ElementListrighthand = new List<Element>();
    public bool HasSpellInleftHand;
    public bool HasSpellInRightHand;
    public Element currentElementleft;
    public Element currentElementright;
    public int spellNumber;
    public int spellNumberAlternate;
    public Transform LeftHand;
    public Transform RightHand;
    public Projectile spellProjectile;
    

    public void AddSpellToSpellLists(Element element,Element element2)
    {
        ElementListlefthand.Add(element);
        ElementListrighthand.Add(element2);
        
    }
    private void Start()
    {
        SetSpell();
    }
    private void Update()
    {
        if (SpellList.Count > 0)
        {
            if (!HasSpellInleftHand || !HasSpellInRightHand)
            {
                MakeSpellAppearInPlayerHand();
                HasSpellInleftHand = true;
                HasSpellInRightHand = true; 
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
            if (spellNumber >= ElementListlefthand.Count)
            {
                spellNumber = 0;
            }
            else if (spellNumber < 0)
            {
                spellNumber = ElementListlefthand.Count - 1;
            }
        }
        else if (number == 1)
        {
            spellNumberAlternate++;
            HasSpellInleftHand = false;
            HasSpellInRightHand = false;
            if (spellNumberAlternate >= ElementListlefthand.Count)
            {
                spellNumberAlternate = 0;
            }
            else if (spellNumberAlternate < 0)
            {
                spellNumberAlternate = ElementListrighthand.Count - 1;
            }
        }
    }

    private void MakeSpellAppearInPlayerHand()
    {
        if (SpellList != null && SpellList.Count > 0)
        {
            if (currentElementleft != null)
            {
                currentElementleft.gameObject.SetActive(false);
                currentElementleft = null;
            }

            currentElementleft = ElementListlefthand[spellNumber];
            currentElementleft.gameObject.SetActive(true);
            if (currentElementleft != null)
            {
                
                currentElementleft.transform.SetParent(LeftHand);
                currentElementleft.transform.localPosition = Vector3.zero;
                currentElementleft.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellList is null or empty.");
        }

        if (ElementListrighthand != null && ElementListrighthand.Count > 0)
        {
            if (currentElementright != null)
            {
                currentElementright.gameObject.SetActive(false);
                
            }

            currentElementright = ElementListrighthand[spellNumberAlternate];
            currentElementright.gameObject.SetActive(true);
            if (currentElementright != null)
            {
                // Set the spell for the right hand
                currentElementright.transform.SetParent(RightHand);
                currentElementright.transform.localPosition = Vector3.zero;
                currentElementright.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.LogError("SpellListAlternate is null or empty.");
        }
        SetSpell();
    }
    private void SetSpell()
    {
        if(currentElementleft.element == Element.SpellElement.Fire && currentElementright.element == Element.SpellElement.Fire)
        {
            currentspell = SpellList[0];
        }
        else if(currentElementleft.element == Element.SpellElement.space && currentElementright.element == Element.SpellElement.space)
        {
            currentspell = SpellList[1];
        }
        else if(currentElementleft.element == Element.SpellElement.Fire && currentElementright.element == Element.SpellElement.space || currentElementleft.element == Element.SpellElement.space && currentElementright.element == Element.SpellElement.Fire)
        {
            currentspell = SpellList[2];
        }
        
        
    }
   
    private Spell InstantiateSpell(Spell spellPrefab)
    {
        return Instantiate(spellPrefab);
    }
}
