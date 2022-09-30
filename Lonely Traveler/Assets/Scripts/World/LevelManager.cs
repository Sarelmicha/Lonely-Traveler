using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HappyFlow.LonelyTraveler.World.LevelExposure;
using UnityEngine;

/// <summary>
/// This class responsible for managing the level (Start level process, Restart level process)
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelExposureStrategy m_LevelExposureStrategy;
    [SerializeField] private LevelLightManager m_LevelLightManager;
    
    public event Action<bool> OnLevelShouldRestart;
    public event Action OnLevelStarted;
    private Animator m_Transitions;
    private bool m_ShouldExecuteFullRestart;
    private List<Coroutine> m_RunningCoroutines;
    
    /// <summary>
    /// Start restarting the level process.
    /// </summary>
    /// <param name="shouldExecuteFullRestart">A flag that indicate whether to execute a full restart</param>
    public void StartRestartLevel(bool shouldExecuteFullRestart)
    {
        m_ShouldExecuteFullRestart = shouldExecuteFullRestart;
        m_Transitions.SetTrigger("Flash");
    }

    /// <summary>
    /// Restart the level.
    /// Called from the animation key frame in the editor.
    /// </summary>
    public void RestartLevel()
    {
        OnLevelShouldRestart?.Invoke(m_ShouldExecuteFullRestart);
        m_LevelExposureStrategy.Reset(m_ShouldExecuteFullRestart);
        m_LevelLightManager.Reset(m_ShouldExecuteFullRestart);

        StopRunningRoutines();

        if (m_ShouldExecuteFullRestart)
        {
            ExposeLevel();
        }
    }
    private void Awake()
    {
        m_Transitions = GetComponent<Animator>();
        m_RunningCoroutines = new List<Coroutine>();
    }

    private void Start()
    {
        ExposeLevel();
    }

    private void StopRunningRoutines()
    {
        foreach (var runningCoroutine in m_RunningCoroutines.Where(runningCoroutine => runningCoroutine != null))
        {
            StopCoroutine(runningCoroutine);
        }

        m_RunningCoroutines.Clear();
    }

    private void ExposeLevel()
    {
        IlluminateLevel(() =>
        {
            m_LevelExposureStrategy.Expose(OnLevelExposed);
        });
    }

    private void IlluminateLevel(Action onComplete = null)
    {
        Coroutine illuminateCoroutine = null;
        
        illuminateCoroutine = StartCoroutine(m_LevelLightManager.IlluminateLevel(() =>
        {
            if (m_RunningCoroutines.Contains(illuminateCoroutine))
            {
                m_RunningCoroutines.Remove(illuminateCoroutine); 
            }
            
            onComplete?.Invoke();
        }));
        
        m_RunningCoroutines.Add(illuminateCoroutine);
    }

    private void DarkenLevel(Action onComplete = null)
    {
        Coroutine illuminateCoroutine = null;
        
        illuminateCoroutine = StartCoroutine(m_LevelLightManager.DarkenLevel(() =>
        {
            if (m_RunningCoroutines.Contains(illuminateCoroutine))
            {
                m_RunningCoroutines.Remove(illuminateCoroutine); 
            }
            onComplete?.Invoke();
        }));
        
        m_RunningCoroutines.Add(illuminateCoroutine);
    }
    private void OnLevelExposed()
    {
        DarkenLevel();
        OnLevelStarted?.Invoke();
    }
}