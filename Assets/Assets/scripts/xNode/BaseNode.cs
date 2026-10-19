using System;
using UnityEngine;
using XNode;

[Serializable]
public struct EmptyStruct { } // empty struct to use as a placeholder for input/output ports

public abstract class BaseNode : Node
{
    public abstract BaseNode GetNext();

    protected BaseNode Follow(string portName) // follows the connection of the given port name and returns the connected node, or null if not connected
    {
        NodePort port = GetOutputPort(portName);
        if (port == null || !port.IsConnected) return null; // if the port is not found or not connected, return null
        return port.Connection.node as BaseNode;
    }

    public override object GetValue(NodePort port) // override GetValue to return null for all ports, since we don't use them in this base class
    {
        return null;
    }
}
