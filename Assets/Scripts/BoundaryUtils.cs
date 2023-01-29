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
    IBoundaryHandlerBuilder OnAboveYMax(Action response);
    IBoundaryHandlerBuilder OnBelowYMin(Action response);
    IBoundaryHandlerBuilder OnAboveXMax(Action response);
    IBoundaryHandlerBuilder OnBelowXMin(Action response);
    IBoundaryHandlerBuilder OnOutOfBoundary(Action response);
    IBoundaryHandler Build();
}
class RectBoundaryHandler : IBoundaryHandlerBuilder, IBoundaryHandler
{
    public RectBoundaryHandler(Rect boundary)
    {
        this.boundary = boundary;
    }
    private Action onAboveYMax = null;
    private Action onBelowYMin = null;
    private Action onBelowXMin = null;
    private Action onAboveXMax = null;
    private Action onOutOfBondary = null;
    private Rect boundary;
    public IBoundaryHandler Build()
    {
        return this;
    }

    public void Handle(Vector2 position)
    {
        bool isOut = false;
        if (position.x < boundary.xMin)
        {
            if(onBelowXMin!=null) { onBelowXMin(); }
            isOut = true;
        }
        if (position.x > boundary.xMax)
        {
            if (onAboveXMax != null) { onAboveXMax(); }
            isOut = true;
        }
        if (position.y > boundary.yMax)
        {
            if (onAboveYMax != null) { onAboveYMax(); }
            isOut = true;
        }
        if (position.y < boundary.yMin)
        {
            if(onBelowYMin!=null) { onBelowYMin(); }
            isOut = true;
        }
        if (isOut)
        {
            if(onOutOfBondary!=null) { onOutOfBondary(); }
        }
    }

    public IBoundaryHandlerBuilder OnAboveYMax(Action response)
    {
        onAboveYMax = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnBelowYMin(Action response)
    {
        onBelowYMin = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnBelowXMin(Action response)
    {
        onBelowXMin = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnOutOfBoundary(Action response)
    {
        onOutOfBondary = response;
        return this;
    }

    public IBoundaryHandlerBuilder OnAboveXMax(Action response)
    {
        onAboveXMax = response;
        return this;
    }
}

class BoundaryUtils{
    /// <summary>
    /// Provide a utility instance that will allow specifying fluent boundary checks for the given rect. 
    /// 
    /// This allows for code to be (arguably) much more readable. However this is at the expensive of speed due to the numer of lambdas involved. 
    /// Ideally the compiler would allow us to specify that these callbacks be inlined, the calls rewritten with a compiler plugin (apt/ksp like), or some other
    /// such compile time optimization.
    /// </summary>
    /// <param name="boundary"></param>
    /// <returns></returns>    
    public static IBoundaryHandlerBuilder ForBoundary(Rect boundary)
    {
        return new RectBoundaryHandler(boundary);
    }
}

