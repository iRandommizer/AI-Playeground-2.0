using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    //This is the cotainer object to store the set of states

    /*!We will use a Dictionary container class to 
     * store the key, value pair of the set of states.
     * The key will be a unique ID for an instantiate
     * application-specific State object.*/

    /*A dictionary was used here instead of a list or array because
     *we can make use of a key(only dictionary has) to add and extract
     * a specific state from the set of states*/
    protected Dictionary<int, State> m_states;

    //The current state the FSM is in
    public State m_currentState;

    public FSM()
    {
        m_states = new Dictionary<int, State>();
    }

    //Add states in the dictionary using an integer value plus state arguments
    public void Add(int key, State state)
    {
        m_states.Add(key, state);
    }

    //Get the state according to int given(with the help of enums)
    public State GetState(int key)
    {
        return m_states[key];
    }

    public void SetCurrentState(State newState)
    {
        //Check if current state is not null, then run exit function
        if (m_currentState != null)
        {
            m_currentState.Exit();
        }

        //Change state to new state
        m_currentState = newState;

        //Check if current state is not null, then run exit function
        if (m_currentState != null)
        {
            m_currentState.Enter();
        }
    }

    public void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (m_currentState != null)
        {
            m_currentState.FixedUpdate();
        }
    }
}
