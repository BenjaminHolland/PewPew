using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoundaryHandler
{
    void Handle(Vector2 position);
}
interface IBoundaryHandlerBuilder
{
    IBoundaryHandlerBuilder OnAboveBoundary(Action response);
    IBoundaryHandlerBuilder OnBelowBoundary(Action response);
    IBoundaryHandlerBuilder OnRightOfBoundary(Action response);
    IBoundaryHandlerBuilder OnLeftOfBoundary(Action response);
    IBoundaryHandlerBuilder OnOutOfBoundary(Action response);
    IBoundaryHandler Build();
}
class RectBoundaryHandler : IBoundaryHandlerBuilder, IBoundaryHandler;
{
    public RectBoundaryHandler(Rect boundary)
    {
        this.boundary = boundary;
    }
    private Action onAboveBoundary = null;
    private Action onBelowBoundary = null;
    private Action onLeftOfBoundary = null;
    private Action onRightOfBoundary = null;
    private Action onOutOfBondary = null;
    private Rect boundary;
    public IBoundaryHandler Build()
    {
        return this;
    }

    public void Handle(Vector2 position)
    {
        bool isOut = false;
        if (position.x < boundary.left)
        {
            if(onLeftOfBoundary!=null) { onLeftOfBoundary(); }
            isOut = true;
        }
        if (position.x > boundary.right)
        {
            if (onRightOfBoundary != null) { onRightOfBoundary(); }
            isOut = true;
        }
        if (position.y < boundary.top)
        {
            if (onAboveBoundary != null) { onAboveBoundary(); }
            isOut = true;
        }
        if (position.y > boundary.bottom)
        {
            if(onBelowBoundary!=null) { onBelowBoundary(); }
            isOut = true;
        }
        if (isOut)
        {
            if(onOutOfBondary!=null) { onOutOfBondary(); }
        }
    }

    public IBoundaryHandlerBuilder OnAboveBoundary(Action response)
    {
        onAboveBoundary = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnBelowBoundary(Action response)
    {
        onBelowBoundary = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnLeftOfBoundary(Action response)
    {
        onLeftOfBoundary = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnOutOfBoundary(Action response)
    {
        onOutOfBondary = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnRightOfBoundary(Action response)
    {
        onRightOfBoundary = response;
        return this;
    }
}

class BoundaryUtils{
    /// <summary>
    /// Provide a utility instance that will allow specifying fluent boundary checks for the given rect. 
    /// </summary>
    /// <param name="boundary"></param>
    /// <returns></returns>    
    public static IBoundaryHandlerBuilder ForBoundary(Rect boundary)
    {
        return new RectBoundaryHandler(boundary);
    }
}

