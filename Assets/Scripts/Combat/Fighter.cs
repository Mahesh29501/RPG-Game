using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat 
{ 
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.2f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = 0f;
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime; 

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);    
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform); 
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // this will trigger the event
                AttackTrigger();
                timeSinceLastAttack = Mathf.Infinity;
            } 

        }

        private void AttackTrigger()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if (target == null) { return; }
            target.TakeDamage(weaponDamage);
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttcak(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; } 
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().startAction(this); 
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            stopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void stopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }
}
