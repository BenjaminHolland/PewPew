using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUnit
{
    public int Health { get; }
    public void HitWith(object other);
}

/// <summary>
/// https://www.sunnyvalleystudio.com/blog/unity-csharp-9-features
/// </summary>
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}

public record DamageEffect(int NormalDamage);