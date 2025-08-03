using System.Collections.Generic;

namespace Perennial.Plants.Mutations
{
    public struct MutationSelectionResult
    {
        public MutationCandidate? SelectedMutation { get; set; }
        public List<MutationCandidate> AllCandidates { get; set; }
    }
}