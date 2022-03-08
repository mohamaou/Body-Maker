using UnityEngine;


[CreateAssetMenu(fileName = "Animals")]
public class AnimalManager : ScriptableObject
{
    [SerializeField] private Animal fox, tiger, deer, moose, boar, wolf;

    public Animal GetAnimal(string animal)
    {
        switch (animal)
        {
            case "Fox": return fox;
            case "Tiger": return tiger;
            case "Deer": return deer;
            case "Moose": return moose;
            case "Boar": return boar;
            case "Wolf": return wolf;
        }
        return null;
    }
}
