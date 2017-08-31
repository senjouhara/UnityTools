using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class FpsCounter : MonoBehaviour {

    const float fpsMeasurePeriod = 0.5f;    //FPS测量间隔  
    private int m_FpsAccumulator = 0;   //帧数累计的数量  
    private float m_FpsNextPeriod = 0;  //FPS下一段的间隔  
    private int m_CurrentFps;   //当前的帧率  
    const string display = "{0} FPS";   //显示的文字  
    private GUIStyle bb;
    private double lastInterval;
    private uint AllMemory;
    private void Start()
    {
        m_FpsAccumulator = 0;
        m_FpsNextPeriod = 0;
        //m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        lastInterval = Time.realtimeSinceStartup;
        bb = new GUIStyle();
        //bb.normal.background = null; //这是设置背景填充的  
        bb.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的  
        bb.fontSize = 40; //当然，这是字体颜色  //Time.realtimeSinceStartup获取游戏开始到当前的时间，增加一个测量间隔，计算出下一次帧率计算是要在什么时候  
    }


    void OnGUI()
    {
        //m_FpsAccumulator++;
        //if (Time.realtimeSinceStartup > m_FpsNextPeriod)    //当前时间超过了下一次的计算时间  
        //{
        //    m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);   //计算  
        //    m_FpsAccumulator = 0;   //计数器归零  
        //    m_FpsNextPeriod += fpsMeasurePeriod;    //在增加下一次的间隔  
        //}
        
        GUI.Label(new Rect(20,20,50,20), string.Format(display, m_CurrentFps),bb);
        GUI.Label(new Rect(20,50,100,20), "Memory:" + AllMemory, bb);
    }

    private void Update()
    {
        // 测量每一秒的平均帧率  
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)    //当前时间超过了下一次的计算时间  
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);   //计算  
            m_FpsAccumulator = 0;   //计数器归零  
            m_FpsNextPeriod += fpsMeasurePeriod;    //在增加下一次的间隔  
        }
        //var MonoUsedM = Profiler.GetMonoUsedSize() / 1000000;
        AllMemory = Profiler.GetTotalReservedMemory() / 1000000;
        //++m_FpsAccumulator;
        //float timeNow = Time.realtimeSinceStartup;
        //if (timeNow > lastInterval + fpsMeasurePeriod)
        //{
        //    m_CurrentFps = (int)(m_FpsAccumulator / (timeNow - lastInterval));
        //    m_FpsAccumulator = 0;
        //    lastInterval = timeNow;
        //}

    }

}
