using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
// Note : Unity does not support Generic Components.
// Nor can you use new to instantaite an object of type Monobehaviour.


// Base class for all state
public abstract class State<T>{
	protected List<Transition<T>> transitions;
	protected MonoBehaviour controller;
    protected float minDuration;
    protected bool triggerEntered = false;
    protected bool stateFinished = false;
    public float onEnterTime;
    public float onLeaveTime;
    private T stateName;
    public T StateName {
        get { return stateName; }
    }
    protected int animation;
    protected bool animationStarted;
    protected AnimatorStateInfo animationLayer;
    protected int layer;

    public bool StateFinished {
        get { return stateFinished; }
    }
	
	public State(T stateName, MonoBehaviour controller, float minDuration){
        this.stateName = stateName;
		transitions = new List<Transition<T>>();
        this.controller = controller;
        this.minDuration = minDuration;
	}

    public State(T stateName, MonoBehaviour controller, float minDuration, int animation, int layer) {
        this.stateName = stateName;
        transitions = new List<Transition<T>>();
        this.controller = controller;
        this.minDuration = minDuration;
        this.animation = animation;
        this.layer = layer;
    }
	
	public void AddTransition(T fromState,T toState){
		transitions.Add(new Transition<T>(fromState,toState,controller,this));
	}
	
	public T CheckGuards(out bool changed){
       // Debug.Log("onEnterTime " + onEnterTime + " Min " + minDuration);
        // Ensure the min duration time has elapsed before checking for state changes
        if (onEnterTime + minDuration < Time.time) {
            // Check each transition
            foreach (Transition<T> trans in transitions) {
                // If the guard returns true a transition will take place
                if (trans.InvokeGuard()) {
                    changed = true;
                    return trans.toState;
                }
            }
        }
		changed = false;
		return default(T);
	}

	public string GetName(){
		string name = this.GetType().ToString();
		return name.Remove(name.IndexOf('`'));
	}

	public virtual void OnLeave(){
        Debug.Log("OnLeave " + GetName() + " : " + Time.time);
        onLeaveTime = Time.time;
    }
	
	public virtual void OnEnter(){
        onEnterTime = Time.time;
        Debug.Log("OnEnter " + GetName() + " : " + Time.time);
        stateFinished = false;
        triggerEntered = false;
        animationStarted = false;
    }

    // Monitor is invoked by the state machine each time the machine checks for state changes.
    public virtual void Monitor() { }

    // Act should be invoked within an Update or FixedUpdate callback and include code that moves GameObjects
    public virtual void Act() {
    
    }

    public virtual void OnStateTriggerEnter(Collider collider) {
        triggerEntered = true;
    }

    public virtual bool AnimationEnded() {
        return false;
    }


	
}
