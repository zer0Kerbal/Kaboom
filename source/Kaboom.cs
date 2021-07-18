using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Kaboom
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleKaboom : PartModule
    {
        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Kaboom delay",
            groupDisplayName = "<color=red><b>Switch Safety Cover</b></color>", groupName = "Kaboom", groupStartCollapsed = true,
            guiUnits = "Seconds"),
            UI_FloatRange(minValue = 0f, maxValue = 30f, stepIncrement = 1f)]

        public float delay = 0;

        [KSPField(isPersistant = true)]
        public bool timerActive = false;

        [KSPField(isPersistant = true)]
        public double kaboomTime;

        [KSPEvent(guiActive = true, guiActiveUnfocused = true, unfocusedRange = 5f, guiName = "Kaboom!", groupName = "Kaboom", active = true)]
        public void KaboomEvent()
        {
            KaboomIt();
        }

        [KSPEvent(guiActive = true, guiActiveUnfocused = true, unfocusedRange = 5f, guiName = "Cancel Kaboom!", groupName = "Kaboom", active = false)]
        public void CancelKaboomEvent()
        {
            CancelKaboomIt();
        }

        [KSPAction("Kaboom!")]
        public void KaboomAction(KSPActionParam param)
=> KaboomIt();

        private void KaboomIt()
        {
            Events["CancelKaboomEvent"].active = true;
            Events["KaboomEvent"].active = false;
            part.force_activate();

            if (delay == 0)
            {
                part.explode();
            }
            else
            {
                ScreenMessages.PostScreenMessage("Kaboom set for " + delay + " seconds.", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                kaboomTime = Planetarium.GetUniversalTime() + delay;
                timerActive = true;
            }
        }

        private void CancelKaboomIt()
        {
            Events["CancelKaboomEvent"].active = false;
            Events["KaboomEvent"].active = true;
            ScreenMessages.PostScreenMessage("Kaboom cancelled.", 5.0f, ScreenMessageStyle.UPPER_CENTER);
            timerActive = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (timerActive)
            {
                if (Planetarium.GetUniversalTime() >= kaboomTime)
                {
                    timerActive = false;
                    part.explode();
                }
            }
            base.OnUpdate();
        }
    }
}
