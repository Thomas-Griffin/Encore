using Encore.Model.BandMember;

namespace Encore.Systems.Personality
{
    // Defines pairs of conflicting personality traits, that cannot coexist on the same band member
    public static class PersonalityConflicts
    {
        public static readonly (PersonalityTraits prefer, PersonalityTraits remove)[] Conflicts = {
            (PersonalityTraits.Diligent, PersonalityTraits.Lazy),
            (PersonalityTraits.Disciplined, PersonalityTraits.Lazy),
            (PersonalityTraits.Optimistic, PersonalityTraits.Pessimistic),
            (PersonalityTraits.Ambitious, PersonalityTraits.Complacent),
            (PersonalityTraits.Supportive, PersonalityTraits.Selfish),
            (PersonalityTraits.Collaborative, PersonalityTraits.Manipulative),
            (PersonalityTraits.Loyal, PersonalityTraits.Manipulative),
            (PersonalityTraits.Adaptable, PersonalityTraits.Inflexible),
            (PersonalityTraits.Resourceful, PersonalityTraits.Neglectful),
            (PersonalityTraits.Resilient, PersonalityTraits.Withdrawn)
        };
    }
}

