using System;
using UnityEngine;


[Serializable]
public class BodyInfo
{
    public BodyInfo(float damage , float speed, float health)
    {
       this.damage = damage;
       this.speed = speed;
       this.health = health;
    }
    [Range(0, 1)] public float damage, speed, health;
}

[Serializable]
public class AnimalInfo
{
    public BodyInfo head, hips, arm, leg;
}



[CreateAssetMenu(fileName = "Body")]
public class Body : ScriptableObject
{
    [Header("Player Data")] 
    public AnimalType head;
    public AnimalType hips, leftArm, rightArm, leftLeg, rightLeg;
    public BodyInfo powerInfo;
    
    
    [Header("Power")] public BodyInfo defaultBody;
    public AnimalInfo fox, tiger, deer, moose, boar, wolf;

    

    public void SetPart(AnimalType animal , PartType part)
    {
        switch (part)
        {
            case PartType.Head:
                head = animal;
                break;
            case PartType.Hips:
                hips = animal;
                break;
            case PartType.LeftArm:
                leftArm = animal;
                break;
            case PartType.RightArm:
                rightArm = animal;
                break;
            case PartType.LeftLeg:
                leftLeg = animal;
                break;
            case PartType.RightLeg:
                rightLeg = animal;
                break;
        }
    } 
    
    public void ResetBody()
    {
        head = hips = leftArm = rightArm = leftLeg = rightLeg = AnimalType.Null;
    }
}
