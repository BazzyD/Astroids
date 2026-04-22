using UnityEngine;
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour 
{
    private Animator anim;
    private string lastAnimation;
    private void Awake(){
        anim = GetComponent<Animator>();
    }

    public void Play(string name, bool crossfade = true, float duration = 0.1f) {
        //Debug.Log($"Changing from animation: {lastAnimation} to {name}");
        if (name == lastAnimation) return;
        //Debug.Log($"Playing animation: {name}");
        if (crossfade) anim.CrossFade(name, duration);
        else anim.Play(name);
        
        lastAnimation = name;
    }
}