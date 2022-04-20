using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerson : MonoBehaviour
{
    [SerializeField] Text text;
    Person person;

    public void SetPerson(Person person)
    {
        this.person = person;
        text.text = "Person " + person.personIndex.ToString();
    }
}
