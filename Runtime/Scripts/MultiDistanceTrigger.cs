using MixedReality.Toolkit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class MultiDistanceTrigger : MonoBehaviour, ICondition, IHandedComponent
{
    public static MultiDistanceTrigger Instance;

    public Transform[] Targets;
    public float MinInitDistance;
    public float TriggerDistance;
    public bool Invert;
    public bool Average;

    public UnityEvent OnTrigger;

    public string DebugText;

    [SerializeField] private Handedness handedness;
    [SerializeField, ReadOnly]
    private bool init;
    private float maxDistance;
    [SerializeField, ReadOnly]
    private bool met;

    public float MaxDistance { get => maxDistance; }
    public bool Met
    {
        get => met;
        set
        {
            if (met != value)
            {
                met = value;
                OnConditionChanged.Invoke(value);
            }
        }
    }
    public UnityEvent<bool> OnConditionChanged { get; set; } = new UnityEvent<bool>();
    public Handedness Hand { get => handedness; set => handedness = value; }

    [SerializeField, ReadOnly] private float distance;

    private void Start()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        init = false;
    }

    private void Update()
    {
        if (HandManager.Instance.IsHandTracked(handedness))
        {
            if (!init)
            {
                //Not initialized yet, waiting for minimum distance to be reached before enabling trigger
                var inited = false;

                for (int i = 0; i < Targets.Length; i++)
                {
                    for (int j = 0; j < Targets.Length; j++)
                    {
                        if (i != j)
                        {
                            if (!Invert && (Targets[i].position - Targets[j].position).magnitude > MinInitDistance)
                            {
                                inited = true;
                                if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Distance trigger initialized: {gameObject.name}");
                            }
                            else if (Invert && (Targets[i].position - Targets[j].position).magnitude < MinInitDistance)
                            {
                                inited = true;
                                if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Distance trigger initialized: {gameObject.name}");
                            }
                        }
                    }
                }

                if (inited) init = true;
            }
            else
            {
                //Is hand tracked?
                if (HandManager.Instance.IsHandTracked(handedness))
                {
                    //Initialized, now the trigger can be activated

                    var triggered = true;

                    if (!Average)
                    {
                        for (int i = 0; i < Targets.Length; i++)
                        {
                            for (int j = 0; j < Targets.Length; j++)
                            {
                                if (i != j)
                                {
                                    distance = (Targets[i].position - Targets[j].position).magnitude;
                                    if (distance > maxDistance) maxDistance = distance;
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
                    }
                    else
                    {
                        distance = 0f;
                        for (int i = 0; i < Targets.Length; i++)
                        {
                            for (int j = 0; j < Targets.Length; j++)
                            {
                                if (i != j)
                                {
                                    distance += (Targets[i].position - Targets[j].position).magnitude;
                                }
                            }
                        }

                        distance /= Targets.Length;

                        if (!Invert && distance > TriggerDistance)
                        {
                            triggered = false;
                        }
                        else if (Invert && distance < TriggerDistance)
                        {
                            triggered = false;
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
        }
        else Met = false;
    }

    public void TriggerDirectly()
    {
        OnTrigger.Invoke();
    }

    public void Reset()
    {
        init = false;
    }
}
