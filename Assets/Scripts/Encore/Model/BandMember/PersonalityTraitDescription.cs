namespace Encore.Model.BandMember
{
    public static class PersonalityTraitDescription
    {
        public static string For(PersonalityTraits trait)
        {
            return trait switch
            {
                // Positive personality traits
                PersonalityTraits.Charismatic => "Easily attracts and influences others.",
                PersonalityTraits.Diligent => "Shows persistent effort and hard work.",
                PersonalityTraits.Creative => "Thinks outside the box and generates innovative ideas.",
                PersonalityTraits.Resilient => "Bounces back quickly from setbacks.",
                PersonalityTraits.Empathetic => "Understands and shares the feelings of others.",
                PersonalityTraits.Ambitious => "Has a strong desire to achieve success.",
                PersonalityTraits.Adaptable => "Adjusts easily to new conditions.",
                PersonalityTraits.Confident => "Believes in one's own abilities.",
                PersonalityTraits.Patient => "Able to wait calmly for desired outcomes.",
                PersonalityTraits.Optimistic => "Maintains a positive outlook on life.",
                PersonalityTraits.Humorous => "Possesses a good sense of humor.",
                PersonalityTraits.Analytical => "Able to analyze information effectively.",
                PersonalityTraits.Loyal => "Shows strong allegiance and support.",
                PersonalityTraits.Resourceful => "Finds quick and clever ways to overcome difficulties.",
                PersonalityTraits.Passionate => "Shows intense enthusiasm for something.",
                PersonalityTraits.Observant => "Pays close attention to details.",
                PersonalityTraits.Strategic => "Plans effectively to achieve long-term goals.",
                PersonalityTraits.Supportive => "Provides encouragement and assistance to others.",
                PersonalityTraits.Innovative => "Introduces new ideas and methods.",
                PersonalityTraits.Disciplined => "Maintains self-control and orderliness.",
                PersonalityTraits.Collaborative => "Works well with others towards a common goal.",
                // Negative personality traits
                PersonalityTraits.Lazy => "Avoids work and effort.",
                PersonalityTraits.Arrogant => "Has an inflated sense of one's own importance.",
                PersonalityTraits.Impulsive => "Acts without thinking about consequences.",
                PersonalityTraits.Stubborn => "Unwilling to change one's mind or course of action.",
                PersonalityTraits.Pessimistic => "Tends to see the worst aspect of things.",
                PersonalityTraits.Indecisive => "Struggles to make decisions.",
                PersonalityTraits.Aloof => "Distant and uninvolved.",
                PersonalityTraits.Cynical => "Distrustful of others' motives.",
                PersonalityTraits.Reckless => "Acts without regard for danger or consequences.",
                PersonalityTraits.Selfish => "Concerned primarily with oneself.",
                PersonalityTraits.Inflexible => "Unwilling to change or compromise.",
                PersonalityTraits.Distrustful => "Lacks trust in others.",
                PersonalityTraits.Complacent => "Self-satisfied and unaware of potential dangers.",
                PersonalityTraits.Apathetic => "Shows little interest or concern.",
                PersonalityTraits.Overcritical => "Excessively critical of others.",
                PersonalityTraits.Withdrawn => "Prefers to be alone and avoids social interaction.",
                PersonalityTraits.Neglectful => "Fails to give proper attention to responsibilities.",
                PersonalityTraits.Overambitious => "Has excessive ambition that may lead to risky behavior.",
                PersonalityTraits.Hotheaded => "Easily angered and quick to react.",
                PersonalityTraits.Manipulative => "Skilled at influencing others for personal gain.",
                _ => "No description available."
            };
        }
    }
}