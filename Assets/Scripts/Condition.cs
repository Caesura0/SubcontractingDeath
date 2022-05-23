using System.Collections.Generic;
using System.Linq;
using UnityEngine;



    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        private List<Disjunction> and = new List<Disjunction>();

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            List<IPredicateEvaluator> evalList = evaluators.ToList();
            foreach (Disjunction dis in and)
            {
                if (!dis.Check(evalList))
                {
                    return false;
                }
            }
            return true;
        }

        [System.Serializable]
        public partial class Disjunction
        {
            [SerializeField] private List<Predicate> or = new List<Predicate>();

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                var evalList = evaluators.ToList();
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evalList))
                    {
                        return true;
                    }
                }
                return false;
            }

        }
        [System.Serializable]
        public class Predicate
        {

            [SerializeField] EPredicate predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate = false;



            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null)
                    {
                        continue;
                    }

                    if (result == negate) { return false; }


                }
                return true;
            }



        }
    }

