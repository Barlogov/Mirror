using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DotsManager : NetworkBehaviour
{
    List<Person> persons = new List<Person>();

    public void AddPerson(Person _person)
    {
        persons.Add(_person);
    }
}
