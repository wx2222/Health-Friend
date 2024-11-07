using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthBar : MonoBehaviour
{
   public Slider slider;

   private void Start()
   {
   
      slider.interactable = false;
   }


   public void SetMaxGrowth(int growth)
   {
      slider.maxValue = growth;
      slider.value = growth;
      
   }

   public void SetGrowth(int growth)
   {
      slider.value = growth;
    
   }
}
