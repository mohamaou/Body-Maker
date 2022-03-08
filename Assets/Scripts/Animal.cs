using UnityEngine;

public enum AnimalType
{
   Null, Boar, Deer, Fox, Moose, Tiger, Wolf
}


public class Animal : MonoBehaviour
{
    public AnimalType animalType;
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] private Transform gfx;
    private BodyType bodyType = BodyType.Null;
    private Quaternion startRotation;
    
    
    


    private void Start()
    {
        startRotation = gfx.localRotation;
    }


    void Update()
    {
        SetShapeKey(bodyType);
        Movement();
    }

    #region Shape Key

    public void SetPart(BodyType localBodyType)
    {
        bodyType = localBodyType;
    }
    
    private void SetShapeKey(BodyType localBodyType)
    {
        var index = 100;
        switch (localBodyType)
        {
            case BodyType.Arm:
                index = 1;
                break;
            case BodyType.Leg:
                index = 0;
                break;
            case BodyType.Head:
                index = 3;
                break;
            case BodyType.Hips:
                index = 2;
                break;
        }

        if (mesh == null) return;
        for (int i = 0; i < 4; i++)
        {
            if (i == index)
            {
                var value = Mathf.Lerp(mesh.GetBlendShapeWeight(i), 100, 6 * Time.deltaTime);
                mesh.SetBlendShapeWeight(i,value);
            }
            else
            {
                var value = Mathf.Lerp(mesh.GetBlendShapeWeight(i), 0, 6 * Time.deltaTime);
                mesh.SetBlendShapeWeight(i,value);
            }
        }
    }

    #endregion
    
    #region Movement
    private void Movement()
    {
        if (bodyType == BodyType.Null)
        {
            gfx.localRotation = Quaternion.Lerp(gfx.localRotation,startRotation,6*Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero,6*Time.deltaTime);
            gfx.rotation = Quaternion.Lerp(gfx.rotation,transform.parent.rotation , 6*Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one ,6*Time.deltaTime);
        }
    }
    public void MoveWithMouse(DragObject dragObject, DefaultBody defaultBody)
    {
        if(Input.GetMouseButton(0))transform.localScale = Vector3.one * (1 - SwipeMenu.MouseHeight(1));
        var position = transform.position;
        position = Vector3.Lerp(position, dragObject.MovingObject(position),6 * Time.deltaTime);
        if (position.y < 0) position.y = 0;
        transform.position =position;

        var rotation = transform.rotation;
        var targetRotation = Quaternion.LookRotation(position - defaultBody.HipsPosition);
        rotation = Quaternion.Lerp(rotation,targetRotation, 6*Time.deltaTime);
        transform.rotation = rotation;
        
        SetPart(BodyType.Null);
      
    }
    #endregion
   
}
