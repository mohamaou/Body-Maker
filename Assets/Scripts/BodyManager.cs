using System;
using DG.Tweening;
using UnityEngine;


public enum BodyType
{
    Head, Leg, Arm, Hips, Null
}

public enum PartType
{
    Head, Hips, LeftArm, RightArm, LeftLeg, RightLeg, Null
}
public class DragObject
{
    public DragObject(Camera camera)
    {
        cam = camera;
    }
    private Vector3 _mOffset;
    private Camera cam;
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 2.5f;
        return cam.ScreenToWorldPoint(mousePoint);

    }
    public Vector3 MovingObject(Vector3 holdObject)
    {
        var targetPoint  = GetMouseAsWorldPoint();
        return new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);
    }
}

[Serializable]
public class BodyParts
{
    [SerializeField] private Transform head, hips, leftArm, rightArm, leftLeg, rightLeg;
    public Transform GetClosestPart(Vector3 position)
    {
        var minDistance = Mathf.Infinity;
        Transform closestPoint = null;
        var allParts = new[] { head, hips, rightLeg, leftLeg, rightArm, leftArm};
        for (int i = 0; i < allParts.Length; i++)
        {
            var dist = Vector3.Distance(allParts[i].position, position);
            if (dist < minDistance)
            {
                closestPoint = allParts[i];
                minDistance = dist;
            }
        }
        return closestPoint;
    }
    public BodyType GetBodyType(Transform part)
    {
        if (part == rightArm || part == leftArm)
        {
            return BodyType.Arm;
        }
        if (part == leftLeg || part == rightLeg)
        {
            return BodyType.Leg;
        }
        if (part == head)
        { 
            return BodyType.Head;
        }
        if (part == hips)
        {
            return BodyType.Hips;
        }
        return BodyType.Null;
    }
    public PartType GetPartType(Transform part)
    {
        if (part == head) return PartType.Head;
        if (part == hips) return PartType.Hips;
        if (part == leftArm) return PartType.LeftArm;
        if (part == rightArm) return PartType.RightArm;
        if (part == leftLeg) return PartType.LeftLeg;
        if (part == rightLeg) return PartType.RightLeg;
        return PartType.Null;
    }
}

[Serializable]
public class DefaultBody
{
    [SerializeField] private GameObject head, hips, leftArm, rightArm, leftLeg, rightLeg;
    private bool headFull, hipsFull, leftArmFull, rightArmFull, leftLegFull, rightLegFull;

    public Vector3 HipsPosition => hips.transform.position;
    
    public void HideBodyPart(PartType partType = PartType.Null)
    {
        if (partType == PartType.Head || headFull) head.transform.DOScale(0, 0.4f);
        else head.transform.DOScale(1, 0.4f);
        
        if (partType == PartType.Hips || hipsFull) hips.transform.DOScale(0, 0.4f);
        else hips.transform.DOScale(1, 0.4f);
        
        if (partType == PartType.LeftArm || leftArmFull) leftArm.transform.DOScale(0, 0.4f);
        else leftArm.transform.DOScale(1, 0.4f);
        
        if (partType == PartType.RightArm || rightArmFull) rightArm.transform.DOScale(0, 0.4f);
        else rightArm.transform.DOScale(1, 0.4f);
        
        if (partType == PartType.LeftLeg || leftLegFull) leftLeg.transform.DOScale(0, 0.4f);
        else leftLeg.transform.DOScale(1, 0.4f);
        
        if (partType == PartType.RightLeg || rightLegFull) rightLeg.transform.DOScale(0, 0.4f);
        else rightLeg.transform.DOScale(1, 0.4f);
    }

    public void SetPartFull(PartType partType)
    {
        switch (partType)
        {
            case PartType.Head:
                headFull = true;
                break;
            case PartType.Hips:
                hipsFull = true;
                break;
            case PartType.LeftArm:
                leftArmFull = true;
                break;
            case PartType.RightArm:
                rightArmFull = true;
                break;
            case PartType.LeftLeg:
                leftLegFull = true;
                break;
            case PartType.RightLeg:
                rightLegFull = true;
                break;
        }
    }

    public bool IsPartFull(PartType partType)
    {
        switch (partType)
        {
            case PartType.Head:
                return headFull;
            case PartType.Hips:
                return hipsFull;
            case PartType.LeftArm:
                return leftArmFull;
            case PartType.RightArm:
                return rightArmFull;
            case PartType.LeftLeg:
                return leftLegFull;
            case PartType.RightLeg:
                return rightLegFull;
        }
        return false;
    }
}

