using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public interface IPredicateEvaluator
    {
        bool? Evaluate(EPredicate predicate, string[] parameters);

    }
