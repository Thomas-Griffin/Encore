namespace Encore.Model.BandMember
{
    public static class PersonalityTraitDescriptions
    {
        public static string GetDescription(PersonalityTraits trait)
        {
            switch (trait)
            {
                // Positive personality traits
                case PersonalityTraits.Charismatic:
                    return "Easily attracts and influences others.";
                case PersonalityTraits.Diligent:
                    return "Shows persistent effort and hard work.";
                case PersonalityTraits.Creative:
                    return "Thinks outside the box and generates innovative ideas.";
                case PersonalityTraits.Resilient:
                    return "Bounces back quickly from setbacks.";
                case PersonalityTraits.Empathetic:
                    return "Understands and shares the feelings of others.";
                case PersonalityTraits.Ambitious:
                    return "Has a strong desire to achieve success.";
                case PersonalityTraits.Adaptable:
                    return "Adjusts easily to new conditions.";
                case PersonalityTraits.Confident:
                    return "Believes in one's own abilities.";
                case PersonalityTraits.Patient:
                    return "Able to wait calmly for desired outcomes.";
                case PersonalityTraits.Optimistic:
                    return "Maintains a positive outlook on life.";
                case PersonalityTraits.Humorous:
                    return "Possesses a good sense of humor.";
                case PersonalityTraits.Analytical:
                    return "Able to analyze information effectively.";
                case PersonalityTraits.Loyal:
                    return "Shows strong allegiance and support.";
                case PersonalityTraits.Resourceful:
                    return "Finds quick and clever ways to overcome difficulties.";
                case PersonalityTraits.Passionate:
                    return "Shows intense enthusiasm for something.";
                case PersonalityTraits.Observant:
                    return "Pays close attention to details.";
                case PersonalityTraits.Strategic:
                    return "Plans effectively to achieve long-term goals.";
                case PersonalityTraits.Supportive:
                    return "Provides encouragement and assistance to others.";
                case PersonalityTraits.Innovative:
                    return "Introduces new ideas and methods.";
                case PersonalityTraits.Disciplined:
                    return "Maintains self-control and orderliness.";
                case PersonalityTraits.Collaborative:
                    return "Works well with others towards a common goal.";

                // Negative personality traits
                case PersonalityTraits.Lazy:
                    return "Avoids work and effort.";
                case PersonalityTraits.Arrogant:
                    return "Has an inflated sense of one's own importance.";
                case PersonalityTraits.Impulsive:
                    return "Acts without thinking about consequences.";
                case PersonalityTraits.Stubborn:
                    return "Unwilling to change one's mind or course of action.";
                case PersonalityTraits.Pessimistic:
                    return "Tends to see the worst aspect of things.";
                case PersonalityTraits.Indecisive:
                    return "Struggles to make decisions.";
                case PersonalityTraits.Aloof:
                    return "Distant and uninvolved.";
                case PersonalityTraits.Cynical:
                    return "Distrustful of others' motives.";
                case PersonalityTraits.Reckless:
                    return "Acts without regard for danger or consequences.";
                case PersonalityTraits.Selfish:
                    return "Concerned primarily with oneself.";
                case PersonalityTraits.Inflexible:
                    return "Unwilling to change or compromise.";
                case PersonalityTraits.Distrustful:
                    return "Lacks trust in others.";
                case PersonalityTraits.Complacent:
                    return "Self-satisfied and unaware of potential dangers.";
                case PersonalityTraits.Apathetic:
                    return "Shows little interest or concern.";
                case PersonalityTraits.Overcritical:
                    return "Excessively critical of others.";
                case PersonalityTraits.Withdrawn:
                    return "Prefers to be alone and avoids social interaction.";
                case PersonalityTraits.Neglectful:
                    return "Fails to give proper attention to responsibilities.";
                case PersonalityTraits.Overambitious:
                    return "Has excessive ambition that may lead to risky behavior.";
                case PersonalityTraits.Hotheaded:
                    return "Easily angered and quick to react.";
                case PersonalityTraits.Manipulative:
                    return "Skilled at influencing others for personal gain.";
                default:
                    return "No description available.";
            }
        }
    }
}