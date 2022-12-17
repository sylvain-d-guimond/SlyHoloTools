using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Events;

public class MultiDistanceTrigger : MonoBehaviour, ICondition
{
    public static MultiDistanceTrigger Instance;

    public Transform[] Targets;
    public float MinInitDistance;
    public float TriggerDistance;
    public bool Invert;

    public UnityEvent OnTrigger;

    public string DebugText;

    private bool _init;
    private float _maxDistance;
    private bool _met;

    public float MaxDistance { get => _maxDistance; }
    public bool Met
    {
        get => _met;
        set
        {
            if (_met != value)
            {
                _met = value;
                OnConditionChanged.Invoke(value);
            }
        }
    }
    public ConditionEvent OnConditionChanged { get; set; } = new ConditionEvent();

    private void Start()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _init = false;
    }

    private void Update()
    {
        if (!_init)
        {
            //Not initialized yet, waiting for minimum distance to be reached before enabling trigger
            var inited = false;

            for (int i=0;i<Targets.Length; i++)
            {
                for (int j=0; j<Targets.Length; j++)
                {
                    if (i != j)
                    {
                        if (!Invert && (Targets[i].position - Targets[j].position).magnitude > MinInitDistance)
                        {
                            inited = true;
                            if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Distance trigger initialized: {gameObject.name}");
                        } else if (Invert && (Targets[i].position - Targets[j].position).magnitude < MinInitDistance)
                        {
                            inited = true;
                            if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Distance trigger initialized: {gameObject.name}");
                        }
                    }
                }
            }

            if (inited) _init = true;
        }
        else
        {
            //Initialized, now the trigger can be activated

            var triggered = true;

            for (int i = 0; i < Targets.Length; i++)
            {
                for (int j = 0; j < Targets.Length; j++)
                {
                    if (i != j)
                    {
                        var distance = (Targets[i].position - Targets[j].position).magnitude;
                        if (distance > _maxDistance) _maxDistance = distance;
                        if (!Invert && distance > TriggerDistance)
                        {
                            triggered = false;
                        }
                        else if (Invert && distance < TriggerDistance)
                        {
                            triggered = false;
                        }
                    }
                }
            }

            if (triggered)
            {
                if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Distance trigger {gameObject.name} called: {DebugText}");
                OnTrigger.Invoke();
                Met = true;
            }
            else { Met = false; }
        }
    }

    public void TriggerDirectly()
    {
        OnTrigger.Invoke();
    }
}
