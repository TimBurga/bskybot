namespace BskyBot.Bots;

public class CheneyFactProvider
{
    private readonly string[] _facts = [
        "who redefined torture as enhanced interrogation techniques",
        "champion of waterboarding",
        "who championed torture that a Senate investigation determined was not an effective way to gain intelligence from detainees",
        "who attempted to block the creation of the 9/11 commission that eventually determined he lied",
        "who repeatedly lied to the public about connections between Iraq and terrorists",
        "who said he has no problem with torture as long as it achieves objectives",
        "whose approval rating stood at a staggeringly low 13 percent at the end of his tenure as Vice President",
        "who proudly declared he'd authorize torture again in a minute",
        "who continues to insist that torture worked despite the contrary findings of a Senate investigation",
        "who insists that rectal rehydration and feeding of detainees was done for medical reasons despite medical experts saying otherwise",
        "who continues to argue that anything short of causing pain equal to that of organ failure is not torture",
        "who was unmoved by the revelation that 25% of the prisoners who were tortured were found innocent",
        "the architect of some of the most disastrous foreign and domestic policies of the early 21st century",
        "who pushed for a war based on known false premises of weapons of mass destruction and a link between Iraq and Al Qaeda",
        "who is responsible for an estimated half a million or more Iraqi civilian deaths",
        "who advocated strongly for the passage of the Patriot Act which granted the government sweeping surveillance powers over US citizens",
        "whose policies eroded civil liberties, violated human rights, destabilised entire regions, and left a legacy of fear, instability, and anger that continues to haunt the world today",
        "who outed an undercover CIA officer to the media in retribution for her husband's public opposition to the impending invasion of Iraq"
    ];

    public string Random()
    {
        var rand = new Random();
        var num = rand.Next(0, _facts.Length - 1);
        return _facts[num];
    }
}
