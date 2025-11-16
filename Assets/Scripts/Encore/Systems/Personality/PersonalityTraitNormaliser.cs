using System.Collections.Generic;
using System.Linq;
using Encore.Model.BandMember;
using UnityEngine;

namespace Encore.Systems.Personality
{
    public static class PersonalityTraitNormaliser
    {
        public static List<PersonalityTraits> Normalise(IEnumerable<PersonalityTraits> traits)
        {
            if (traits == null) return new List<PersonalityTraits>();

            List<PersonalityTraits> traitList = traits is List<PersonalityTraits> l ? l : traits.ToList();

            (PersonalityTraits prefer, PersonalityTraits remove)[] conflicts = PersonalityConflicts.Conflicts;

            HashSet<PersonalityTraits> present = new(traitList);

            HashSet<PersonalityTraits> toRemove = new();
            foreach ((PersonalityTraits prefer, PersonalityTraits remove) in conflicts)
            {
                if (present.Contains(prefer) && present.Contains(remove))
                {
                    toRemove.Add(remove);
                }
            }

            HashSet<PersonalityTraits> seen = new();

            return traitList.Where(personalityTrait => !toRemove.Contains(personalityTrait)).Where(personalityTrait => seen.Add(personalityTrait)).ToList();
        }
    }
}