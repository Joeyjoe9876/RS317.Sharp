
sealed class Skills
{
	public static sealed int skillsCount = 25;
	public static sealed String[] skillNames = { "attack", "defence", "strength", "hitpoints", "ranged", "prayer",
			"magic", "cooking", "woodcutting", "fletching", "fishing", "firemaking", "crafting", "smithing", "mining",
			"herblore", "agility", "thieving", "slayer", "farming", "runecraft", "-unused-", "-unused-", "-unused-",
			"-unused-" };
	public static sealed boolean[] skillEnabled = { true, true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, false, true, false, false, false, false };

}
