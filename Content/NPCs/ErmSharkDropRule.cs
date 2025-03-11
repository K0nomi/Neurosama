using Neurosama.Content.NPCs;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;


public class ErmsharkLastDescendantDropRule : IItemDropRuleCondition
{
	private static LocalizedText Description;

	public ErmsharkLastDescendantDropRule()
	{
		Description = Language.GetText("Mods.Neurosama.DropConditions.Ermshark");
	}


	/// <summary>
	/// Checks if the NPC is the last <see cref="ErmShark"/> alive.
	/// </summary>
	/// <param name="info">The drop attempt information.</param>
	/// <returns><c>true</c> if the NPC is the last <see cref="ErmShark"/> alive, <c>false</c> otherwise.</returns>
	public bool CanDrop(DropAttemptInfo info)
	{
		// i have no idea if this is the best way, but it doesn't appear to lag even with a lot of erms
		return info.npc.ModNPC is ErmShark ermShark && ermShark.IsLastDescendant();
	}

	public bool CanShowItemDropInUI()
	{
		return true;
	}

	public string GetConditionDescription()
	{
		return Description.Value;
	}
}