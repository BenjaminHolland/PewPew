
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Interactions:MonoBehaviour
{
    private Dictionary<Tuple<Type, Type>, List<object>> interactions = new Dictionary<Tuple<Type, Type>, List<object>>();
    public void AddEffect<T,U>(object effect)
    {
        var t = typeof(T);
        var u = typeof(U);
        var key = Tuple.Create(t, u);
        if (!interactions.ContainsKey(key))
        {
            interactions[key] = new List<object>() { effect };
        }
        else
        {
            interactions[key].Add(effect);
        }
    }
    public List<object> GetEffects(Type from, Type to)
    {
        var key = Tuple.Create(from, to);
        if (interactions.ContainsKey(key))
        {
            return interactions[key];
        }
        else
        {
            // Cache this.
            return new List<object>();
        }
    }
    void Start()
    {
        AddEffect<BulletController, AsteroidController>(new DamageEffect(NormalDamage: 1));
        AddEffect<BulletController, CrabController>(new DamageEffect(NormalDamage: 1));
        AddEffect<BulletController, BananaController>(new DamageEffect(NormalDamage: 1));

    }
}