public class BodyManager : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private LayerMask bodyLayer, bodyPartLayer;
    [SerializeField] private BodyParts bodyParts;
    [SerializeField] private DefaultBody defaultBody;
    [SerializeField] private SkillsBar skillsBar;
    [SerializeField] private AnimalManager animalsManager;
    [SerializeField] private Body bodyData;
    private DragObject dragObject;
    private Animal animal;
    private bool getBody;
    private static bool inBody;
    private int partFull;
    private Camera cam;
    public static bool IsInBody => inBody;
    
    private void Start()
    {
        cam = Camera.main;
        bodyData.ResetBody();
        dragObject = new DragObject(cam);
        inBody = false;
    }

  
    private void Update()
    {
        RotateBody();
        MoveParts();
    }

    private RaycastHit RayCast(LayerMask layerMask)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask));
        return hit;
    } 
    private void RotateBody()
    {
        if (Input.GetMouseButtonDown(0))
        {
            getBody = RayCast(bodyLayer).transform != null;
        }
        
        if (getBody && Input.GetMouseButton(0))
        {
            if(Swipe.SwipeDirection() == Direction.Left) body.Rotate(0,180*Time.deltaTime,0);
            if(Swipe.SwipeDirection() == Direction.Right) body.Rotate(0,-180*Time.deltaTime,0);
        }
    }

    
    private void MoveParts()
    {
        if (animal == null) return;
        var hit = RayCast(bodyPartLayer);
        if (Input.GetMouseButton(0))
        {
            if (hit.transform == null)
            {
               MoveWithMouse();
            }
            else
            {
                var target = bodyParts.GetClosestPart(hit.point);
                if (defaultBody.IsPartFull(bodyParts.GetPartType(target)))
                {
                    MoveWithMouse();
                    return;
                }
                inBody = true; 
                skillsBar.SetAddedValue(PowerInfo(bodyParts.GetBodyType(target),animal.animalType));
                animal.transform.SetParent(target);
                animal.SetPart(bodyParts.GetBodyType(target));
                defaultBody.HideBodyPart(bodyParts.GetPartType(target));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseAnimal(hit);
        }
    }
    private void MoveWithMouse()
    {
        inBody = false;
        animal.MoveWithMouse(dragObject, defaultBody);
        skillsBar.SetAddedValue(new BodyInfo(0,0,0));
        defaultBody.HideBodyPart();
    }

    private BodyInfo PowerInfo(BodyType bodyType, AnimalType animalType)
    {
        AnimalInfo animalInfo = new AnimalInfo();
        switch (animalType)
        {
            case AnimalType.Fox: animalInfo = bodyData.fox;
                break;
            case AnimalType.Tiger: animalInfo = bodyData.tiger;
                break;
            case AnimalType.Deer: animalInfo = bodyData.deer;
                break;
            case AnimalType.Moose: animalInfo = bodyData.moose;
                break;
            case AnimalType.Boar: animalInfo = bodyData.boar;
                break;
            case AnimalType.Wolf: animalInfo = bodyData.wolf;
                break;
        }

        var info = new BodyInfo(0,0,0);
        switch (bodyType)
        {
            case BodyType.Head: info = animalInfo.head;
                break;
            case BodyType.Hips: info = animalInfo.hips;
                break;
            case BodyType.Arm: info = animalInfo.arm;
                break;
            case BodyType.Leg: info = animalInfo.leg;
                break;
        }

        return info;
    }

    private void ReleaseAnimal(RaycastHit hit)
    {
        if (inBody)
        {
            partFull ++;
            var part = bodyParts.GetPartType(bodyParts.GetClosestPart(hit.point));
            defaultBody.SetPartFull(part);
            bodyData.SetPart(animal.animalType,part);
            skillsBar.SetValue(PowerInfo(bodyParts.GetBodyType(bodyParts.GetClosestPart(hit.point)),animal.animalType));
            skillsBar.SetAddedValue(new BodyInfo(0,0,0));
            if(partFull >= 2)StartUi.Instance.ShowPlayButton();
        }
        else
        {
            skillsBar.SetAddedValue(new BodyInfo(0,0,0));
            animal.transform.DOScale(0,0.4f);
            animal.transform.DOMove(Vector3.back * 1.5f, 0.4f);
            Destroy(animal.gameObject,0.4f);
        }
        animal = null;
    }
    public void SetAnimal(String animalType)
    {
        if (animal != null) return;
        var a = animalsManager.GetAnimal(animalType);
        animal = Instantiate(a);
        animal.transform.position = dragObject.MovingObject(animal.transform.position);
        animal.transform.rotation = Quaternion.LookRotation(animal.transform.position - defaultBody.HipsPosition);
        animal.transform.localScale = Vector3.zero;
    }

   
    
   
}
