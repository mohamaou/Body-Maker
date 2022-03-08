using UnityEngine;
using UnityEngine.UI;

public class SkillsBar : MonoBehaviour
{
   [SerializeField] private Image damage, speed, health,damageAdded, speedAdded, healthAdded;
   [SerializeField] private Body body;
   private float damageValue, speedValue, healthValue, damageAddedValue, speedAddedValue, healthAddedValue;


   private void Start()
   {
      damageValue = body.defaultBody.damage;
      speedValue = body.defaultBody.speed;
      healthValue = body.defaultBody.health;
      
      damage.fillAmount = damageAdded.fillAmount = damageValue;
      speed.fillAmount = speedAdded.fillAmount = speedValue;
      health.fillAmount = healthAdded.fillAmount = healthValue;
      body.powerInfo = new BodyInfo(0, 0, 0);
   }
   

   public void SetValue(BodyInfo bodyInfo)
   {
      damageValue += bodyInfo.damage;
      speedValue += bodyInfo.speed;
      healthValue += bodyInfo.health;
      damageValue = Mathf.Clamp(damageValue, 0, 1);
      speedValue = Mathf.Clamp(speedValue, 0, 1);
      healthValue = Mathf.Clamp(healthValue, 0, 1);
      body.powerInfo = new BodyInfo(damageValue, speedValue, healthValue);
   }

   public void SetAddedValue(BodyInfo bodyInfo)
   {
      damageAddedValue = bodyInfo.damage;
      speedAddedValue = bodyInfo.speed;
      healthAddedValue = bodyInfo.health;
   }


   private void Update()
   {
      damage.fillAmount = Mathf.Lerp(damage.fillAmount, damageValue, 6 * Time.deltaTime);
      speed.fillAmount = Mathf.Lerp(speed.fillAmount, speedValue, 6 * Time.deltaTime);
      health.fillAmount = Mathf.Lerp(health.fillAmount, healthValue, 6 * Time.deltaTime);
      
      damageAdded.fillAmount = Mathf.Lerp(damageAdded.fillAmount, damageValue + damageAddedValue, 6 * Time.deltaTime);
      speedAdded.fillAmount = Mathf.Lerp(speedAdded.fillAmount, speedValue + speedAddedValue, 6 * Time.deltaTime);
      healthAdded.fillAmount = Mathf.Lerp(healthAdded.fillAmount, healthValue + healthAddedValue, 6 * Time.deltaTime);
   }
}
