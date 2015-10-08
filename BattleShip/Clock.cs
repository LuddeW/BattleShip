using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BattleShip
{
    class Clock
    {       
        float Timer;
        int RadarRotation = 0;
 
        public Clock()
        {
            Timer = 0;
        }
        public void AddTime(float Time)
        {
            Timer += Time;
        }
        public float Timern()
        {
            return Timer;
        }
        public void ResetTime()
        {
            
                Timer = 0;          
        }
        public int GetRotationForRadar()
        {
            
            if (Timer > 0.05f)
            {
                RadarRotation++;
                ResetTime();
            }
            if (RadarRotation > 11)
            {
                RadarRotation = 0;
            }
            return RadarRotation;
        }
    }
}
