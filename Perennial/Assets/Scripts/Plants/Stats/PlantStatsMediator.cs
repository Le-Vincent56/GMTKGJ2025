using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Extensions;

namespace Perennial.Plants.Stats
{
    public class PlantStatsMediator
    {
        private readonly LinkedList<StatModifier> _modifiers = new LinkedList<StatModifier>();

        public event EventHandler<Query> Queries;
        
        /// <summary>
        /// Perform a query to find a Stat Modifier
        /// </summary>
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        /// <summary>
        /// Add a Stat Modifier to the Stats Mediator
        /// </summary>
        public void AddModifier(StatModifier modifier)
        {
            // Add the modifier to the end of the LinkedList
            _modifiers.AddLast(modifier);
            Queries += modifier.Modify;

            // Set a disposal callback to remove the modifier
            // from the mediator
            modifier.OnDispose += _ =>
            {
                _modifiers.Remove(modifier);
                Queries -= modifier.Modify;
            };
        }

        /// <summary>
        /// Remove a Stat Modifier from the Stats Mediator
        /// </summary>
        public void RemoveModifier(SerializableGuid id)
        {
            // Get the first node of the LinkedList
            LinkedListNode<StatModifier> node = _modifiers.First;

            // Get a default case for looped removal
            bool removed = false;

            // Loop through the LinkedList
            while (node != null && !removed)
            {
                // Skip if the IDs mismatch
                if (node.Value.ID != id) continue;
                
                // Mark the node for removal and break the loop
                node.Value.MarkedForRemoval = true;
                removed = true;
            }
        }

        /// <summary>
        /// Determine whether any modifiers have the give ID
        /// </summary>
        public bool ContainsModifierID(SerializableGuid id) => _modifiers.Any(x => x.ID == id);

        /// <summary>
        /// Update the Stat Mediator
        /// </summary>
        public void Update()
        {
            // Get the first node of the LinkedList
            LinkedListNode<StatModifier> node = _modifiers.First;
            
            // Loop through each node
            while (node != null)
            {
                // Get the next node
                LinkedListNode<StatModifier> nextNode = node.Next;
                
                // Check if the current node is marked for removal
                if (node.Value.MarkedForRemoval)
                {
                    // Remove the stat modifier and dispose it
                    _modifiers.Remove(node);
                    node.Value.Dispose();
                }

                // Set the current node to the next node
                node = nextNode;
            }
        }
    }   
}