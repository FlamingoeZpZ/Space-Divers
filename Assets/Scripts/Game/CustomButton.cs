
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class CustomButton : Button
{
    [SerializeField] private UnityEvent onPointerUpEvent;
    [SerializeField] private UnityEvent onPointerDownEvent;
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUpEvent.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onPointerDownEvent.Invoke();

    }
}

 
#if UNITY_EDITOR
[CustomEditor(typeof(CustomButton), true)]
public class AeButtonEditor : ButtonEditor
{
    SerializedProperty onPointerUpEvent;
    SerializedProperty onPointerDownEvent;
 
    protected override void OnEnable()
    {
        base.OnEnable();
        onPointerUpEvent = serializedObject.FindProperty("onPointerUpEvent");
        onPointerDownEvent = serializedObject.FindProperty("onPointerDownEvent");
    }
 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
 
        serializedObject.Update();
        EditorGUILayout.PropertyField(onPointerUpEvent);
        EditorGUILayout.PropertyField(onPointerDownEvent);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif